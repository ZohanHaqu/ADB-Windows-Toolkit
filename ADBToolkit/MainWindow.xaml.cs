using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Threading.Tasks;

namespace ADBToolkit
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Disable window resizing and maximize functionality
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStyle = WindowStyle.SingleBorderWindow;
        }

        // Splash Screen
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SplashScreen splash = new SplashScreen("C:\\Users\\zohan\\Downloads\\WixPieOutput\\splash.png");
            splash.Show(true);
            await Task.Delay(3000);
            splash.Close(TimeSpan.FromMilliseconds(500));
        }

        // Check Devices Button
        private void CheckDevices_Click(object sender, RoutedEventArgs e)
        {
            ExecuteCommandAsAdmin("adb devices");
        }

        // Platform Tools Check Button
        private async void CheckPlatformTools_Click(object sender, RoutedEventArgs e)
        {
            string platformToolsPath = @"C:\Program Files (x86)\ADB Tools FOR AWT\";

            if (!Directory.Exists(platformToolsPath))
            {
                var result = MessageBox.Show(
                    "Platform Tools are missing. Would you like to download and install them?",
                    "Missing Platform Tools",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    await InstallPlatformTools();
                }
            }
        }

        // Download and install Platform Tools
        private async Task InstallPlatformTools()
        {
            string url = "https://github.com/ZohanHaqu/PLATFORM-TOOLS-FOR-ADB-WINDOWS-TOOLKIT/releases/download/1.0/ADBToolsForAWT.msi";
            using (HttpClient client = new HttpClient())
            {
                var fileBytes = await client.GetByteArrayAsync(url);
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ADBToolsForAWT.msi");

                File.WriteAllBytes(filePath, fileBytes);
                Process.Start("msiexec", $"/qb /i \"{filePath}\"");
            }
        }

        // Execute ADB Command in a Visible Administrator CMD
        private void ExecuteCommandAsAdmin(string command)
        {
            string platformToolsPath = @"C:\Program Files (x86)\ADB Tools FOR AWT\";

            if (!Directory.Exists(platformToolsPath))
            {
                MessageBox.Show("Platform tools not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string adbCommand = $"cd /d \"{platformToolsPath}\" && {command} & echo. & echo Press any key to close... & pause >nul";

            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/K \"{adbCommand}\"",
                Verb = "runas",  // Runs as Administrator
                UseShellExecute = true
            };

            try
            {
                Process.Start(processInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open CMD as administrator: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Reboot Bootloader Button
        private void RebootBootloader_Click(object sender, RoutedEventArgs e)
        {
            ExecuteCommandAsAdmin("adb reboot bootloader");
        }

        // Reboot Recovery Button
        private void RebootRecovery_Click(object sender, RoutedEventArgs e)
        {
            ExecuteCommandAsAdmin("adb reboot recovery");
        }

        // Install APK Button
        private void InstallAPK_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "APK Files (*.apk)|*.apk";
            if (openFileDialog.ShowDialog() == true)
            {
                string apkPath = openFileDialog.FileName;
                ExecuteCommandAsAdmin($"adb install \"{apkPath}\"");
            }
        }

        // Custom Command Button
        private void CustomCommand_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Command Files (*.cmd)|*.cmd";
            if (openFileDialog.ShowDialog() == true)
            {
                string cmdPath = openFileDialog.FileName;
                ExecuteCommandAsAdmin($"\"{cmdPath}\"");
            }
        }
    }
}
