using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace EpiscanUtil
{
    public class MyEpiscan
    {
        public MySensor Sensor { private set; get; }
        public MyScreen Screen
        {
            private set => screen = value;
            get
            {
                screen = screen ?? new MyScreen();
                return screen;
            }
        }
        public SyncBoard SyncBoard { private set; get; }
        public int[] ImageRoi { set; get; }

        private MyScreen screen;

        public void InitializeSensor(IntPtr handle, int deviceId)
        {
            Sensor = new MySensor(handle, deviceId);
        }

        public void SetDelayExposure(float delay, float exposure)
        {
            Sensor.ShutterMode = MySensor.ShutterModeList.Rolling;

            try
            {
                Sensor.Delay = (int)(delay - 0.5f * exposure);
                Sensor.Exposure = exposure;
            }
            catch (Exception)
            {
                throw new Exception("failed to set delay / exposure.");
            }
        }

        public async Task<Mat> CaptureAverage(int averageNum = 4)
        {
            // color mode
            var isEightBit = Sensor.PixelFormat == MySensor.PixelFormatList.Mono8
                | Sensor.PixelFormat == MySensor.PixelFormatList.BGR24;
            var isMono = Sensor.PixelFormat == MySensor.PixelFormatList.Mono8
                | Sensor.PixelFormat == MySensor.PixelFormatList.Mono12;

            int height = Sensor.Height, width = Sensor.Width;

            Mat image = null;
            if (isEightBit)
            {
                var arr = await CaptureAverage8Async(averageNum);
                if (isMono)
                    image = new Mat(height, width, MatType.CV_8UC1, arr);
                else
                    image = new Mat(height, width, MatType.CV_8UC3, arr);
            }
            else
            {
                var arr = await CaptureAverage12Async(averageNum);
                if (isMono)
                    image = new Mat(height, width, MatType.CV_16UC1, arr);
                else
                    image = new Mat(height, width, MatType.CV_16UC3, arr);
            }

            var rect = new Rect(ImageRoi[0], ImageRoi[1], ImageRoi[2], ImageRoi[3]);
            return new Mat(image, rect).Clone();
        }


        //
        // for 8-bit image
        //
        public async Task<byte[]> CaptureAverage8Async(int averageNum = 4)
        {
            float[] arr = null;
            for (int i = 0; i < averageNum; i++)
            {
                await Task.Delay(1);
                var frame = Sensor.CaptureFrame();
                float[] farr = Array.ConvertAll(frame, b => (float)b);
                arr = arr ?? new float[farr.Length];
                for (int j = 0; j < farr.Length; j++) arr[j] += farr[j];
            }

            return Array.ConvertAll(arr, b => (byte)Math.Min(255, b / averageNum));
        }

        public async Task<UInt16[]> CaptureAverage12Async(int averageNum = 4)
        {
            float[] arr = null;
            for (int i = 0; i < averageNum; i++)
            {
                await Task.Delay(1);
                var frame = Sensor.CaptureFrame();
                float[] farr = Array.ConvertAll(frame, b => (float)b);
                arr = arr ?? new float[farr.Length];
                for (int j = 0; j < farr.Length; j++) arr[j] += farr[j];
            }
            return Array.ConvertAll(arr, b => (UInt16)Math.Min(65535, b * 16 / averageNum));
        }
    }
}
