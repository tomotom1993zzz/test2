using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using EpiscanUtil;

namespace UserScriptHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var script = ScriptLoader.Load(@"scripts\TestScript.cs");
        }
    }
}
