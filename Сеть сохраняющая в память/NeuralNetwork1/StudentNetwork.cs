using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
namespace NeuralNetwork1
{
    public class StudentNetwork : BaseNetwork
    {

        private string path = @"c:\NeyronNetwork.txt";
        //скорость обучения
        private double eta = 0.1;

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

        public override int Train(Sample sample, double acceptableError, bool parallel)
        {
            int iters = 1;
            while (Teacher(sample.input, sample.Output, parallel) > acceptableError)
                ++iters;

            return iters;
        }

        public override double TrainOnDataSet(SamplesSet samplesSet, int epochsCount, double acceptableError, bool parallel)
        {
            //  Сначала надо сконструировать массивы входов и выходов
            double[][] inputs = new double[samplesSet.Count][];
            double[][] outputs = new double[samplesSet.Count][];

            //  Теперь массивы из samplesSet группируем в inputs и outputs
            for (int i = 0; i < samplesSet.Count; ++i)
            {
                inputs[i] = samplesSet[i].input;
                outputs[i] = samplesSet[i].Output;
            }

            //  Текущий счётчик эпох
            int epoch_to_run = 0;

            double error = double.PositiveInfinity;

            while (epoch_to_run < epochsCount && error > acceptableError)
            {
                epoch_to_run++;
                error = TeacherEpoch(inputs, outputs, parallel);
            }

            return error;
        }

        protected override double[] Compute(double[] input)
        {
            for (int i = 0; i < N; i++)
                input = Layers[i].Compute(input);

            return input;
        }

        private double TeacherEpoch(double[][] inputs, double[][] outputs, bool parallel)
        {
            double sum = 0;
            if (parallel)
                Parallel.For(0, inputs.Length, i =>{ sum += Teacher(inputs[i], outputs[i], parallel); });
            else
                for (int i = 0; i < inputs.Length; i++)
                    sum += Teacher(inputs[i], outputs[i], parallel);
            return sum;
        }
        private double Teacher(double[] input, double[] output, bool parallel)
        {
            //Ообратное распространение ошибки
            Tuple<Matrix, Matrix>[] Nabla = Backprop(input, output, parallel);

            //изменение слоёв (обучение сети)
            for (int i = 0; i < N; i++)
                Layers[i].BackpropPlus(Nabla[i], eta);

            //Оценка работы сети
            double[] RealOutput = Compute(input);
            double Sum = 0;
            for (int i = 0; i < output.Length; i++)
                Sum += Math.Pow((output[i] - RealOutput[i]), 2);
            return Sum / 2;
        }
        
        private Tuple<Matrix, Matrix>[] Backprop(double[] activation, double[] output, bool parallel)
        {
            //прямой проход
            List<double[]> activations = new List<double[]> { activation };

            for (int i = 0; i < N; i++)
            {
                activation = Layers[i].Compute(activation);
                activations.Add(activation);
            }

            //инициализации изменений весов и сдвигов
            Matrix[] delta_shift = new Matrix[N];
            Matrix[] delta_w = new Matrix[N];

            //обратный проход
            Matrix Error = Matrix.ToMatrix(output).Minus(activation).Adamar(Matrix.ToMatrix(output).PrSigma());
            delta_w[N - 1] = Error.Dot(activations[activations.Count - 2]);
            delta_shift[N - 1] = Error;

            for (int i = N - 1; i > 0; i--)
            {
                Error = Layers[i].Main_Matrix.T().Mul(Error);
                Matrix M = Matrix.ToMatrix(activations[i]).PrSigma();
                Error = Error.Adamar(M);
                delta_w[i - 1] = Error.Dot(activations[i - 1]);
                delta_shift[i - 1] = Error;
            }

            //возвращение результата
            List<Tuple<Matrix, Matrix>> T = new List<Tuple<Matrix, Matrix>> { };
            for (int i = 0; i < N; i++)
                T.Add(new Tuple<Matrix, Matrix>(delta_w[i], delta_shift[i]));

            return T.ToArray();
        }
        public override void Save() 
        {
            try
            {
                FileInfo fileInfo = new FileInfo(path);
                StreamWriter sw = fileInfo.CreateText();
                sw.WriteLine(Layers.Count);
                foreach (var item in Layers)
                {
                    List<string> arr = item.ToFile();
                    foreach (var a in arr)
                        sw.WriteLine(a);
                }
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public override void Load() 
        {
            List<Layer> NewLayers = new List<Layer> { };
            string[] readText = File.ReadAllLines(path);
            int N = int.Parse(readText[0]);
            int j = 1;
            for (int i = 0; i < N; i++)
            {
                Layer L = new Layer(readText,ref j);
                NewLayers.Add(L);
            }
            Layers = NewLayers;
            N = Layers.Count;
        }
    }
}