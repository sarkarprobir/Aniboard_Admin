using System.Xml.Linq;
using System.Drawing; // for Icon (need System.Drawing.Common on non-Windows)
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace AniBoard_Admin.Utility
{
    public static class ImageHelper
    {
        public static (int width, int height) GetImageSize(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLower();

            if (ext == ".ico")
            {
                return GetIcoSize(filePath);
            }
            else if (ext == ".svg")
            {
                return GetSvgSize(filePath);
            }
            else
            {
                return GetRasterSize(filePath);
            }
        }

        private static (int width, int height) GetRasterSize(string filePath)
        {
            using (var image = SixLabors.ImageSharp.Image.Load<Rgba32>(filePath))
            {
                return (image.Width, image.Height);
            }
        }

        private static (int width, int height) GetIcoSize(string filePath)
        {
            using (var icon = new Icon(filePath))
            {
                return (icon.Width, icon.Height);
            }
        }

        private static (int width, int height) GetSvgSize(string filePath)
        {
            var doc = XDocument.Load(filePath);
            var svg = doc.Root;
            if (svg == null) return (0, 0);

            int width = 0, height = 0;

            // Try width & height attributes
            var widthAttr = svg.Attribute("width")?.Value;
            var heightAttr = svg.Attribute("height")?.Value;

            if (!string.IsNullOrEmpty(widthAttr))
                int.TryParse(new string(widthAttr.Where(char.IsDigit).ToArray()), out width);

            if (!string.IsNullOrEmpty(heightAttr))
                int.TryParse(new string(heightAttr.Where(char.IsDigit).ToArray()), out height);

            // If missing, fallback to viewBox
            var viewBox = svg.Attribute("viewBox")?.Value;
            if ((width == 0 || height == 0) && !string.IsNullOrEmpty(viewBox))
            {
                var parts = viewBox.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 4)
                {
                    int.TryParse(parts[2], out width);
                    int.TryParse(parts[3], out height);
                }
            }

            return (width, height);
        }
    }
}
