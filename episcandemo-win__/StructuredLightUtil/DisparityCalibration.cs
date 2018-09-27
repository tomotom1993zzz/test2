using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;
using Accord.IO;
using OpenCvSharp;

namespace StructuredLightUtil
{
    public class DisparityCalibration
    {
        private Polynomial2D model;
        public DisparityCalibration()
        {
            model = new Polynomial2D()
            {
                Coef = new double[,] {
                    { 33.8504792299378, 0.0165929854877602, -8.279223601177E-06 },
                    { 0.82796340799605, 1.57301601046165E-05, 0 },
                    { -1.30466960028068E-05, 0, 0 }
                }
            };
        }

        public void Save(string filename = "calib.txt")
        {
            var writer = new CsvWriter(filename);
            writer.Write(model.Coef);
            writer.Dispose();
        }

        public bool Load(string filename = "calib.txt")
        {
            if (!System.IO.File.Exists(filename)) return false;

            var reader = new CsvReader(filename, false);
            model.Coef = reader.ToMatrix<double>();
            reader.Dispose();
            return true;
        }

        public double Warp(double x1, double x2)
        {
            return model.Evaluate(x1, x2);
        }

        public void CalibrateCoefficient(Mat decoded, double d0)
        {
            var imgSize = decoded.Size();
            var P = new List<double[]>();
            var T = new List<double>();
            for (int i = 0; i < imgSize.Height; i+=1)
            {
                for (int j = 0; j < imgSize.Width; j+=1)
                {
                    var p = (double)decoded.At<float>(i, j);
                    if (p < 0) continue;

                    P.Add(new[] { p, i });
                    T.Add(j + d0);
                }
            }
            model = Ransac(P.ToArray(), T.ToArray());
        }

        private Polynomial2D Ransac(double[][] xdata, double[] ydata, 
            double threshold = 1.0, int maxEvaluations = 500, int maxSamplings = 30)
        {
            // We are going to find the best model (which fits
            //  the maximum number of inlier points as possible).
            Polynomial2D bestModel = null;
            int[] bestInliers = null;
            int maxInliers = 0;

            // For this we are going to search for random samples
            //  of the original points which contains no outliers.

            int count = 0;  // Total number of trials performed
            double N = maxEvaluations;   // Estimative of number of trials needed.

            // While the number of trials is less than our estimative,
            //   and we have not surpassed the maximum number of trials
            while (count < N && count < maxEvaluations)
            {
                int[] idx;
                Polynomial2D model = null;
                
                // Select at random s datapoints to form a trial model.
                idx = Vector.Sample(ydata.Length).Take(maxSamplings).ToArray();
                double[][] inlierX = xdata.Get(idx);
                double[] inlierY = ydata.Get(idx);

                model = Polynomial2D.Fit(inlierX, inlierY);

                // Now, evaluate the distances between total points and the model returning the
                //  indices of the points that are inliers (according to a distance threshold t).
                
                idx = xdata
                    .Where((x, i) => ydata[i] - model.Evaluate(x[0], x[1]) < threshold)
                    .Select((x, i) => i).ToArray();

                // Check if the model was the model which highest number of inliers:
                if (idx.Length > maxInliers)
                {
                    maxInliers = idx.Length;   // Set the new maximum,
                    bestModel = model;         // This is the best model found so far,
                    bestInliers = idx;         // Store the indices of the current inliers.
                }

                count++; // Increase the trial counter.
            }

            return bestModel;
        }
    }

}
