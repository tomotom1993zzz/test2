using System;
using System.Diagnostics;
using UserScriptHost;
using System.Collections.Generic;
using System.Linq;
using EpiscanUtil;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Threading.Tasks;

public class TestScript : IEpiscanScript
{
    public int YMin { set; get; } = -1;
    public int YMax { set; get; } = -1;
    public int Width { set; get; } = 1;
    public int Shift { set; get; } = 1;
    public int AverageNum { set; get; } = 1;

    public void Configure(MyEpiscan episcan)
    {
        Debug.Write("hello");
    }

    public async Task Run(MyEpiscan episcan)
    {
        if (YMin < 0) YMin = 0;
        if (YMax < 0) YMax = episcan.Screen.Size.Height;

        // generate patterns
        var projSize = new Size(episcan.Screen.Size.Width, episcan.Screen.Size.Height);
        var projNum = (YMax - YMin) / Shift;

        var patterns = new List<Mat>();
        for (int i = Math.Max(YMin, 0); i < Math.Min(YMax - Width, YMax - 1); i += Shift)
        {
            var pattern = new Mat(projSize, MatType.CV_8UC1);
            pattern.RowRange(i, i + Width).SetTo(255);
            patterns.Add(pattern);
        }
        // Switch to synchronized mode
        episcan.Sensor.ShutterMode = MySensor.ShutterModeList.Rolling;

        // capture and save
        for (int i = 0; i < patterns.Length; i++)
        {
            var pattern = patterns[i];
            episcan.Screen.BackgroundMat = pattern;
            await Task.Delay(100);

            var captured = await episcan.CaptureAverage(AverageNum);

            var filename = $"line_{i:D4}.png";
            Cv2.ImWrite(filename, captured);
        }
    }
}
