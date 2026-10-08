using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PaintBox3000.Core
{
    /// <summary>
    /// Saves and Loads image-files.
    /// </summary>
    internal static class ImageFileService
    {
        public const string SaveFilter = "PNG-Datei|*.png|JPEG-Datei|*.jpg|Bitmap-Datei|*.bmp";
        public const string OpenFilter = "Imagefiles|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tif;*.tiff";

        public static BitmapImage LoadImage(string path)
        {
            Uri uri = new(path); // Pfad vorhanden?

            BitmapImage bmp = new(); // Bildformat gültig?
            bmp.BeginInit();
            bmp.UriSource = uri;
            bmp.CacheOption = BitmapCacheOption.OnLoad; // lädt und decodiert präventiv
            bmp.EndInit();

            return bmp;
        }

        public static void SaveCanvas(Canvas canvas, string path)
        {
            int width = (int)canvas.ActualWidth;
            int height = (int)canvas.ActualHeight;

            RenderTargetBitmap renderBitmap = new(width, height, 96, 96, PixelFormats.Pbgra32);
            renderBitmap.Render(canvas);

            BitmapEncoder encoder = Path.GetExtension(path).ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => new JpegBitmapEncoder(),
                ".bmp" => new BmpBitmapEncoder(),
                ".png" => new PngBitmapEncoder(),
                _ => throw new NotSupportedException("Das Format wird nicht unterstützt."),
            };
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            using FileStream stream = new(path, FileMode.Create);
            encoder.Save(stream);
        }
    }
}