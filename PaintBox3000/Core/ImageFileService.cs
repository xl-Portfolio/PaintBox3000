using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace PaintBox3000.Core
{
    /// <summary>
    /// Saves and Loads image-files
    /// </summary>
    internal class ImageFileService
    {  
        /// <summary>
        /// String(path) to Uri to BitmapImage
        /// </summary>
        /// <param name="path"></param>
        /// <returns>BitmapImage</returns>
        public BitmapImage LoadImage(string path)
        {
            Uri uri = new(path); // Pfad vorhanden?

            BitmapImage bmp = new(); //Bildformat gültig?
            bmp.BeginInit();
            bmp.UriSource = uri;
            bmp.CacheOption = BitmapCacheOption.OnLoad; //lädt und decodiert präventiv
            bmp.EndInit();

            return bmp;
        }
        /// <summary>
        /// Canvas to BitmapFrames to Filestream to saved File
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="path"></param>
        public void SaveCanvas(Canvas canvas, string path)
        {
            int width = (int)canvas.ActualWidth;
            int height = (int)canvas.ActualHeight;

            RenderTargetBitmap renderBitmap = new(width, height, 96, 96, PixelFormats.Pbgra32);
            renderBitmap.Render(canvas);

            BitmapEncoder encoder = Path.GetExtension(path).ToLower() switch
            {
                ".jpg" or ".jpeg" => new JpegBitmapEncoder(),
                ".bmp" => new BmpBitmapEncoder(),
                _ => new PngBitmapEncoder()
            };
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            using FileStream stream = new(path, FileMode.Create);
            encoder.Save(stream);
        }
    }
}
