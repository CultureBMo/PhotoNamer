namespace PhotoNamer
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;

    public class Video : MediaFile
    {
        public Video(string path, int mediaCreatedIndex)
        {
            this.OriginalPath = path;

            var location = Path.GetDirectoryName(path);
            var extension = Path.GetExtension(path);

            this.TemporaryPath = Path.Combine(location, Guid.NewGuid().ToString() + extension);

            this.DateTaken = GetDateTaken(path, mediaCreatedIndex);

            if (this.DateTaken == DateTime.MinValue)
            {
                // couldn't get the Media created value - fall back to Date Modified or Date Created whichever is earliest
                var fileCreatedDate = File.GetCreationTime(path);
                var fileModifiedDate = File.GetLastWriteTime(path);

                this.DateTaken = fileCreatedDate < fileModifiedDate ? fileCreatedDate : fileModifiedDate;
            }
        }

        public static int GetMediaCreatedIndex(string rootPath)
        {
            Shell32.Shell shell = null;
            Shell32.Folder folder = null;

            try
            {
                shell = new Shell32.Shell();
                folder = shell.NameSpace(Path.GetDirectoryName(rootPath));

                for (int i = 0; i < 300; i++)
                {
                    var header = folder.GetDetailsOf(null, i);
                    Console.WriteLine(header);

                    if (header == "Media created")
                    {
                        return i;
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (folder != null)
                {
                    Marshal.ReleaseComObject(folder);
                }

                if (shell != null)
                {
                    Marshal.ReleaseComObject(shell);
                }
            }

            return 0;
        }

        private static string CleanString(string stringToClean)
        {
            char[] charactersToRemove = new char[] { (char)8206, (char)8207 };

            foreach (char c in charactersToRemove)
            {
                stringToClean = stringToClean.Replace(c.ToString(), string.Empty).Trim();
            }

            return stringToClean;
        }

        private static DateTime GetDateTaken(string path, int mediaCreatedIndex)
        {
            var dateToReturn = default(DateTime);

            Shell32.Shell shell = null;
            Shell32.Folder folder = null;
            Shell32.FolderItem media = null;

            try
            {
                shell = new Shell32.Shell();
                folder = shell.NameSpace(Path.GetDirectoryName(path));
                media = folder.ParseName(Path.GetFileName(path));

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
    }
}