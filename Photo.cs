using System;
using ExifLib;

namespace PictureUtil
{
    public class Photo
    {
        public Photo(string path)
        {
            this.Path = path;

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
