using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;

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


        public PhotoCollection()
        {
            string folderPath = HostingEnvironment.MapPath("~/Content/");
            string[] photos = Directory.GetFiles(Path.Combine(folderPath, "photos")).Select(path => Path.GetFileName(path)).ToArray();
            List<Photo> photoCollection = new List<Photo>();

            var encodedTitle = new byte[] { };
            var encodedCaption = new byte[] { };

            foreach (var photo in photos)
            {
                Image image = Image.FromFile(folderPath + "photos\\" + photo);
                PropertyItem title = image.PropertyItems.Where(p => p.Id == 40091).FirstOrDefault();
                PropertyItem caption = image.PropertyItems.Where(p => p.Id == 40092).FirstOrDefault();

                if (title != null) encodedTitle = title.Value;
                else encodedTitle = new byte[] { };
                if (caption != null) encodedCaption = caption.Value;
                else encodedCaption = new byte[] { };
                photoCollection.Add(new Photo() { Title = DecodeExif(encodedTitle), Caption = DecodeExif(encodedCaption), FilePath = "/Content/photos/" + photo });
                image.Dispose();
            }

            ReloadInterval = (photoCollection.Count() * 13000);
            Photos = photoCollection;
        }

        private string DecodeExif(byte[] encodedData)
        {
            string decodedData = Encoding.Default.GetString(encodedData);
            decodedData = decodedData.Replace("\0", String.Empty);
            return decodedData;
        }
    }
}