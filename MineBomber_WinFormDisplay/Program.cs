using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MineBomber_WinFormDisplay
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var mineBomberForm = new Form1();
            Application.Run(mineBomberForm);
        }
    }
}
