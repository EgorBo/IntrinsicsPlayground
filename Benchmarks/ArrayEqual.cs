using System.Linq;
using BenchmarkDotNet.Attributes;

namespace IntrinsicsPlayground
{
    public class ArrayEqual : BenchmarkBase
    {
        [Benchmark]
        public bool ArrayEqual_LINQ_SequenceEqual()
        {
            return Enumerable.SequenceEqual(ArrayOfDoubles, ArrayOfDoubles2);
        }

        [Benchmark]
        public bool ArrayEqual_Simple()
        {
            return ArrayEqual_Simple(ArrayOfDoubles, ArrayOfDoubles2);
        }

        [Benchmark(Baseline = true)]
        public bool ArrayEqual_AVX2()
        {
            return ArrayIntrinsics.SequenceEqual_Avx(ArrayOfDoubles, ArrayOfDoubles2);
        }

        public static bool ArrayEqual_Simple(double[] array1, double[] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i]) //bounds check for array2, also compare using eps?
                    return false;
            }

            return true;
        }
    }
}
