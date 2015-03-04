using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Web;

namespace ExoBillboard.Models
{
    public class Photo
    {
        public string FilePath { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        public int Index { get; set; }
        public Photo()
        {
        }
    }

    public class PhotoCollection
    {
        public List<Photo> Photos { get; set; }
        public int ReloadInterval { get; set; }
        private string[] ValidExtensions = { ".png", ".jpg" };
        private const string PhotoRoot = "/Content/photos/";

        public PhotoCollection()
        {
            string folderPath = HostingEnvironment.MapPath(PhotoRoot);
            string[] photos = Directory.EnumerateFiles(folderPath).ToArray();
            List<Photo> photoCollection = new List<Photo>();
            var encodedTitle = new byte[] { };
            var encodedCaption = new byte[] { };

            foreach (var photo in photos)
            {
                if (IsValidExtension(photo))
                {
                    Image image = Image.FromFile(photo);
                    PropertyItem title = image.PropertyItems.Where(p => p.Id == 40091).FirstOrDefault();
                    PropertyItem caption = image.PropertyItems.Where(p => p.Id == 40092).FirstOrDefault();

                    if (title != null) encodedTitle = title.Value;
                    else encodedTitle = new byte[] { };
                    if (caption != null) encodedCaption = caption.Value;
                    else encodedCaption = new byte[] { };

                    photoCollection.Add(new Photo()
                    {
                        Title = DecodeExif(encodedTitle),
                        Caption = DecodeExif(encodedCaption),
                        FilePath = PhotoRoot + Path.GetFileName(photo)
                    });

                    image.Dispose();
                }
            }

            ReloadInterval = (photoCollection.Count() * 13000);
            Photos = photoCollection;
        }

        private bool IsValidExtension(string filePath)
        {
            if (ValidExtensions.Contains(Path.GetExtension(filePath))) return true;
            return false;
        }

        private string DecodeExif(byte[] encodedData)
        {
            string decodedData = Encoding.Default.GetString(encodedData);
            decodedData = decodedData.Replace("\0", String.Empty);
            return decodedData;
        }
    }
}