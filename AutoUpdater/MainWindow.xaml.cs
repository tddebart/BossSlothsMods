using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;
//using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ionic.Zip;
using Octokit;

namespace AutoUpdater
{
    enum LauncherStatus
    {
        ready,
        failed,
        downloadingGame,
        downloadingUpdate
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly string rootPath;
        private readonly string versionFile;
        private readonly string gameZip;
        private bool _official;

        private LauncherStatus _status;

        private LauncherStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                switch (_status)
                {
                    case LauncherStatus.ready:
                        PlayButton.Content = "Update Downloaded, click to close";
                        break;
                    case LauncherStatus.failed:
                        PlayButton.Content = "Update Failed - Retry";
                        break;
                    case LauncherStatus.downloadingGame:
                        PlayButton.Content = "Downloading Mod files";
                        break;
                    case LauncherStatus.downloadingUpdate:
                        PlayButton.Content = "Downloading Update";
                        break;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            rootPath = Directory.GetCurrentDirectory();
            versionFile = Path.Combine(rootPath, "libs", "Version.txt");
            gameZip = Path.Combine(rootPath, "ModFiles.zip");
        }

        private async Task CheckForUpdates()
        {
            if (File.Exists(versionFile))
            {
                Version localVersion = new Version(File.ReadAllText(versionFile));
                VersionText.Text = "Version: " + localVersion.ToString();

                try
                {
                    //WebClient webClient = new WebClient();
                    //var onlineVersion = new Version(webClient.DownloadString("https://github.com/tddebart/ActualRoundsMod/releases/latest/download/Version.txt"));
                    var client = new GitHubClient(new ProductHeaderValue("SomeName"));
                    var releases = await client.Repository.Release.GetAll("tddebart", "ActualRoundsMod");
                    var verFix = Regex.Replace(releases[0].TagName, "[^0-9.]", "");
                    var onlineVersion = new Version(verFix);
                    
                    
                    
                    if (onlineVersion.IsDifferentThan(localVersion))
                    {
                        InstallGameFiles(true, onlineVersion);
                    }
                    else
                    {
                        Status = LauncherStatus.ready;
                        /*
                        await Task.Delay(1000);
                        PlayButton.Content = "Update Downloaded, will close in 1 seconds";
                        await Task.Delay(1000);
                        PlayButton.Content = "Update Downloaded, will close in 0 seconds";
                        Close(); */
                    }
                }
                catch (Exception ex)
                {
                    Status = LauncherStatus.failed;
                    MessageBox.Show($"Error checking for game updates: {ex}");
                }
            }
            else
            {
                InstallGameFiles(false, Version.zero);
            }
        }

        private async void InstallGameFiles(bool _isUpdate, Version _onlineVersion)
        {
            try
            {
                WebClient webClient = new WebClient();
                if (_isUpdate)
                {
                    Status = LauncherStatus.downloadingUpdate;
                }
                else
                {
                    Status = LauncherStatus.downloadingGame;
                    //_onlineVersion = new Version(webClient.DownloadString("https://github.com/tddebart/ActualRoundsMod/releases/latest/download/Version.txt"));
                    var client = new GitHubClient(new ProductHeaderValue("SomeName"));
                    var releases = await client.Repository.Release.GetAll("tddebart", "ActualRoundsMod");
                    var verFix = Regex.Replace(releases[0].TagName, "[^0-9.]", "");
                    _onlineVersion = new Version(verFix);
                }

                webClient.DownloadFileCompleted += DownloadGameCompletedCallback;
                webClient.DownloadFileAsync(new Uri("https://github.com/tddebart/ActualRoundsMod/releases/latest/download/ActualRoundsMod.zip"), gameZip, _onlineVersion);
            }
            catch (Exception ex)
            {
                Status = LauncherStatus.failed;
                MessageBox.Show($"Error installing game files: {ex}");
            }
        }
        private void DownloadGameCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                
                var onlineVersion = ((Version)e.UserState).ToString();
                
                //ZipFile.ExtractToDirectory(gameZip, rootPath);
                var zip = ZipFile.Read(gameZip);
                foreach (var ene in zip)
                {
                    ene.Extract(rootPath, ExtractExistingFileAction.OverwriteSilently);
                }
                zip.Dispose();
                File.Delete(gameZip);
                File.WriteAllText(versionFile, onlineVersion);

                VersionText.Text = "Version: " + onlineVersion;
                Status = LauncherStatus.ready;
                /*
                await Task.Delay(1000);
                PlayButton.Content = "Update Downloaded, will close in 1 seconds";
                await Task.Delay(1000);
                PlayButton.Content = "Update Downloaded, will close in 0 seconds";
                Close(); */
            }
            catch (Exception ex)
            {
                Status = LauncherStatus.failed;
                MessageBox.Show($"Error finishing download: {ex}");
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            // ReSharper disable once UnusedVariable
            var task = CheckForUpdates();

            _official = (string)Properties.Settings.Default["DVersion"] == "Official";
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (Status == LauncherStatus.ready) Close();
            else if (Status == LauncherStatus.failed)
            {
                // ReSharper disable once UnusedVariable
                var task = CheckForUpdates();
            }
        }
        
        private void Official(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default["DVersion"] = "Official";
            Properties.Settings.Default.Save();
        }
        
        private void Beta(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default["DVersion"] = "Beta";
            Properties.Settings.Default.Save();
        }
    }

    internal readonly struct Version
    {
        internal static Version zero = new Version(0, 0, 0);

        private readonly short major;
        private readonly short minor;
        private readonly short subMinor;

        private Version(short _major, short _minor, short _subMinor)
        {
            major = _major;
            minor = _minor;
            subMinor = _subMinor;
        }
        
        internal Version(string _version)
        {
            var versionStrings = _version.Split('.');
            if (versionStrings.Length != 3)
            {
                major = 0;
                minor = 0;
                subMinor = 0;
                return;
            }

            major = short.Parse(versionStrings[0]);
            minor = short.Parse(versionStrings[1]);
            subMinor = short.Parse(versionStrings[2]);
        }

        internal bool IsDifferentThan(Version _otherVersion)
        {
            if (major != _otherVersion.major)
            {
                return true;
            }
            else
            {
                if (minor != _otherVersion.minor)
                {
                    return true;
                }
                else
                {
                    if (subMinor != _otherVersion.subMinor)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override string ToString()
        {
            return $"{major}.{minor}.{subMinor}";
        }


    }
}
