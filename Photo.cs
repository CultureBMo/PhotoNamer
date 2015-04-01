namespace PhotoNamer
{
    using System;
    using System.IO;
    using ExifLib;

    public class Photo : MediaFile
    {
        public Photo(string path)
        {
            this.OriginalPath = path;

            var location = Path.GetDirectoryName(path);
            this.TemporaryPath = Path.Combine(location, Guid.NewGuid().ToString() + ".jpg");

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
    }
}