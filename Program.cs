using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace PictureUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            // get the path from config
            var rootPath = ConfigurationManager.AppSettings["Path"];

            // sort the photos by DateTaken
            var photos = Directory.GetFiles(rootPath, "*.jpg")
                            .Select(p => new Photo(p))
                            .OrderBy(x => x.DateTaken).ToList();

            // Rename them
            RenamePhotos(rootPath, photos);

            stopwatch.Stop();
            System.Console.WriteLine();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void RenamePhotos(string rootPath, List<Photo> photos)
        {
            for (int i = 0; i < photos.Count; i++)
            {
                var oldFilename = photos[i].Path;

                Console.WriteLine(oldFilename);
                Console.WriteLine("Taken: {0}", photos[i].DateTaken);

                var newName = GetNewName(i + 1);

                Console.WriteLine("Now called: {0}", newName);

                var newFilename = Path.Combine(rootPath, newName);

                // rename if necessary
                if (string.Compare(oldFilename, newFilename, StringComparison.InvariantCultureIgnoreCase) != 0)
                {
                    File.Copy(oldFilename, newFilename, true);
                }

                var deleteOriginals = bool.Parse(ConfigurationManager.AppSettings["DeleteOriginals"]);

                if (deleteOriginals)
                {
                    Console.WriteLine("Deleting original...");
                    File.Delete(oldFilename);
                }

                Console.WriteLine("----------------------------");
            }
        }

        private static string GetNewName(int photoIndex)
        {
            var formattedNumber = photoIndex.ToString("000");

            return string.Format("100 {0}.jpg", formattedNumber);
        }
    }
}
