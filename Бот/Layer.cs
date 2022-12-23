using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMLTGBot
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
        public Layer(string[] readText, ref int start)
        {
            x = int.Parse(readText[start]);
            y = int.Parse(readText[start + 1]);

            Main_Matrix = new Matrix(readText, start + 2, x, y);
            Shift_Matrix = new Matrix(readText, start + x + 2, 1, y);


            start += x + 3;
        }
    }
}
