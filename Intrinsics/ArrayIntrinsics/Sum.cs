using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace IntrinsicsPlayground
{
    unsafe partial class ArrayIntrinsics
    {
        public static float Sum_Avx(float[] array)
        {
            if (array.Length < 1)
                return 0f;

            const int vecSize = 8;

            fixed (float* ptr = &array[0])
            {
                if (array.Length < 8)
                    return Sum_Soft(ptr, array.Length);

                int i = 0;
                Vector256<float> sum = Avx.SetZeroVector256<float>();
                {
                    for (; i <= array.Length - vecSize; i += vecSize)
                    {
                        var current = Avx.LoadVector256(ptr + i);
                        sum = Avx.Add(current, sum);
                    }
                }

                // store __m256 into float[8] and sum all values
                // as an alternative we can do (copied from https://stackoverflow.com/a/18616679/298088)
                /*
                    var t1 = Avx.HorizontalAdd(sum, sum);
                    var t2 = Avx.HorizontalAdd(t1, t1);
                    var t3 = Avx.ExtractVector128(t2, 1);
                    var t4 = Sse.Add(Avx.GetLowerHalf(t2), t3);
                    var hSum =  Sse.ConvertToSingle(t4);
                 */

                var result = stackalloc float[vecSize];
                Avx.Store(result, sum);

                float finalSum = *(result + 0) + *(result + 1) + *(result + 2) + *(result + 3)
                               + *(result + 4) + *(result + 5) + *(result + 6) + *(result + 7);

                if (i < array.Length)
                    finalSum += Sum_Soft(ptr + i, array.Length - i);

                return finalSum;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float Sum_Soft(float* array, int count)
        {
            float sum = 0.0f;
            for (int i = 0; i < count; i++)
                sum += *(array + i);
            return sum;
        }
    }
}
