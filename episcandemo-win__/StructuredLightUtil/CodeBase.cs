using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructuredLightUtil
{
    public class CodeBase
    {
        public virtual Mat[] GeneratePatterns() 
            => throw new NotImplementedException();

        public virtual  Mat[] Decode(Mat[] imgs)
            => throw new NotImplementedException();

        public virtual Mat CovertToDisparity(Mat decoded, DisparityCalibration calib)
            => throw new NotImplementedException();
    }
}
