namespace PhotoNamer
{
    using System;
    using System.IO;
    using ExifLib;

    public class Photo
    {
        public Photo(string path)
        {
            this.Path = path;

            try
            {
                using (ExifReader reader = new ExifReader(path))
                {
                    // Extract the tag data using the ExifTags enumeration
                    DateTime datePictureTaken;
                    if (reader.GetTagValue<DateTime>(ExifTags.DateTimeDigitized, out datePictureTaken))
                    {
                        this.DateTaken = datePictureTaken;
                    }
                }
            }
            catch
            {
                // couldn't get the Exif data - fall back to Date Modified or Date Created whichever is earliest
                var fileCreatedDate = File.GetCreationTime(path);
                var fileModifiedDate = File.GetLastWriteTime(path);

                this.DateTaken = fileCreatedDate < fileModifiedDate ? fileCreatedDate : fileModifiedDate;
            }
        }

        public DateTime DateTaken
        {
            get;
            private set;
        }

        public string Path
        {
            get;
            private set;
        }
    }
}