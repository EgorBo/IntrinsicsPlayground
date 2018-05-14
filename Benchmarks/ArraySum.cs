using System.Linq;
using BenchmarkDotNet.Attributes;

namespace IntrinsicsPlayground
{
    public class ArraySum : BenchmarkBase
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
