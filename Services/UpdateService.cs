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

                    using (var p = Process.Start(new ProcessStartInfo
                    {
                        FileName = psPath,
                        Arguments = $"-NoProfile -Command \"Expand-Archive -Path '{zipPath}' -DestinationPath '{tempDir}' -Force\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }))
                        p?.WaitForExit();

                    var scriptPath = Path.Combine(tempDir, "update.ps1");
                    File.WriteAllText(scriptPath,
                        "$retry = 10\n" +
                        "do {\n" +
                        "    $retry--\n" +
                        "    try {\n" +
                        $"        Get-ChildItem '{tempDir}\\*' -Exclude update.ps1 | Copy-Item -Destination '{AppFolder}' -Recurse -Force -ErrorAction Stop\n" +
                        "        break\n" +
                        "    } catch {\n" +
                        "        Start-Sleep 2\n" +
                        "    }\n" +
                        "} while ($retry -gt 0)\n" +
                        $"Start-Process '{AppFolder}AppTaller.exe'\n" +
                        $"Remove-Item '{tempDir}' -Recurse -Force\n");

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = psPath,
                        Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{scriptPath}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
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
