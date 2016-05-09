﻿/*
The MIT License (MIT)

Copyright (c) 2014-2016 William Ivanski

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

namespace Spartacus.PollyDB
{
    /// <summary>
    /// Classe Column.
    /// Representa uma coluna de qualquer Relação.
    /// </summary>
    public class Column
    {
        /// <summary>
        /// Apelido da Relação.
        /// </summary>
        public string v_relationalias;

        /// <summary>
        /// Nome da Coluna.
        /// </summary>
        public string v_name;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="Spartacus.PollyDB.Column"/>.
        /// </summary>
        /// <param name="p_relationalias">Apelido da Relação.</param>
        /// <param name="p_name">Nome da Coluna.</param>
        public Column(string p_relationalias, string p_name)
        {
            this.v_relationalias = p_relationalias;
            this.v_name = p_name;
        }
    }
}