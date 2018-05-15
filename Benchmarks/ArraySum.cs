using System.Linq;
using BenchmarkDotNet.Attributes;
using JM.LinqFaster.SIMD;

namespace IntrinsicsPlayground
{
    public class ArraySum : ArrayBenchmarkBase
    {
        [Benchmark]
        public float Sum_LINQ()
        {
            return ArrayOfFloats.Sum();
        }

        [Benchmark]
        public float Sum_Simple()
        {
            return Sum_Simple(ArrayOfFloats);
        }

        [Benchmark]
        public float Sum_LinqFasterLib()
        {
            return ArrayOfFloats.SumS();
        }

        [Benchmark(Baseline = true)]
        public float Sum_Avx()
        {
            return ArrayIntrinsics.Sum_Avx(ArrayOfFloats);
        }

        public static float Sum_Simple(float[] array)
        {
            float result = 0;
            for (int i = 0; i < array.Length; i++)
                result += array[i]; // no bounds check
            return result;
        }
    }
}
