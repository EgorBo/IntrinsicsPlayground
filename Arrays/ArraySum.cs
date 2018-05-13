using System.Linq;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;

namespace IntrinsicsPlayground
{
    public class ArraySum
    {
        float[] _testArray;

        [GlobalSetup]
        public void GlobalSetup()
        {
            const int count = 1024 * 32;
            _testArray = Enumerable.Range(0, count).Select(i => (float)i).ToArray();
        }

        [Benchmark]
        public float Sum_LINQ()
        {
            // LINQ is so slow :(
            return _testArray.Sum();
        }

        [Benchmark]
        public float Sum_Simple()
        {
            return Sum_Simple(_testArray);
        }

        [Benchmark(Baseline = true)]
        public float Sum_AVX()
        {
            return Sum_AVX(_testArray);
        }

        public static float Sum_Simple(float[] array)
        {
            float result = 0;
            for (int i = 0; i < array.Length; i++)
                result += array[i]; // no bounds check
            return result;
        }

        public static unsafe float Sum_AVX(float[] array)
        {
            Vector256<float> sum = Avx.SetZeroVector256<float>();
            fixed (float* ptr = &array[0])
            {
                for (int i = 0; i < array.Length; i += 8)
                {
                    var current = Avx.LoadVector256(ptr + i);
                    sum = Avx.Add(current, sum);
                }
            }

            // sum all values in __m256 (horizontal sum)
            var ha = Avx.HorizontalAdd(sum, sum);
            var ha2 = Avx.HorizontalAdd(ha, ha);
            var lo = Avx.ExtractVector128(ha2, 1);
            var resultV = Sse.Add(Avx.GetLowerHalf(ha2), lo);

            return Sse.ConvertToSingle(resultV);
        }

        public static unsafe float Sum_AVX_stackalloc(float[] array)
        {
            Vector256<float> sum = Avx.SetZeroVector256<float>();
            fixed (float* ptr = &array[0])
            {
                for (int i = 0; i < array.Length; i += 8)
                {
                    var current = Avx.LoadVector256(ptr + i);
                    sum = Avx.Add(current, sum);
                }
            }

            // store __m256 into float[8] and sum all values via code - will it be slower than Sum_AVX()?
            var result = stackalloc float[8];
            Avx.Store(result, sum);

            return *result + *(result + 1) + *(result + 2) + *(result + 3)
                + *(result + 4)+ *(result + 5)+ *(result + 6)+ *(result + 7);
        }
    }
}
