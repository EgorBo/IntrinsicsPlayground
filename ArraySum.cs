using System.Linq;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;

namespace IntrinsicsPlayground
{
    public class ArraySum
    {
        float[] testArray;

        [GlobalSetup]
        public void GlobalSetup()
        {
            const int count = 1024 * 32;
            testArray = Enumerable.Range(0, count).Select(i => (float)i).ToArray();
        }

        [Benchmark]
        public float Sum_LINQ()
        {
            return testArray.Sum();
        }

        [Benchmark(Baseline = true)]
        public float Sum_FOR()
        {
            return Sum_FOR(testArray);
        }

        [Benchmark]
        public float Sum_AVX()
        {
            return Sum_AVX(testArray);
        }

        //[Benchmark]
        public float Sum_AVX_stackalloc()
        {
            return Sum_AVX_stackalloc(testArray);
        }

        public static float Sum_FOR(float[] array)
        {
            float result = 0;
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i]; // no bounds check
            }
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

            var t1 = Avx.HorizontalAdd(sum, sum);
            var t2 = Avx.HorizontalAdd(t1, t1);
            var t3 = Avx.ExtractVector128(t2, 1);
            var t4 = Sse.Add(Avx.GetLowerHalf(t2), t3);

            return Sse.ConvertToSingle(t4);
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


            var hi = Avx.ExtractVector128(sum, 1);
            var lo = Avx.ExtractVector128(sum, 0);
            var s = Sse.Add(hi, lo);

            var result = stackalloc float[4];
            Sse.Store(result, s);

            return *result + *(result + 1) + *(result + 2) + *(result + 3);
        }
    }
}
