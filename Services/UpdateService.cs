using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace AppTaller.Services
{
    internal static class UpdateService
    {
        private static readonly string Owner = "tadeolmd-ubu";
        private static readonly string Repo = "AppTallerProyecto";
        private static readonly HttpClient Client = new HttpClient();
        private static readonly string AppFolder = AppDomain.CurrentDomain.BaseDirectory;

        public static async Task CheckForUpdatesAsync()
        {
            try
            {
                Client.DefaultRequestHeaders.UserAgent.ParseAdd("AppTaller-Updater");
                var url = $"https://api.github.com/repos/{Owner}/{Repo}/releases/latest";
                var response = await Client.GetStringAsync(url);

                using (var doc = JsonDocument.Parse(response))
                {
                    var tag = doc.RootElement.GetProperty("tag_name").GetString();

                    if (string.IsNullOrEmpty(tag) || !tag.StartsWith("v"))
                        return;

                    var latestVersion = new Version(tag.TrimStart('v'));
                    var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

                    if (currentVersion >= latestVersion)
                        return;

                    var assets = doc.RootElement.GetProperty("assets");
                    string downloadUrl = null;
                    foreach (var asset in assets.EnumerateArray())
                    {
                        var name = asset.GetProperty("name").GetString();
                        if (name != null && name.EndsWith(".zip"))
                        {
                            downloadUrl = asset.GetProperty("browser_download_url").GetString();
                            break;
                        }
                    }

                    if (downloadUrl == null)
                        return;

                    var result = MessageBox.Show(
                        $"Nueva versión disponible: {tag}\nversión actual: {currentVersion}\n\n¿Descargar e instalar ahora?",
                        "Actualización disponible",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information);

                    if (result != MessageBoxResult.Yes)
                        return;

                    var tempDir = Path.Combine(Path.GetTempPath(), "AppTallerUpdate");
                    if (Directory.Exists(tempDir))
                        Directory.Delete(tempDir, true);
                    Directory.CreateDirectory(tempDir);

                    var zipPath = Path.Combine(tempDir, "update.zip");
                    var zipBytes = await Client.GetByteArrayAsync(downloadUrl);
                    File.WriteAllBytes(zipPath, zipBytes);

                    var psPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.System),
                        "WindowsPowerShell", "v1.0", "powershell.exe");
                    var psi = new ProcessStartInfo
                    {
                        FileName = psPath,
                        Arguments = $"-NoProfile -Command \"Expand-Archive -Path '{zipPath}' -DestinationPath '{tempDir}' -Force\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    using (var p = Process.Start(psi))
                        p?.WaitForExit();

                    var batPath = Path.Combine(AppFolder, "update.bat");
                    var batContent = $@"@echo off
timeout /t 3 /nobreak >nul
xcopy /E /Y ""{tempDir}\*"" ""{AppFolder}""
start """" ""{AppFolder}AppTaller.exe""
rmdir /S /Q ""{tempDir}""
del ""%~f0""
                ";
                    File.WriteAllText(batPath, batContent);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = batPath,
                        UseShellExecute = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    });

                    Application.Current.Shutdown();
                }
            }
            catch
            {
            }
        }
    }
}
