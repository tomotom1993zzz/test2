using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;

namespace StructuredLightUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            // test code for disparity measurement
            var code = new XorCode(new Size(1280, 1024), true, false, false);
            var patterns = code.GeneratePatterns();

            var roi = new Rect(0, 231, 929, 591);
            var imgs = Enumerable.Range(0, 11)
                .Select(i => Cv2.ImRead($"board/code_{i}.png", ImreadModes.Unchanged))
                .Select(i => new Mat(i, roi))
                .ToArray();


            var calib = new DisparityCalibration();
            var decoded = code.Decode(imgs);
            Cv2.ImWrite("decoded2.png", decoded[0] * 0.1);
            var disparity = code.CovertToDisparity(decoded[0], calib);
            Cv2.ImShow("disparity", disparity * 0.002f);
            Cv2.WaitKey();

            var dispArr = new float[disparity.Height][];
            for (int i = 0; i < disparity.Height; i++)
            {
                dispArr[i] = new float[disparity.Width];
                for (int j = 0; j < disparity.Width; j++)
                    dispArr[i][j] = disparity.At<float>(i, j);
            }
            File.WriteAllLines(@"tmp.txt", dispArr.Select(s => String.Join(" ", s)));

            //disparity.ConvertTo(disparity, MatType.CV_32FC1);

            // test code for calibration
            //var roi = new Rect(0, 231, 929, 591);
            //var imgs = Enumerable.Range(0, 22)
            //    .Select(i => Cv2.ImRead($"board/code_{i}.png", ImreadModes.Unchanged))
            //    .Select(i => new Mat(i, roi))
            //    .ToArray();
            //var structuredLight = StructuredLight.CalibrationSetup(new System.Drawing.Size(1280, 1024));

            //for (int i = 0; i < imgs.Length; i++)
            //{
            //    imgs[i].ConvertTo(imgs[i], MatType.CV_32FC1);
            //}
            //structuredLight.Calibrate2(440, 80, 1612, 1280, 1024, imgs);
            //structuredLight.Calib.Save();


            //var code = new XorCode(new Size(1280, 1024), true, false, true);
            //var decoded = code.Decode(imgs);
            //var disparity = XorCode.CovertToDisparity(decoded[0], structuredLight.Calib);
            //Cv2.ImShow("disparity", disparity * 0.001f);
            //Cv2.WaitKey();
        }
    }
}
