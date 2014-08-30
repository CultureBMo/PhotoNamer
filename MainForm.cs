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
                MessageBox.Show("There was an error saving your options" + Environment.NewLine + "Please ensure your user account has Modify permissons to the PhotoNamer.config file");
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.pathTextBox.Text = this.folderBrowserDialog.SelectedPath;
            }
        }

        private string GetNewName(int photoIndex)
        {
            var formattedNumber = photoIndex.ToString("000");

            return string.Format(this.formatStringTextBox.Text, formattedNumber);
        }

        private void GoButton_Click(object sender, EventArgs e)
        {
            this.logTextBox.Text = string.Empty;

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            // get the path from config
            var rootPath = this.pathTextBox.Text;

            if (Directory.Exists(rootPath))
            {
                // Rename them
                this.CreateTemporaryPhotos(rootPath);

                this.RenameTemporaryPhotos(rootPath);

                this.SaveSettings();
            }

            stopwatch.Stop();
            this.Log(string.Format("Time elapsed: {0}", stopwatch.Elapsed));
            this.Log("Copyright © CultureBMo 2014");
        }

        private void Log(string text)
        {
            this.logTextBox.Text += text + Environment.NewLine;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var pathSetting = ConfigurationManager.AppSettings["Path"] ?? @"D:\Users\Ben\OneDrive\Pictures\2014";

            if (!Directory.Exists(pathSetting))
            {
                pathSetting = @"C:\Temp\Puppy";
            }

            this.pathTextBox.Text = pathSetting;
            this.folderBrowserDialog.SelectedPath = pathSetting;

            this.formatStringTextBox.Text = ConfigurationManager.AppSettings["FormatString"] ?? "100 {0}.jpg";
        }

        private void CreateTemporaryPhotos(string rootPath)
        {
            var photos = Directory.GetFiles(rootPath, "*.jpg")
                    .Select(p => new Photo(p))
                    .OrderBy(x => x.DateTaken).ToList();

            for (int i = 0; i < photos.Count; i++)
            {
                var oldFilename = photos[i].Path;

                this.Log(oldFilename);
                this.Log(string.Format("Taken: {0}", photos[i].DateTaken));

                var tempFilename = Path.Combine(rootPath, Guid.NewGuid().ToString() + ".jpg");

                File.Copy(oldFilename, tempFilename, true);
                File.Delete(oldFilename);

                this.Log(string.Format("Now called: {0}", this.GetNewName(i + 1)));
                this.Log("----------------------------");
            }
        }

        private void RenameTemporaryPhotos(string rootPath)
        {
            var photos = Directory.GetFiles(rootPath, "*.jpg")
                    .Select(p => new Photo(p))
                    .OrderBy(x => x.DateTaken).ToList();

            for (int i = 0; i < photos.Count; i++)
            {
                var tempFilename = photos[i].Path;

                var newName = this.GetNewName(i + 1);

                var newFilename = Path.Combine(rootPath, newName);

                File.Copy(tempFilename, newFilename, true);
                File.Delete(tempFilename);
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
        }
    }
}