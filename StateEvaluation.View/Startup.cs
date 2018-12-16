using StateEvaluation.Repository.Providers;
using System.Windows;
using Application = System.Windows.Application;

namespace StateEvaluation
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow(new DataRepository());
            MainWindow.ShowDialog();
        }

    }
}
