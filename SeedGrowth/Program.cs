using System;
using System.Windows.Forms;
using SeedGrowth.Controllers;
namespace SeedGrowth
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
            SetupView view = new SetupView();
            SeedGrowthController controller = new SeedGrowthController(view);        
            Application.Run(view);
        }
    }
}
