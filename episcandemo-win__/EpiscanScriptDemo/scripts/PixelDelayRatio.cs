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
    public int DelayMin { set; get; } = -500;
    public int DelayMax { set; get; } = 500;
    public int Shift { set; get; } = 1;
    public float Exposure { set; get; } = 0.5f;
    public int AverageNum { set; get; } = 4;

    public void Configure(MyEpiscan episcan)
    {
        Debug.Write("hello");
    }

    public async Task Run(MyEpiscan episcan)
    {
        // generate patterns
        var projSize = new Size(episcan.Screen.Size.Width, episcan.Screen.Size.Height);
        var pattern = new Mat(projSize, MatType.CV_8UC1);
        pattern.ColRange(projSize.Width / 2 - 50, projSize.Width / 2 + 50).SetTo(255);

        episcan.Screen.BackgroundMat = pattern;
        await Task.Delay(100);

        // Switch to synchronized mode
        episcan.Sensor.ShutterMode = MySensor.ShutterModeList.Rolling;
        for (int delay = DelayMin, i = 0; delay < DelayMax; delay += Shift, i++)
        {
            episcan.SetDelayExposure(delay, Exposure);
            await Task.Delay(1);
            var captured = await episcan.CaptureAverage(AverageNum);
            var filename = $"capture_{i:D5}_{(int)delay:D5}.png";
            Cv2.ImWrite(filename, captured);
        }
    }
}
