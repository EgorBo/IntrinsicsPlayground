using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;

namespace IntrinsicsPlayground.Benchmarks.Misc
{
    public unsafe class LeadingZeroCount
    {
        // benchmarking LZCNT against its soft implementation in Decimal https://github.com/dotnet/corert/blob/a4c6faac2c31bffc6f13287c6cb8c6a7bb9667fd/src/System.Private.CoreLib/src/System/Decimal.DecCalc.cs#L2047

        [Benchmark]
        public int[] LeadingZeroCount_Soft()
        {
            int[] results = new int[100000];
            for (uint i = 1 /* see https://github.com/dotnet/corert/pull/5883#issuecomment-403617647 */ ; i < results.Length; i++)
                results[i] = LeadingZeroCount_SoftStatic(i);
            return results;
        }

        [Benchmark(Baseline = true)]
        public int[] LeadingZeroCount_HW()
        {
            int[] results = new int[100000];
            for (uint i = 1; i < results.Length; i++)
                results[i] = (int)Lzcnt.LeadingZeroCount(i);
            return results;
        }

        public static int LeadingZeroCount_SoftStatic(uint value)
        {
            int c = 1;
            if ((value & 0xFFFF0000) == 0)
            {
                value <<= 16;
                c += 16;
            }
            if ((value & 0xFF000000) == 0)
            {
                value <<= 8;
                c += 8;
            }
            if ((value & 0xF0000000) == 0)
            {
                value <<= 4;
                c += 4;
            }
            if ((value & 0xC0000000) == 0)
            {
                value <<= 2;
                c += 2;
            }
            return c + ((int)value >> 31);
        }
    }
}
