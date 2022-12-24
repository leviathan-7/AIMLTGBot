using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NeuralNetwork1 
{
    /// <summary>
    /// Тип фигуры
    /// </summary>
    public enum FigureType : byte { Smile = 0, Sad, Neutral, Surprised, Angry, Undef };
    
    public class GenerateImage
    {
        /// <summary>
        /// Текущая сгенерированная фигура
        /// </summary>
        public FigureType currentFigure = FigureType.Undef;

        /// <summary>
        /// Количество классов генерируемых фигур (5 - максимум)
        /// </summary>
        public int EmotionsCount { get; set; } = 5;


        public List<Sample> LoadTrainSamples()
        {
            string smilePath = @"C:\Users\1\Desktop\AIMLTGBot\Сеть сохраняющая в память\NeuralNetwork1\эмоции\тест\улыбка";
            string sadPath = @"C:\Users\1\Desktop\AIMLTGBot\Сеть сохраняющая в память\NeuralNetwork1\эмоции\тест\грусть";
            string angryPath = @"C:\Users\1\Desktop\AIMLTGBot\Сеть сохраняющая в память\NeuralNetwork1\эмоции\тест\злой";
            string neutralPath = @"C:\Users\1\Desktop\AIMLTGBot\Сеть сохраняющая в память\NeuralNetwork1\эмоции\тест\нейтральный";
            string surprisedPath = @"C:\Users\1\Desktop\AIMLTGBot\Сеть сохраняющая в память\NeuralNetwork1\эмоции\тест\удивление";

            var smiles = Directory.GetFiles(smilePath).Select(filename => new Sample(ImageEncoder.Flatten(new Bitmap(filename)), EmotionsCount, FigureType.Smile));
            var sads = Directory.GetFiles(sadPath).Select(filename => new Sample(ImageEncoder.Flatten(new Bitmap(filename)), EmotionsCount, FigureType.Sad));
            var angries = Directory.GetFiles(angryPath).Select(filename => new Sample(ImageEncoder.Flatten(new Bitmap(filename)), EmotionsCount, FigureType.Angry));
            var neutrals = Directory.GetFiles(neutralPath).Select(filename => new Sample(ImageEncoder.Flatten(new Bitmap(filename)), EmotionsCount, FigureType.Neutral));
            var surpriseds = Directory.GetFiles(surprisedPath).Select(filename => new Sample(ImageEncoder.Flatten(new Bitmap(filename)), EmotionsCount, FigureType.Surprised));

            return smiles
                .Concat(sads)
                .Concat(angries)
                .Concat(neutrals)
                .Concat(surpriseds)
                .ToList();
        }

        public List<Sample> LoadTestSamples()
        {
            string smilePath = @"C:\Users\1\Desktop\AIMLTGBot\Сеть сохраняющая в память\NeuralNetwork1\эмоции\обучение\улыбка";
            string sadPath = @"C:\Users\1\Desktop\AIMLTGBot\Сеть сохраняющая в память\NeuralNetwork1\эмоции\обучение\грусть";
            string angryPath = @"C:\Users\1\Desktop\AIMLTGBot\Сеть сохраняющая в память\NeuralNetwork1\эмоции\обучение\злой";
            string neutralPath = @"C:\Users\1\Desktop\AIMLTGBot\Сеть сохраняющая в память\NeuralNetwork1\эмоции\обучение\нейтральный";
            string surprisedPath = @"C:\Users\1\Desktop\AIMLTGBot\Сеть сохраняющая в память\NeuralNetwork1\эмоции\обучение\удивление";

            var smiles = Directory.GetFiles(smilePath).Select(filename => new Sample(ImageEncoder.Flatten(new Bitmap(filename)), EmotionsCount, FigureType.Smile));
            var sads = Directory.GetFiles(sadPath).Select(filename => new Sample(ImageEncoder.Flatten(new Bitmap(filename)), EmotionsCount, FigureType.Sad));
            var angries = Directory.GetFiles(angryPath).Select(filename => new Sample(ImageEncoder.Flatten(new Bitmap(filename)), EmotionsCount, FigureType.Angry));
            var neutrals = Directory.GetFiles(neutralPath).Select(filename => new Sample(ImageEncoder.Flatten(new Bitmap(filename)), EmotionsCount, FigureType.Neutral));
            var surpriseds = Directory.GetFiles(surprisedPath).Select(filename => new Sample(ImageEncoder.Flatten(new Bitmap(filename)), EmotionsCount, FigureType.Surprised));

            return smiles
                .Concat(sads)
                .Concat(angries)
                .Concat(neutrals)
                .Concat(surpriseds)
                .ToList();
        }
    }
}
