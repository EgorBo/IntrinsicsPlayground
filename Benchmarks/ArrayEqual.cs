using System.Linq;
using BenchmarkDotNet.Attributes;

namespace IntrinsicsPlayground
{
    public class ArrayEqual : ArrayBenchmarkBase
    {
        [Benchmark]
        public bool ArrayEqual_LINQ_SequenceEqual()
        {
            return Enumerable.SequenceEqual(ArrayOfFloats, ArrayOfFloats2);
        }

        [Benchmark]
        public bool ArrayEqual_Simple()
        {
            return ArrayEqual_Simple(ArrayOfFloats, ArrayOfFloats2);
        }

        [Benchmark(Baseline = true)]
        public bool ArrayEqual_AVX2()
        {
            return ArrayIntrinsics.SequenceEqual_Avx(ArrayOfFloats, ArrayOfFloats2);
        }

        public static bool ArrayEqual_Simple(float[] array1, float[] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            if (array1.Length < 1) //both are empty
                return true;

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i]) //bounds check for array2, also compare using eps?
                    return false;
            }

            return true;
        }
    }
}
