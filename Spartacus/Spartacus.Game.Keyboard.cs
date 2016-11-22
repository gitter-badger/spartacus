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

namespace Spartacus.Game
{
    public class Keyboard
    {
        public Keyboard(System.Windows.Forms.Form p_screen)
        {
            p_screen.KeyPreview = true;
            p_screen.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.OnPreviewKeyDown);
            p_screen.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            p_screen.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
            p_screen.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPressed);
        }

        private void OnPreviewKeyDown(object sender, System.Windows.Forms.PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }

        private void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Console.WriteLine("KeyDown: '" + e.KeyCode + "' down.");
        }

        private void OnKeyPressed(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            Console.WriteLine("KeyPressed: '" + e.KeyChar.ToString() + "' pressed.");
        }

        private void OnKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Console.WriteLine("KeyUp: '" + e.KeyCode + "' up.");
            e.Handled = true;
        }
    }
}