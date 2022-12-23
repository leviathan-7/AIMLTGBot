using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork1
{
    class Layer
    {
        private int x;
        private int y;
        public Matrix Main_Matrix;
        public Matrix Shift_Matrix;
        public Layer(int a, int b)
        {
            Main_Matrix = new Matrix(b, a);
            Shift_Matrix = new Matrix(b, 1);
            x = a;
            y = b;
        }
        public double[] Compute(double[] input)
        {
            return Main_Matrix.Mul(input).Plus(Shift_Matrix).Sigma().ToArray();
        }
        public void BackpropPlus(Tuple<Matrix, Matrix>  Nabla, double eta)
        {
            Main_Matrix = Main_Matrix.Plus(Nabla.Item1.Mul(eta));
            Shift_Matrix = Shift_Matrix.Plus(Nabla.Item2.Mul(eta));
        }
        public List<string> ToFile()
        {
            List<string> Arr = new List<string> { };
            Arr.Add("" + x);
            Arr.Add("" + y);
            List<string> M = Main_Matrix.ToFile();
            List<string> S = Shift_Matrix.ToFile();
            foreach (var item in M)
                Arr.Add(item);
            Arr.Add(S[0]);

            return Arr;
        }
        public Layer(string[] readText, ref int start)
        {
            x = int.Parse(readText[start]);
            y = int.Parse(readText[start + 1]);

            Main_Matrix = new Matrix(readText, start + 2,x,y);
            Shift_Matrix = new Matrix(readText, start + x + 2, 1, y);


            start += x + 3;
        }
    }
}
