using System.Drawing;

namespace APLTechChallenge.Helpers
{
    public static class ImageValidator
    {
        public static void CheckExtension(IFormFile file)
        {
            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
            var fileExtension = Path.GetExtension(file.FileName);

            if (!Array.Exists(allowedExtensions, ext => ext == fileExtension))
            {
                throw new Exception("Invalid file format. Only PNG and JPG files are allowed.");
            }
        }

        public static void CheckImageDimensions(IFormFile file)
        {
            using (var image = Image.FromStream(file.OpenReadStream()))
            {
                if (image.Width > 1024 || image.Height > 1024)
                {
                    throw new Exception("Image dimensions should be no more than 1024x1024 pixels.");
                }
            }
        }
    }

}
