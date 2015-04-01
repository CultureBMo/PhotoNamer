namespace PhotoNamer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    public class Video : MediaFile
    {
        public Video(string path)
        {
            this.OriginalPath = path;

            var location = Path.GetDirectoryName(path);
            var extension = Path.GetExtension(path);
            this.TemporaryPath = Path.Combine(location, Guid.NewGuid().ToString() + extension);

            this.DateTaken = GetDateCreated(path);

            if (this.DateTaken == DateTime.MinValue)
            {
                // couldn't get the Exif data - fall back to Date Modified or Date Created whichever is earliest
                var fileCreatedDate = File.GetCreationTime(path);
                var fileModifiedDate = File.GetLastWriteTime(path);

                this.DateTaken = fileCreatedDate < fileModifiedDate ? fileCreatedDate : fileModifiedDate;
            }
        }

        private static string CleanString(string stringToClean)
        {
            char[] charactersToRemove = new char[] { (char)8206, (char)8207 };

            // Removing the suspect characters
            foreach (char c in charactersToRemove)
            {
                stringToClean = stringToClean.Replace(c.ToString(), string.Empty).Trim();
            }

            return stringToClean;
        }

        private static DateTime GetDateCreated(string path)
        {
            var dateToReturn = new DateTime();

            Shell32.Shell shell = null;
            Shell32.Folder folder = null;
            Shell32.FolderItem media = null;

            try
            {
                shell = new Shell32.Shell();
                folder = shell.NameSpace(Path.GetDirectoryName(path));
                media = folder.ParseName(Path.GetFileName(path));

                var mediaCreatedIndex = GetMediaCreatedIndex(folder);

                if (mediaCreatedIndex > 0)
                {
                    var dateString = folder.GetDetailsOf(media, mediaCreatedIndex);

                    if (!string.IsNullOrWhiteSpace(dateString))
                    {
                        var cleanString = CleanString(dateString);

                        dateToReturn = Convert.ToDateTime(cleanString, new CultureInfo("en-GB"));
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (media != null)
                {
                    Marshal.ReleaseComObject(media);
                }

                if (folder != null)
                {
                    Marshal.ReleaseComObject(folder);
                }

                if (shell != null)
                {
                    Marshal.ReleaseComObject(shell);
                }
            }

            return dateToReturn;
        }

        private static int GetMediaCreatedIndex(Shell32.Folder folder)
        {
            // TODO: pass this into the class rather than lookin git up each time
            for (int i = 0; i < 300; i++)
            {
                var header = folder.GetDetailsOf(null, i);
                Console.WriteLine(header);

                if (header == "Media created")
                {
                    return i;
                }
            }

            return 0;
        }
    }
}