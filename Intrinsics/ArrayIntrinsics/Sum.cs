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

            if (array.Length < 8)
                return Sum_Soft(array, 0);
            
            int i = 0;
            Vector256<float> sum = Avx.SetZeroVector256<float>();
            fixed (float* ptr = &array[0])
            {
                for (; i <= array.Length - vecSize; i += vecSize)
                {
                    var current = Avx.LoadVector256(ptr + i);
                    sum = Avx.Add(current, sum);
                }
            }

            // store __m256 into float[8] and sum all values
            var result = stackalloc float[vecSize];
            Avx.Store(result, sum);

            float finalSum = *(result + 0) + *(result + 1) + *(result + 2) + *(result + 3)
                           + *(result + 4) + *(result + 5) + *(result + 6) + *(result + 7);

            if (i < array.Length - 1)
                finalSum += Sum_Soft(array, i);

            return finalSum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float Sum_Soft(float[] array, int offset)
        {
            float sum = 0.0f;
            for (; offset < array.Length; offset++)
                sum += array[offset];
            return sum;
        }
    }
}
