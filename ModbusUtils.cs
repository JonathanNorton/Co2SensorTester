using System.Diagnostics;
using System.Linq;

namespace Co2SensorTester
{
    public static class ModbusUtils
    {
        public static int Get32(ushort[] regs, int offset)
        {
            int high = regs[offset];
            int low = regs[offset + 1];

            int result = high;

            result <<= 16;

            result |= low;

            return result;
        }

        public static double BuildDouble(int val, int div)
        {
            double v = val;
            v /= div;
            return v;
        }
    }
}
