using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend
{
    public class NodeJsOptions
    {
        public string InterpreterPath { get; set; } = "node";
        public string NodeJsEntryPoint { get; set; } = "../../../../lib/src/index.js";

    }
}
