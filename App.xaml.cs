using AppTaller.EF;
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
                using (var db = new efAppDbContext())
                    db.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con la base de datos:\n{ex.Message}\n\nVerifica que SQL Server esté instalado y ejecutándose.", "Error BD", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
                return;
            }

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
