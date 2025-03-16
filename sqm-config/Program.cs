namespace sqm_config
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            // Show splash screen
            ApplicationConfiguration.Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Splash splash = new Splash();
            //center splash screen
            splash.StartPosition = FormStartPosition.CenterScreen;

            splash.Show();
            splash.Refresh(); // Ensure it renders immediately

            //close when mainform has loaded

            Form1 mainForm = new Form1();
            splash.Close();

            Application.Run(mainForm);
        }
    }
}