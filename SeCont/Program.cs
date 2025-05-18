using SeContCoreLib;

namespace SeCont
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            OpenGostConfigurator.ConfigureGostCryptographicServices();

            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}