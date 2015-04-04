namespace PhotoNamer
{
    using System;

    public class MediaFile
    {
        public DateTime DateTaken
        {
            get;
            protected internal set;
        }

        public string NewPath
        {
            get;
            set;
        }

        public string OriginalPath
        {
            get;
            protected internal set;
        }

        public bool RequiresTemporaryFile
        {
            get
            {
                return string.Compare(this.OriginalPath, this.NewPath, StringComparison.OrdinalIgnoreCase) != 0;
            }
        }

        public string TemporaryPath
        {
            get;
            protected internal set;
        }
    }
}