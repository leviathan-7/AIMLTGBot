using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AIMLTGBot
{
    public static class BitmapEncoder
    {
        private const float DIFFERENCE_LIM = 0.131f;
        private const int RGB_COMPONENT_TRESHOLD = 25;

        public static double[] Flatten(Bitmap original)
        {
            var blobs = CaptureBlobs(original);

            int vectorSize = blobs[0].Width + blobs[0].Height
                + blobs[1].Width + blobs[1].Height
                + blobs[2].Width + blobs[2].Height;

            var vector = Enumerable
                .Repeat(0.0, vectorSize)
                .ToArray();

            for (int x = 0; x < blobs[0].Width; x += 1)
            {
                for (int y = 0; y < blobs[0].Height; y += 1)
                {
                    var color = blobs[0].GetPixel(x, y);
                    if (SatisfiesRGBComponentTreshold(color))
                    {
                        vector[x] += 1.0;
                        vector[x + blobs[0].Height] += 1.0;
                    }
                }
            }

            for (int x = 0; x < blobs[1].Width; x += 1)
            {
                for (int y = 0; y < blobs[1].Height; y += 1)
                {
                    var color = blobs[1].GetPixel(x, y);
                    if (SatisfiesRGBComponentTreshold(color))
                    {
                        vector[x + blobs[0].Width] += 1.0;
                        vector[x + blobs[0].Height + blobs[1].Height] += 1.0;
                    }
                }
            }

            for (int x = 0; x < blobs[2].Width; x += 1)
            {
                for (int y = 0; y < blobs[2].Height; y += 1)
                {
                    var color = blobs[2].GetPixel(x, y);
                    if (SatisfiesRGBComponentTreshold(color))
                    {
                        vector[x + blobs[0].Width + blobs[1].Width] += 1.0;
                        vector[x + blobs[0].Height + blobs[1].Height + blobs[2].Height] += 1.0;
                    }
                }
            }

            return vector;
        }

        public static Bitmap[] CaptureBlobs(Bitmap original)
        {
            // Переводим в оттенки серого
            var grayFilter = new AForge.Imaging.Filters.Grayscale(0.2125, 0.7154, 0.0721);
            var unmanaged = grayFilter.Apply(AForge.Imaging.UnmanagedImage.FromManagedImage(original));

            // Инвертируем
            var invertFilter = new AForge.Imaging.Filters.Invert();
            invertFilter.ApplyInPlace(unmanaged);

            // Применяем пороговый фильтр
            var threshldFilter = new AForge.Imaging.Filters.BradleyLocalThresholding();
            threshldFilter.PixelBrightnessDifferenceLimit = DIFFERENCE_LIM;
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

            // Вытаскиваем каждый из блоб (для смайлика их всего должно быть три)
            var cropFilter = new AForge.Imaging.Filters.Crop(rectangles[0]);
            var uMouth = cropFilter.Apply(unmanaged);

            cropFilter.Rectangle = rectangles[1];
            var uEye1 = cropFilter.Apply(unmanaged);

            cropFilter.Rectangle = rectangles[2];
            var uEye2 = cropFilter.Apply(unmanaged);

            // Масштабируем картинки
            var scaleFilter = new AForge.Imaging.Filters.ResizeBilinear(90, 90);
            uMouth = scaleFilter.Apply(uMouth);
            scaleFilter.NewWidth = 50;
            scaleFilter.NewHeight = 50;
            uEye1 = scaleFilter.Apply(uEye1);
            uEye2 = scaleFilter.Apply(uEye2);

            var blobs = new Bitmap[3]
            {
                uMouth.ToManagedImage(),
                uEye1.ToManagedImage(),
                uEye2.ToManagedImage()
            };

            return blobs;
        }

        private static bool SatisfiesRGBComponentTreshold(Color color)
            => color.R <= RGB_COMPONENT_TRESHOLD && color.G <= RGB_COMPONENT_TRESHOLD && color.B <= RGB_COMPONENT_TRESHOLD;
    }
}
