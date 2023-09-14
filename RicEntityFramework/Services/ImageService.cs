using System;
using System.Drawing;
using System.IO;
using RicCommon.Constants;
using RicEntityFramework.Interfaces;

namespace RicEntityFramework.Services
{
    public class ImageService : IImageService
    {
        public string GetImageInBase64(int renterId)
        {
            var folderName = Path.Combine("Resources", "Images", "Profile");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fileName = $"{renterId}.png";
            var fullPath = Path.Combine(pathToSave, fileName);

            if (!File.Exists(fullPath))
                fullPath = Path.Combine(pathToSave, "default-user-image.png");

            using (Image image = Image.FromFile(fullPath))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    var base64String = Convert.ToBase64String(imageBytes);
                    return $"data:image/png;base64,{base64String}";
                }
            }
        }

        public void Upload(int renterId, string base64)
        {
            try
            {
                var folderName = Path.Combine("Resources", "Images", "Profile");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                var fileName = $"{renterId}.png";
                var imageBytes = Convert.FromBase64String(base64);
                var fullPath = Path.Combine(pathToSave, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                using (var ms = new MemoryStream(imageBytes))
                {
                    ms.CopyTo(stream);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public string Upload(string base64, string location)
        {
            string filename = Guid.NewGuid().ToString();
            try
            { 
                var folderName = Path.Combine("Resources", "Images", location);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                filename = $"{filename}-inventory.png";
                var imageBytes = Convert.FromBase64String(base64);
                var fullPath = Path.Combine(pathToSave, filename);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                using (var ms = new MemoryStream(imageBytes))
                {
                    ms.CopyTo(stream);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return filename;
        }
    }
}
