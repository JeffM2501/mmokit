using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ClientApp
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

            LoginForm loginForm = new LoginForm();
            Application.Run(loginForm);

            if (loginForm.Token > 0)
            {
                MessageBox.Show("Play that shit!");
            }
        }
    }
}
