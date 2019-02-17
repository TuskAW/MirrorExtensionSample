using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SerializeFloat
{
    public static class FloatConverterExt
    {
        public static short ToShort(this float value)
        {
            return new FloatConverter(value).ToShort();
        }

        public static float ToFloat(this short value)
        {
            return FloatConverter.FromShort(value);
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct FloatConverter
    {
        [FieldOffset(0)] private readonly float Value;

        [FieldOffset(0)] private uint Bytes;

        public FloatConverter(float value)
        {
            this = default(FloatConverter);
            this.Value = value;
        }

        private uint GetExponent()
        {
            return (Bytes >> 23) & 255U;
        }

        private uint GetFraction()
        {
            return Bytes & 0x7FFFFF;
        }

        public short ToShort()
        {
            if (Math.Abs(Value) < 0.0001f) return 0;
            var s = Value > 0 ? 0U : 1U;
            var e = (GetExponent()) - 127 + 15;
            var f = (GetFraction() >> 13) & 1023U;
            return (short) (f | e << 10 | s << 15);
        }


        public static float FromShort(short v)
        {
            if (v == 0) return 0;

            var s = (v >> 15) & 1;
            var e = (v >> 10) & 31;
            var f = v & 1023U;

            var exp = e - 15 + 127;
            var frac = (int) f << 13;

            var r = (uint) (s << 31 | exp << 23 | frac);

            var fts = new FloatConverter
            {
                Bytes = r
            };

            return fts.Value;
        }
    }
}