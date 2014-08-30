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

        private void CreateTemporaryPhotos(string rootPath, List<Photo> photos)
        {
            for (int i = 0; i < photos.Count; i++)
            {
                var photo = photos[i];

                this.Log(photo.OriginalPath);
                this.Log(string.Format("Taken: {0}", photo.DateTaken));

                photo.NewPath = Path.Combine(rootPath, this.GetNewName(i + 1));

                this.Log(string.Format("Now called: {0}", photo.NewPath));
                this.Log("----------------------------");

                if (photos[i].RequiresTemporaryFile)
                {
                    var tempFilename = photo.TemporaryPath;

                    File.Copy(photo.OriginalPath, tempFilename, true);
                    File.Delete(photo.OriginalPath);
                }
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

            var rootPath = this.pathTextBox.Text;

            if (Directory.Exists(rootPath))
            {
                var photos = Directory.GetFiles(rootPath, "*.jpg")
                        .Select(p => new Photo(p))
                        .OrderBy(x => x.DateTaken).ToList();

                // create temporary copies...
                this.CreateTemporaryPhotos(rootPath, photos);

                // so that we can successfully rename them
                this.RenameTemporaryPhotos(rootPath, photos);

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

        private void RenameTemporaryPhotos(string rootPath, List<Photo> photos)
        {
            foreach (var photo in photos)
            {
                if (photo.RequiresTemporaryFile)
                {
                    File.Copy(photo.TemporaryPath, photo.NewPath, true);
                    File.Delete(photo.TemporaryPath);
                }
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