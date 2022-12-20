using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMLTGBot
{
    class Layer
    {
        public Matrix Main_Matrix;
        public Matrix Shift_Matrix;
        public Layer(int a, int b)
        {
            Main_Matrix = new Matrix(b, a);
            Shift_Matrix = new Matrix(b, 1);
        }
        public double[] Compute(double[] input)
        {
            return Main_Matrix.Mul(input).Plus(Shift_Matrix).Sigma().ToArray();
        }

    }
}
