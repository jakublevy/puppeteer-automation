using System.Diagnostics;
using NetMQ;
using NetMQ.Sockets;

namespace Frontend
{
    public static class PairSocketExtensions
    {
        /// <summary>
        /// Extension method that waits maximally timeoutMs for a string data on pair.
        /// </summary>
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
