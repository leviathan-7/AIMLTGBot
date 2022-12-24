using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NeuralNetwork1
{
    public static class ImageEncoder
    {
        private const int TRESHOLD = 10;
        private static float DifferenceLimit => TRESHOLD / 255.0f;

        public static double[] Flatten(Bitmap original)
        {
            var blob = ExtractBlob(original);
            var vector = Vectorize(blob).ToArray();
            return vector;
        }

        public static Bitmap ExtractBlob(Bitmap original)
        {
            // Переводим в оттенки серого
            var grayFilter = new AForge.Imaging.Filters.Grayscale(0.2125, 0.7154, 0.0721);
            var unmanaged = grayFilter.Apply(AForge.Imaging.UnmanagedImage.FromManagedImage(original));

            // Инвертируем
            var invertFilter = new AForge.Imaging.Filters.Invert();
            invertFilter.ApplyInPlace(unmanaged);

            // Применяем пороговый фильтр
            var threshldFilter = new AForge.Imaging.Filters.BradleyLocalThresholding();
            threshldFilter.PixelBrightnessDifferenceLimit = DifferenceLimit;
            threshldFilter.ApplyInPlace(unmanaged);

            // Создаём BlobCounter, вытаскиваем блобы
            var bc = new AForge.Imaging.BlobCounter();

            bc.FilterBlobs = true;
            bc.MinWidth = 10;
            bc.MinHeight = 10;
            // Упорядочиваем по размеру
            bc.ObjectsOrder = AForge.Imaging.ObjectsOrder.Size;

            bc.ProcessImage(unmanaged);

            var rectangles = bc.GetObjectsRectangles().Skip(1).ToArray();

            int lx = unmanaged.Width;
            int ly = unmanaged.Height;
            int rx = 0;
            int ry = 0;
            for (int i = 0; i < rectangles.Length; ++i)
            {
                if (lx > rectangles[i].X)
                {
                    lx = rectangles[i].X;
                }
                if (ly > rectangles[i].Y)
                {
                    ly = rectangles[i].Y;
                }
                if (rx < rectangles[i].X + rectangles[i].Width)
                {
                    rx = rectangles[i].X + rectangles[i].Width;
                }
                if (ry < rectangles[i].Y + rectangles[i].Height)
                {
                    ry = rectangles[i].Y + rectangles[i].Height;
                }
            }

            var cropFilter = new AForge.Imaging.Filters.Crop(new Rectangle(lx, ly, rx - lx, ry - ly));
            unmanaged = cropFilter.Apply(unmanaged);

            // Масштабируем до 20x20
            var scaleFilter = new AForge.Imaging.Filters.ResizeBilinear(20, 20);
            unmanaged = scaleFilter.Apply(unmanaged);

            return unmanaged.ToManagedImage();
        }

        private static double[] Vectorize(Bitmap blob)
        {
            var vector = Enumerable
                .Repeat(0.0, blob.Width * blob.Height)
                .ToArray();
            for (int x = 0; x < blob.Width; x += 1)
            {
                for (int y = 0; y < blob.Height; y += 1)
                {
                    var color = blob.GetPixel(x, y);
                    if (SatisfiesTreshold(color))
                    {
                        vector[x + y] = 1.0;
                    }
                }
            }
            return vector;
        }

        private static bool SatisfiesTreshold(Color color)
            => color.R <= TRESHOLD && color.G <= TRESHOLD && color.B <= TRESHOLD;
    }
}
