using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public static StudentNetwork ReadFromFile(string path)
        {
            //TODO: прочесть готовую сеть из файла
            return null;
        }
        public double[] Compute(double[] input)
        {
            for (int i = 0; i < N; i++)
                input = Layers[i].Compute(input);

            return input;
        }

    }
}