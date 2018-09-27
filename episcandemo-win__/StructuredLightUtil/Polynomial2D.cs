using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;

namespace StructuredLightUtil
{
    public class Polynomial2D
    {
        // 2d-polynomial fitting
        // z = a_00 x^2 + a_02 y^2 + a_11 xy + a_10 x + a_01 y + a_00
        public static readonly int Deg1 = 2;
        public static readonly int Deg2 = 2;
        public double[,] Coef { set; get; }

        public Polynomial2D()
        {
            Coef = new double[Deg1 + 1, Deg2 + 1];
        }

        static public Polynomial2D Fit(double[][] xdata, double[] ydata)
        {
            var model = new Polynomial2D();
            var n = xdata.Count();
            var A = new double[n, (Deg1 + 1) * (Deg2 + 1)];
            int dmin = Math.Min(Deg1, Deg2), dmax = Math.Max(Deg1, Deg2);
            var minPoints = (dmin + 1) * dmin / 2 + (dmax - dmin) * (dmin + 1);
            if (n < minPoints)
                throw new Exception();

            for (int t = 0; t < n; t++)
            {
                double x1n = 1, x2n = 1;
                int k = 0;
                for (int i = 0; i <= Deg2; i++, x2n *= xdata[t][1])
                {
                    x1n = 1;
                    for (int j = 0; j <= Deg1; j++, x1n *= xdata[t][0])
                    {
                        if (i + j <= Math.Max(Deg1, Deg2))
                            A[t, k] = x1n * x2n;
                        k++;
                    }
                }
            }

            var x = Matrix.Solve(A, ydata, true);
            for (int i = 0; i < (Deg1 + 1) * (Deg2 + 1); i++)
            {
                model.Coef[i / (Deg2 + 1), i % (Deg2 + 1)] = x[i];
            }
            //Buffer.BlockCopy(x, 0, model.Coef, 0, x.Length);
            for (int i = 0; i < Deg1; i++)
                for (int j = 0; j < Deg2; j++)
                    if (i + j > dmax) model.Coef[i, j] = 0;
            model.Coef = model.Coef.Transpose(true);
            return model;
        }

        public double Evaluate(double x1, double x2)
        {
            var A = new double[Deg1 + 1, Deg2 + 1];
            double x1n = 1, x2n = 1;
            for (int i = 0; i <= Deg1; i++, x1n *= x1, x2n = 1)
                for (int j = 0; j <= Deg2; j++, x2n *= x2)
                    if (i + j <= Math.Max(Deg1, Deg2))
                        A[i, j] = x1n * x2n;

            return Coef.Multiply(A).Sum();
        }
    }
}
