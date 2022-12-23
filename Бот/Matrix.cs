using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMLTGBot
{
    class Matrix
    {
        private int x;
        private int y;
        private double[,] arr;
        public double X()
        {
            return x;
        }
        public double Y()
        {
            return y;
        }
        public Matrix(int x, int y, double[,] arr)
        {
            this.x = x;
            this.y = y;
            this.arr = arr;
        }
        public Matrix(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.arr = new double[x, y];
            Random rnd = new Random();
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                {
                    int t = rnd.Next(0, 99);
                    double r = t / 100.0 - 0.495;
                    this.arr[i, j] = r;
                }
        }
        public Matrix Mul(Matrix M)
        {
            double[,] A = new double[x, M.y];
            for (int i = 0; i < x; i++)
                for (int j = 0; j < M.y; j++)
                {
                    double S = 0;
                    for (int r = 0; r < y; r++)
                        S += arr[i, r] * M.arr[r, j];
                    A[i, j] = S;
                }
            return new Matrix(x, M.y, A);
        }
        public Matrix Mul(double[] M)
        {
            return Mul(ToMatrix(M));
        }
        public Matrix Plus(Matrix M)
        {
            double[,] A = new double[x, y];
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                     A[i, j] = arr[i, j] + M.arr[i, j];
            return new Matrix(x, y, A);
        }
        public Matrix Minus(Matrix M)
        {
            double[,] A = new double[x, y];
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    A[i, j] = arr[i, j] - M.arr[i, j];
            return new Matrix(x, y, A);
        }
        public Matrix Minus(double[] M)
        {
            return Minus(ToMatrix(M));
        }
        public Matrix Mul(double d)
        {
            double[,] A = new double[x, y];
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    A[i, j] = arr[i,j]* d;
            return new Matrix(x, y, A);
        }
        public Matrix Sigma()
        {
            double[,] A = new double[x, y];
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    A[i, j] = Sigma(arr[i, j]);
            return new Matrix(x, y, A);
        }
        public double[] ToArray()
        {
            double[] A = new double[x];
            for (int i = 0; i < x; i++)
                A[i] = arr[i, 0];
            return A;
        }
        public static Matrix ToMatrix(double[] M)
        {
            double[,] AA = new double[M.Length, 1];
            for (int i = 0; i < M.Length; i++)
                AA[i, 0] = M[i];
            Matrix A = new Matrix(M.Length, 1, AA);
            return A;
        }
        private static double Sigma(double z)
        {
            return 1 / (1 + Math.Pow(Math.E,-z));
        }
        public static double PrSigma(double z)
        {
            return Sigma(z) * (1 - Sigma(z));
        }
        public Matrix PrSigma()
        {
            double[,] A = new double[x, y];
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    A[i, j] = PrSigma(arr[i, j]);
            return new Matrix(x, y, A);
        }
        public Matrix Adamar(Matrix a)
        {
            Matrix M = new Matrix(x, y);
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    M.arr[i, j] = arr[i, j] * a.arr[i, j];
            return M;
        }
        public Matrix Dot(double[] a)
        {
            Matrix M = new Matrix(x,a.Length);
            for (int j = 0; j < a.Length; j++)
                for (int k = 0; k < x; k++)
                    M.arr[k,j] = arr[k, 0] * a[j];
            return M;
        }
        public Matrix T()
        {
            Matrix M = new Matrix(y, x);

            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    M.arr[j, i] = arr[i, j];
            return M;
        }
        public Matrix(string[] readText, int start, int x, int y)
        {
            this.x = y;
            this.y = x;
            this.arr = new double[y, x];

            for (int i = start; i < start + x; i++)
            {
                string str = readText[i];
                string[] Arr = str.Split(' ');
                for (int j = 0; j < y; j++)
                {
                    this.arr[j, i - start] = double.Parse(Arr[j]);
                }
            }
        }
    }
}
