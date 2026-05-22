using AppTaller.Services;
using System;
using System.Windows;

namespace AppTaller
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                await UpdateService.CheckForUpdatesAsync();
            }
            catch
            {
            }
        }
    }
}
