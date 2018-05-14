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
                return Sum_Soft(array);
            
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
            var result = new float[vecSize + array.Length - i];
            
            fixed (float* ptr = &result[0])
                Avx.Store(ptr, sum);

            if (array.Length > vecSize) // copy the rest of array into final array
                Array.Copy(array, i, result, vecSize, result.Length - vecSize);

            return Sum_Soft(array);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float Sum_Soft(float[] array)
        {
            float sum = 0.0f;
            for (int i = 0; i < array.Length; i++)
                sum += array[i];
            return sum;
        }
    }
}
