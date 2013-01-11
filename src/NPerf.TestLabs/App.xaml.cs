using System.Windows;
using DaveSexton.Labs;

namespace NPerf.TestLabs
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var catalog = new Catalog();

            var controller = new WindowsLabController(catalog);

            controller.Show();
        }
    }
}