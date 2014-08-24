namespace PhotoNamer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private string GetNewName(int photoIndex)
        {
            var formattedNumber = photoIndex.ToString("000");

            return string.Format(this.formatStringTextBox.Text, formattedNumber);
        }

        private void Log(string text)
        {
            this.logTextBox.Text += text + Environment.NewLine;
        }

        private void RenamePhotos(string rootPath, List<Photo> photos)
        {
            for (int i = 0; i < photos.Count; i++)
            {
                var oldFilename = photos[i].Path;

                this.Log(oldFilename);
                this.Log(string.Format("Taken: {0}", photos[i].DateTaken));

                var newName = GetNewName(i + 1);

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
        }

        private void GoButton_Click(object sender, EventArgs e)
        {
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
                RenamePhotos(rootPath, photos);

                stopwatch.Stop();
                this.Log(string.Format("Time elapsed: {0}", stopwatch.Elapsed));
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.pathTextBox.Text = this.folderBrowserDialog.SelectedPath;
            }
        }
    }
}