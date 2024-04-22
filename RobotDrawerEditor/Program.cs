using System;
using System.Drawing;
using System.Windows.Forms;


namespace RobotDrawerEditor
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

            ProgramLogic programLogic = new ProgramLogic();
            MainForm mainForm = new MainForm(programLogic);
            Application.Run(mainForm);
        }
    }
}
