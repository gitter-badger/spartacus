﻿/*
The MIT License (MIT)

Copyright (c) 2014,2015 William Ivanski

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Data;
using System.Data.OracleClient;

namespace Spartacus.Database
{
    /// <summary>
    /// Classe Spartacus.Database.Oracle.
    /// Herda da classe <see cref="Spartacus.Database.Generic"/>.
    /// Utiliza o Oracle .NET Provider para acessar um SGBD Oracle.
    /// </summary>
    public class Oracle : Spartacus.Database.Generic
    {
        /// <summary>
        /// String de conexão para acessar o banco.
        /// </summary>
        private string v_connectionstring;

        /// <summary>
        /// Conexão com o banco de dados.
        /// </summary>
        private System.Data.OracleClient.OracleConnection v_con;

        /// <summary>
        /// Comando para conexão com o banco de dados.
        /// </summary>
        private System.Data.OracleClient.OracleCommand v_cmd;

        /// <summary>
        /// Leitor de dados do banco de dados.
        /// </summary>
        private System.Data.OracleClient.OracleDataReader v_reader;

        /// <summary>
        /// Linha atual da QueryBlock.
        /// </summary>
        private uint v_currentrow;


        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="Spartacus.Database.Oracle"/>.
        /// Cria a string de conexão ao banco.
        /// </summary>
        /// <param name='p_host'>
        /// Hostname ou IP onde o banco de dados está localizado.
        /// </param>
        /// <param name='p_port'>
        /// Porta TCP para conectar-se ao SGBG.
        /// </param>
        /// <param name='p_service'>
        /// Nome do serviço que representa o banco ao qual desejamos nos conectar.
        /// </param>
        /// <param name='p_user'>
        /// Usuário ou schema para se conectar ao banco de dados.
        /// </param>
        /// <param name='p_password'>
        /// A senha do usuário ou schema.
        /// </param>
        public Oracle (string p_host, string p_port, string p_service, string p_user, string p_password)
            : base(p_host, p_port, p_service, p_user, p_password)
        {
            this.v_connectionstring = "User ID=" + p_user + ";";
            this.v_connectionstring += "Password=" + p_password + ";Pooling=false;";
            this.v_connectionstring += "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)";
            this.v_connectionstring += "(HOST=" + p_host + ")";
            this.v_connectionstring += "(PORT=" + p_port + "))(CONNECT_DATA=(SERVER=DEDICATED)";
            this.v_connectionstring += "(SERVICE_NAME=" + p_service + ")))";

            this.v_con = null;
            this.v_cmd = null;
            this.v_reader = null;
        }

        /// <summary>
        /// Cria um banco de dados.
        /// </summary>
        /// <param name="p_name">Nome do arquivo de banco de dados a ser criado.</param>
        public override void CreateDatabase(string p_name)
        {
        }

        /// <summary>
        /// Cria um banco de dados.
        /// </summary>
        public override void CreateDatabase()
        {
        }

        /// <summary>
        /// Abre a conexão com o banco de dados.
        /// </summary>
        public override void Open()
        {
            try
            {
                this.v_con = new System.Data.OracleClient.OracleConnection(this.v_connectionstring);
                this.v_con.Open();
                this.v_cmd = new System.Data.OracleClient.OracleCommand();
                this.v_cmd.Connection = this.v_con;
            }
            catch (System.Data.OracleClient.OracleException e)
            {
                throw new Spartacus.Database.Exception(e);
            }
        }

        /// <summary>
        /// Realiza uma consulta no banco de dados, armazenando os dados de retorno em um <see cref="System.Data.DataTable"/>.
        /// </summary>
        /// <param name='p_sql'>
        /// Código SQL a ser consultado no banco de dados.
        /// </param>
        /// <param name='p_tablename'>
        /// Nome virtual da tabela onde deve ser armazenado o resultado, para fins de cache.
        /// </param>
        /// <returns>Retorna uma <see cref="System.Data.DataTable"/> com os dados de retorno da consulta.</returns>
        public override System.Data.DataTable Query(string p_sql, string p_tablename)
        {
            System.Data.DataTable v_table = null;
            System.Data.DataRow v_row;

            if (this.v_con == null)
            {
                try
                {
                    this.v_con = new System.Data.OracleClient.OracleConnection(this.v_connectionstring);
                    this.v_con.Open();
                    this.v_cmd = new System.Data.OracleClient.OracleCommand(p_sql, this.v_con);
                    this.v_reader = this.v_cmd.ExecuteReader();

                    v_table = new System.Data.DataTable(p_tablename);
                    for (int i = 0; i < v_reader.FieldCount; i++)
                        v_table.Columns.Add(this.FixColumnName(this.v_reader.GetName(i)), typeof(string));

                    while (this.v_reader.Read())
                    {
                        v_row = v_table.NewRow();
                        for (int i = 0; i < this.v_reader.FieldCount; i++)
                            v_row[i] = this.v_reader[i].ToString();
                        v_table.Rows.Add(v_row);
                    }

                    return v_table;
                }
                catch (System.Data.OracleClient.OracleException e)
                {
                    throw new Spartacus.Database.Exception(e);
                }
                finally
                {
                    this.v_reader.Close();
                    this.v_reader = null;
                    this.v_cmd.Dispose();
                    this.v_cmd = null;
                    this.v_con.Close();
                    this.v_con = null;
                }
            }
            else
            {
                try
                {
                    this.v_cmd.CommandText = p_sql;
                    this.v_reader = this.v_cmd.ExecuteReader();

                    v_table = new System.Data.DataTable(p_tablename);
                    for (int i = 0; i < v_reader.FieldCount; i++)
                        v_table.Columns.Add(this.FixColumnName(this.v_reader.GetName(i)), typeof(string));

                    while (this.v_reader.Read())
                    {
                        v_row = v_table.NewRow();
                        for (int i = 0; i < this.v_reader.FieldCount; i++)
                            v_row[i] = this.v_reader[i].ToString();
                        v_table.Rows.Add(v_row);
                    }

                    return v_table;
                }
                catch (System.Data.OracleClient.OracleException e)
                {
                    throw new Spartacus.Database.Exception(e);
                }
                finally
                {
                    this.v_reader.Close();
                    this.v_reader = null;
                }
            }
        }

        /// <summary>
        /// Realiza uma consulta no banco de dados, armazenando os dados de retorno em um <see creg="System.Data.DataTable"/>.
        /// Utiliza um DataReader para buscar em blocos. Conexão com o banco precisa estar aberta.
        /// </summary>
        /// <param name='p_sql'>
        /// Código SQL a ser consultado no banco de dados.
        /// </param>
        /// <param name='p_tablename'>
        /// Nome virtual da tabela onde deve ser armazenado o resultado, para fins de cache.
        /// </param>
        /// <param name='p_startrow'>
        /// Número da linha inicial.
        /// </param>
        /// <param name='p_endrow'>
        /// Número da linha final.
        /// </param>
        /// <param name='p_hasmoredata'>
        /// Indica se ainda há mais dados a serem lidos.
        /// </param>
        public override System.Data.DataTable Query(string p_sql, string p_tablename, uint p_startrow, uint p_endrow, out bool p_hasmoredata)
        {
            System.Data.DataTable v_table = null;
            System.Data.DataRow v_row;

            try
            {
                if (this.v_reader == null)
                {
                    this.v_cmd.CommandText = p_sql;
                    this.v_reader = this.v_cmd.ExecuteReader();
                    this.v_currentrow = 0;
                }

                v_table = new System.Data.DataTable(p_tablename);
                for (int i = 0; i < v_reader.FieldCount; i++)
                    v_table.Columns.Add(this.FixColumnName(this.v_reader.GetName(i)), typeof(string));

                while (this.v_reader.Read())
                {
                    if (this.v_currentrow >= p_startrow && this.v_currentrow <= p_endrow)
                    {
                        v_row = v_table.NewRow();
                        for (int i = 0; i < this.v_reader.FieldCount; i++)
                            v_row[i] = this.v_reader[i].ToString();
                        v_table.Rows.Add(v_row);
                    }

                    if (this.v_currentrow > p_endrow)
                        break;

                    this.v_currentrow++;
                }

                if (this.v_currentrow > p_endrow)
                {
                    this.v_reader.Close();
                    this.v_reader = null;
                    p_hasmoredata = false;
                }
                else
                    p_hasmoredata = true;

                return v_table;
            }
            catch (System.Data.OracleClient.OracleException e)
            {
                throw new Spartacus.Database.Exception(e);
            }
        }

        /// <summary>
        /// Executa um código SQL no banco de dados.
        /// </summary>
        /// <param name='p_sql'>
        /// Código SQL a ser executado no banco de dados.
        /// </param>
        public override void Execute(string p_sql)
        {
            if (this.v_con == null)
            {
                try
                {
                    this.v_con = new System.Data.OracleClient.OracleConnection(this.v_connectionstring);
                    this.v_con.Open();
                    this.v_cmd = new System.Data.OracleClient.OracleCommand(Spartacus.Database.Command.RemoveUnwantedCharsExecute(p_sql), this.v_con);
                    this.v_cmd.ExecuteNonQuery();
                }
                catch (System.Data.OracleClient.OracleException e)
                {
                    throw new Spartacus.Database.Exception(e);
                }
                finally
                {
                    this.v_cmd.Dispose();
                    this.v_cmd = null;
                    this.v_con.Close();
                    this.v_con = null;
                }
            }
            else
            {
                try
                {
                    this.v_cmd.CommandText = Spartacus.Database.Command.RemoveUnwantedCharsExecute(p_sql);
                    this.v_cmd.ExecuteNonQuery();
                }
                catch (System.Data.OracleClient.OracleException e)
                {
                    throw new Spartacus.Database.Exception(e);
                }
            }
        }

        /// <summary>
        /// Realiza uma consulta no banco de dados, armazenando um único dado de retorno em uma string.
        /// </summary>
        /// <returns>
        /// string com o dado de retorno.
        /// </returns>
        /// <param name='p_sql'>
        /// Código SQL a ser consultado no banco de dados.
        /// </param>
        public override string ExecuteScalar(string p_sql)
        {
            if (this.v_con == null)
            {
                try
                {
                    this.v_con = new System.Data.OracleClient.OracleConnection(this.v_connectionstring);
                    this.v_con.Open();
                    this.v_cmd = new System.Data.OracleClient.OracleCommand(Spartacus.Database.Command.RemoveUnwantedCharsExecute(p_sql), this.v_con);
                    return (string) this.v_cmd.ExecuteScalar();
                }
                catch (System.Data.OracleClient.OracleException e)
                {
                    throw new Spartacus.Database.Exception(e);
                }
                finally
                {
                    this.v_cmd.Dispose();
                    this.v_cmd = null;
                    this.v_con.Close();
                    this.v_con = null;
                }
            }
            else
            {
                try
                {
                    this.v_cmd.CommandText = Spartacus.Database.Command.RemoveUnwantedCharsExecute(p_sql);
                    return (string) this.v_cmd.ExecuteScalar();
                }
                catch (System.Data.OracleClient.OracleException e)
                {
                    throw new Spartacus.Database.Exception(e);
                }
            }
        }

        /// <summary>
        /// Fecha a conexão com o banco de dados.
        /// </summary>
        public override void Close()
        {
            this.v_cmd.Dispose();
            this.v_cmd = null;
            this.v_con.Close();
            this.v_con = null;
        }

        /// <summary>
        /// Deleta um banco de dados.
        /// </summary>
        /// <param name="p_name">Nome do banco de dados a ser deletado.</param>
        public override void DropDatabase(string p_name)
        {
        }

        /// <summary>
        /// Deleta o banco de dados conectado atualmente.
        /// </summary>
        public override void DropDatabase()
        {
        }

        /// <summary>
        /// Transfere dados do banco de dados atual para um banco de dados de destino.
        /// Conexão com o banco de destino precisa estar aberta.
        /// </summary>
        /// <returns>Número de linhas transferidas.</returns>
        /// <param name="p_query">Consulta SQL para buscar os dados no banco atual.</param>
        /// <param name="p_insert">Comando de inserção para inserir cada linha no banco de destino.</param>
        /// <param name="p_destdatabase">Conexão com o banco de destino.</param>
        public override int Transfer(string p_query, string p_insert, Spartacus.Database.Generic p_destdatabase)
        {
            int v_transfered = 0;
            string v_insert;

            if (this.v_con == null)
            {
                try
                {
                    this.v_con = new System.Data.OracleClient.OracleConnection(this.v_connectionstring);
                    this.v_con.Open();
                    this.v_cmd = new System.Data.OracleClient.OracleCommand(p_query, this.v_con);
                    this.v_reader = this.v_cmd.ExecuteReader();

                    while (v_reader.Read())
                    {
                        v_insert = p_insert;
                        for (int i = 0; i < v_reader.FieldCount; i++)
                            v_insert = v_insert.Replace("#" + this.FixColumnName(v_reader.GetName(i)).ToLower() + "#", v_reader[i].ToString());

                        p_destdatabase.Execute(v_insert);
                        v_transfered++;
                    }

                    return v_transfered;
                }
                catch (System.Data.OracleClient.OracleException e)
                {
                    throw new Spartacus.Database.Exception(e);
                }
                finally
                {
                    this.v_reader.Close();
                    this.v_reader = null;
                    this.v_cmd.Dispose();
                    this.v_cmd = null;
                    this.v_con.Close();
                    this.v_con = null;
                }
            }
            else
            {
                try
                {
                    this.v_cmd.CommandText = p_query;
                    this.v_reader = this.v_cmd.ExecuteReader();

                    while (v_reader.Read())
                    {
                        v_insert = p_insert;
                        for (int i = 0; i < v_reader.FieldCount; i++)
                            v_insert = v_insert.Replace("#" + this.FixColumnName(v_reader.GetName(i)).ToLower() + "#", v_reader[i].ToString());

                        p_destdatabase.Execute(v_insert);
                        v_transfered++;
                    }

                    return v_transfered;
                }
                catch (System.Data.OracleClient.OracleException e)
                {
                    throw new Spartacus.Database.Exception(e);
                }
                finally
                {
                    this.v_reader.Close();
                    this.v_reader = null;
                }
            }
        }
    }
}
    