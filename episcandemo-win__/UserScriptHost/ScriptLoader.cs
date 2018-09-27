using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSScriptLibrary;
using Microsoft.CodeAnalysis.Scripting;

namespace UserScriptHost
{
    public class ScriptLoader
    {
        public static dynamic Load(string filename)
        {
            dynamic script = CSScript.RoslynEvaluator.LoadFile<IEpiscanScript>(filename);
            return script._obj;
        }
    }
}
