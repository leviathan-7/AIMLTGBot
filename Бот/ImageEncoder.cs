using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AIMLTGBot
{
    public static class ImageEncoder
    {
        private const int TRESHOLD = 10;
        private static float DifferenceLimit => TRESHOLD / 255.0f;

        public static double[] Flatten(Bitmap original)
        {
            var blobs = ExtractBlobs(original);

            var vector = Vectorize(blobs[0])
                .Concat(Vectorize(blobs[1]))
                .Concat(Vectorize(blobs[2]))
                .ToArray();

            return vector;
        }

        private static Bitmap[] ExtractBlobs(Bitmap original)
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

            // Самый первый блоб - вся картинка. Пропускаем его и берем все оставшиеся
            var rectangles = bc.GetObjectsRectangles().Skip(1).ToArray();
            if (rectangles.Length < 3)
            {
                string message = "Не удалось получить требуемое количество блобов";
                throw new EmotionRecognitionException(message);
            }

            // Вытаскиваем каждый из блоб (для смайлика их всего должно быть три)
            var cropFilter = new AForge.Imaging.Filters.Crop(rectangles[0]);
            var uMouth = cropFilter.Apply(unmanaged);

            cropFilter.Rectangle = rectangles[1];
            var uEye1 = cropFilter.Apply(unmanaged);

            cropFilter.Rectangle = rectangles[2];
            var uEye2 = cropFilter.Apply(unmanaged);

            // Масштабируем картинки
            var scaleFilter = new AForge.Imaging.Filters.ResizeBilinear(100, 100);
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

        private static double[] Vectorize(Bitmap blob)
        {
            double[] vector = new double[blob.Width + blob.Height];
            for (int x = 0; x < blob.Width; x += 1)
            {
                for (int y = 0; y < blob.Height; y += 1)
                {
                    var color = blob.GetPixel(x, y);
                    if (SatisfiesTreshold(color))
                    {
                        vector[x] += 1.0;
                        vector[x + blob.Height] += 1.0;
                    }
                }
            }
            return vector;
        }

        private static bool SatisfiesTreshold(Color color)
            => color.R <= TRESHOLD && color.G <= TRESHOLD && color.B <= TRESHOLD;
    }
}
