﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GemSwap
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new FormGemSwap());
              //  Application.Run(new Form1());
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
