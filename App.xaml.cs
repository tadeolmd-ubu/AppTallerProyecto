using AppTaller.EF;
using AppTaller.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace AppTaller
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;

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

            _ = CheckForUpdatesAsync();
        }

        private async Task CheckForUpdatesAsync()
        {
            try
            {
                await UpdateService.CheckForUpdatesAsync();
            }
            catch
            {
            }
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Error inesperado:\n{e.Exception.GetType().Name}\n{e.Exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        private void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            MessageBox.Show($"Error crítico:\n{ex?.GetType().Name}\n{ex?.Message}", "Error crítico", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
