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
                // sort the photos by DateTaken
                var photos = Directory.GetFiles(rootPath, "*.jpg")
                                .Select(p => new Photo(p))
                                .OrderBy(x => x.DateTaken).ToList();

                // Rename them
                this.RenamePhotos(rootPath, photos);

                stopwatch.Stop();
                this.Log(string.Format("Time elapsed: {0}", stopwatch.Elapsed));
                this.Log("Copyright © CultureBMo 2014");
            }
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
                pathSetting = @"C:\Temp";
            }

            this.pathTextBox.Text = pathSetting;
            this.folderBrowserDialog.SelectedPath = pathSetting;

            var deleteOriginalsSetting = ConfigurationManager.AppSettings["DeleteOriginals"] ?? "False";

            var deleteOriginals = false;

            bool.TryParse(deleteOriginalsSetting, out deleteOriginals);

            deleteOriginalsYes.Checked = deleteOriginals;

            this.formatStringTextBox.Text = ConfigurationManager.AppSettings["FormatString"] ?? "100 {0}.jpg";
        }

        private void RenamePhotos(string rootPath, List<Photo> photos)
        {
            for (int i = 0; i < photos.Count; i++)
            {
                var oldFilename = photos[i].Path;

                this.Log(oldFilename);
                this.Log(string.Format("Taken: {0}", photos[i].DateTaken));

                var newName = this.GetNewName(i + 1);

                this.Log(string.Format("Now called: {0}", newName));

                var newFilename = Path.Combine(rootPath, newName);

                // rename if necessary
                if (string.Compare(oldFilename, newFilename, StringComparison.InvariantCultureIgnoreCase) != 0)
                {
                    File.Copy(oldFilename, newFilename, true);
                }

                if (this.deleteOriginalsYes.Checked)
                {
                    this.Log("Deleting original...");
                    File.Delete(oldFilename);
                }

                this.Log("----------------------------");
            }

            this.SaveSettings();
        }

        private void SaveSettings()
        {
            var key = "Path";
            var value = this.pathTextBox.Text;

            SaveToConfig(key, value);

            key = "DeleteOriginals";
            value = this.deleteOriginalsYes.Checked.ToString();

            SaveToConfig(key, value);

            key = "FormatString";
            value = this.formatStringTextBox.Text;

            SaveToConfig(key, value);
        }
    }
}