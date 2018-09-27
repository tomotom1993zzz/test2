using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace StructuredLightUtil
{
    public class StructuredLight
    {
        public DisparityCalibration Calib { private set; get; }
        private CodeBase code;
        private Bitmap[] patternImages;

        StructuredLight() { }
        public StructuredLight(System.Drawing.Size size, bool xaxis = false, bool yaxis = true, bool inversePattern = true)//horizonal codes for disparity-gating 
        {
            var cvSize = new OpenCvSharp.Size(size.Width, size.Height);
            code = new XorCode(cvSize, xaxis, yaxis, inversePattern);
            Calib = new DisparityCalibration();
        }

        static public StructuredLight CalibrationSetup(System.Drawing.Size size)
        {
            var cvSize = new OpenCvSharp.Size(size.Width, size.Height);
            return new StructuredLight
            {
                code = new XorCode(cvSize, false, true, true),//horizonal codes for disparity-gating 
                Calib = new DisparityCalibration()
            };
        }

        public bool LoadCalibrationData(string filename = "calib.txt")
        {
            return Calib.Load(filename);
        }
        
        public Bitmap[] PatternImages
        {
            get
            {
                if (patternImages == null)
                {
                    var patterns = code.GeneratePatterns();
                    patternImages = patterns.Select(p => BitmapConverter.ToBitmap(p)).ToArray();
                }
                return patternImages;
            }
        }

        public float[][] CalculateDisparity(Mat[] imgs)
        {
            var decoded = code.Decode(imgs);
            var disparity = code.CovertToDisparity(decoded[0], Calib);

            // convert mat to float array
            var dispArr = new float[disparity.Height][];
            for (int i = 0; i < disparity.Height; i++)
            {
                dispArr[i] = new float[disparity.Width];
                for (int j = 0; j < disparity.Width; j++)
                    dispArr[i][j] = disparity.At<float>(i, j);
            }

            return dispArr;
        }

        public float[][] CalculateDisparity(int width, int height, byte[][] capturedImages)
        {
            var tmp = capturedImages[0];
            var imgs = capturedImages
                .Select(buff => Array.ConvertAll(buff, b => Math.Min(255, (int)b)))
                .Select(arr => (new Mat(height, width, MatType.CV_32SC1, arr) * 255).ToMat())
                .ToArray();

            var decoded = code.Decode(imgs);
            var disparity = code.CovertToDisparity(decoded[0], Calib);

            //Cv2.ImShow("disparity", disparity * 0.002f);
            //Cv2.WaitKey();

            // convert mat to float array
            var dispArr = new float[disparity.Height][];
            for (int i = 0; i < disparity.Height; i++)
            {
                dispArr[i] = new float[disparity.Width];
                for (int j = 0; j < disparity.Width; j++)
                    dispArr[i][j] = disparity.At<float>(i, j);
            }

            return dispArr;
        }

        public void Calibrate(double planeDistance, double baseLine, double focalLength, Mat[] imgs)
        {
            var d0 = baseLine * focalLength / planeDistance;
            var decoded = code.Decode(imgs)[0];

            var filtered = new Mat();
            Cv2.BilateralFilter(decoded, filtered, -1, 4, 4);
            Calib.CalibrateCoefficient(filtered, d0);
        }

        public void Calibrate(double planeDistance, double baseLine, double focalLength, int width, int height, byte[][] capturedImages)
        {
            var imgs = capturedImages
                .Select(buff => Array.ConvertAll(buff, b => Math.Min(255, (int)b)))
                .Select(arr => (new Mat(height, width, MatType.CV_32SC1, arr) * 255).ToMat())
                .ToArray();

            var d0 = baseLine * focalLength / planeDistance;
            var decoded = code.Decode(imgs)[0];

            var filtered = new Mat();
            Cv2.BilateralFilter(decoded, filtered, -1, 4, 4);
            Calib.CalibrateCoefficient(filtered, d0);
        }


        public void Calibrate2(double planeDistance, double baseLine, double focalLength, int width, int height, Mat[] imgs)
        {
            var d0 = baseLine * focalLength / planeDistance;
            var decoded = code.Decode(imgs)[0];

            var filtered = new Mat();
            Cv2.BilateralFilter(decoded, filtered, -1, 4, 4);
            Calib.CalibrateCoefficient(filtered, d0);
        }
    }
}
