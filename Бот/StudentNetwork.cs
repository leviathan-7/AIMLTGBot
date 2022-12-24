using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace AIMLTGBot
{
    public class StudentNetwork
    {

        // количество слоев
        private int N;

        private List<Layer> Layers;
        public StudentNetwork(int[] structure)
        {
            Layers = new List<Layer> { };
            for (int i = 0; i < structure.Length - 1; i++)
            {
                Layer L = new Layer(structure[i], structure[i + 1]);
                Layers.Add(L);
            }
            N = Layers.Count;
        }
        public StudentNetwork(string path)
        {
            List<Layer> NewLayers = new List<Layer> { };
            string[] readText = File.ReadAllLines(path);
            N = int.Parse(readText[0]);
            int j = 1;
            for (int i = 0; i < N; i++)
            {
                Layer L = new Layer(readText, ref j);
                NewLayers.Add(L);
            }
            Layers = NewLayers;
        }
        public double[] Compute(double[] input)
        {
            Console.WriteLine(N);
            for (int i = 0; i < N; i++)
                input = Layers[i].Compute(input);
            return input;
        }

    }
}