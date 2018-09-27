using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace StructuredLightUtil
{
    public class StripeLight
    {
        private OpenCvSharp.Size projSize;
        private bool xaxis, yaxis;
        private bool inversePattern;
        private int lineWidth, lineShift;
        private Bitmap[] patternImages;


        public Bitmap[] PatternImages
        {
            get
            {
                if (patternImages == null)
                {
                    var patterns = GeneratePatterns();
                    patternImages = patterns.Select(p => BitmapConverter.ToBitmap(p)).ToArray();
                }
                return patternImages;
            }
        }

        public StripeLight(System.Drawing.Size projSize, int lineWidth, int lineShift, bool xaxis = true, bool yaxis = false, bool inversePattern = false)
        {
            this.projSize = new OpenCvSharp.Size(projSize.Width, projSize.Height);
            this.lineWidth = lineWidth;
            this.lineShift = lineShift;
            this.xaxis = xaxis;
            this.yaxis = yaxis;
            this.inversePattern = inversePattern;
        }

        public Mat[] GeneratePatterns()
        {
            var patterns = new List<Mat>();
            if (xaxis)
            {
                // projSize.Width = 1280
                //var nSteps = (projSize.Width - lineWidth) / lineShift + 1;
                var nSteps = (100 - lineWidth) / lineShift + 1;
                for (int i = 0; i < nSteps; i++)
                {
                    var pattern = Mat.Zeros(projSize, MatType.CV_8UC1).ToMat();
                    
                    pattern.Col[i * lineShift + 640, i * lineShift + lineWidth + 640].SetTo(255);
                    if (inversePattern)
                        pattern = 255 - pattern;

                    pattern.Col[0].SetTo(255);
                    pattern.Col[projSize.Width - 1].SetTo(255);
                    patterns.Add(pattern);
                }
            }
            return patterns.ToArray();
        }
    }
}
