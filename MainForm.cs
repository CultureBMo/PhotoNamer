namespace PhotoNamer
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    public partial class MainForm : Form
    {
        public MainForm()
        {
            this.InitializeComponent();
        }

        private static void SaveToConfig(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;

                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }

                configFile.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch
            {
                MessageBox.Show("There was an error saving your options"
                    + Environment.NewLine
                    + "Please ensure your user account has Modify permissons to the PhotoNamer.config file");
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.pathTextBox.Text = this.folderBrowserDialog.SelectedPath;
            }
        }

        private string GetNewName(int fileIndex)
        {
            var formattedNumber = fileIndex.ToString("000");

            return string.Format(this.formatStringTextBox.Text, formattedNumber);
        }

        private void GoButton_Click(object sender, EventArgs e)
        {
            this.logTextBox.Text = string.Empty;

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            var rootPath = this.pathTextBox.Text;

            if (Directory.Exists(rootPath))
            {
                IEnumerable<MediaFile> mediaFiles;

                if (this.photosButton.Checked)
                {
                    mediaFiles = Directory
                                    .EnumerateFiles(rootPath, "*.jpg")
                                    .Select(p => new Photo(p))
                                    .OrderBy(x => x.DateTaken);
                }
                else
                {
                    int mediaIndex = Video.GetMediaCreatedIndex(rootPath);

                    mediaFiles = Directory
                                    .EnumerateFiles(rootPath, "*.*")
                                    .Where(s =>
                                        s.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase) ||
                                        s.EndsWith(".mov", StringComparison.OrdinalIgnoreCase) ||
                                        s.EndsWith(".avi", StringComparison.OrdinalIgnoreCase))
                                    .Select(v => new Video(v, mediaIndex))
                                    .OrderBy(x => x.DateTaken);
                }

                this.RenameFiles(rootPath, mediaFiles);

                this.SaveSettings();
            }

            stopwatch.Stop();

            this.Log(string.Format("Time elapsed: {0}", stopwatch.Elapsed));
            this.Log("Copyright © CultureBMo 2015");
            this.Log("Icon: Oxygen Team, GPL");
        }

        private void Log(string text)
        {
            this.logTextBox.Text += text + Environment.NewLine;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var pathSetting = ConfigurationManager.AppSettings["Path"] ?? @"D:\Users\Ben\OneDrive\Pictures\2015";

            if (!Directory.Exists(pathSetting))
            {
                pathSetting = @"C:\Temp";
            }

            this.pathTextBox.Text = pathSetting;
            this.folderBrowserDialog.SelectedPath = pathSetting;

            this.formatStringTextBox.Text = ConfigurationManager.AppSettings["FormatString"] ?? "100 {0}";

            this.photosButton.Checked = Convert.ToBoolean(ConfigurationManager.AppSettings["ForPhotosNotVideos"]);
        }

        private void RenameFiles(string rootPath, IEnumerable<MediaFile> mediaFiles)
        {
            var listOfFiles = mediaFiles.ToList();

            foreach (var mediaFile in listOfFiles)
            {
                if (mediaFile.RequiresTemporaryFile)
                {
                    File.Copy(mediaFile.OriginalPath, mediaFile.TemporaryPath, true);
                    File.Delete(mediaFile.OriginalPath);
                }

                Application.DoEvents();
            }

            for (int i = 0; i < listOfFiles.Count; i++)
            {
                var mediaFile = listOfFiles[i];

                this.Log(mediaFile.OriginalPath);
                this.Log(string.Format("Taken: {0}", mediaFile.DateTaken));

                mediaFile.NewPath = Path.Combine(rootPath, this.GetNewName(i + 1) + Path.GetExtension(mediaFile.OriginalPath).ToLower());

                this.Log(string.Format("Now called: {0}", mediaFile.NewPath));
                this.Log("----------------------------");

                if (mediaFile.RequiresTemporaryFile)
                {
                    File.Copy(mediaFile.TemporaryPath, mediaFile.NewPath, true);
                    File.Delete(mediaFile.TemporaryPath);
                }

                Application.DoEvents();
            }
        }

        private void SaveSettings()
        {
            var key = "Path";
            var value = this.pathTextBox.Text;
            SaveToConfig(key, value);

            key = "FormatString";
            value = this.formatStringTextBox.Text;
            SaveToConfig(key, value);

            key = "ForPhotosNotVideos";
            value = this.photosButton.Checked.ToString();
            SaveToConfig(key, value);
        }
    }
}