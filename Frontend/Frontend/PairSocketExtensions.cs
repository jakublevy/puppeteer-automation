using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;

namespace Frontend
{
    public static class PairSocketExtensions
    {
        public static bool ReceiveFrameStringTimeout(this PairSocket pair, out string result, int timeoutMs)
        {
            Stopwatch sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds <= timeoutMs)
            {
                if (pair.HasIn)
                {
                    result = pair.ReceiveFrameString();
                    return true;
                }
            }
            sw.Stop();
            result = null;
            return false;
        }
    }
}
