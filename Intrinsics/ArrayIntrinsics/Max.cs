using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace IntrinsicsPlayground
{
    unsafe partial class ArrayIntrinsics
    {
        public static int Max_Avx2(int[] array)
        {
            if (array.Length == 0)
                return 0;

            const int vecSize = 8;

            if (array.Length < vecSize)
                return Max_Soft(array);

            int i = 0;
            Vector256<int> max = Avx.SetAllVector256(int.MinValue);
            fixed (int* ptr = &array[0])
            {
                for (; i <= array.Length - vecSize; i += vecSize) //16 for AVX512
                {
                    var current = Avx.LoadVector256(ptr + i);
                    max = Avx2.Max(current, max);
                }
            }

            // seems like there is no simple way to calculate horizontal maximum in __m256 
            // so let's just do simзle FOR for these 8 values (+ the rest if array.Length % 8 != 0)
            var maxArray = new int[vecSize + array.Length - i];
            fixed (int* result = &maxArray[0])
                Avx.Store(result, max);

            if (maxArray.Length > vecSize) // copy the rest of array into final array
                Array.Copy(array, i, maxArray, vecSize, maxArray.Length - vecSize);

            return Max_Soft(maxArray);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Max_Soft(int[] array) // for small chunks
        {
            int max = Int32.MinValue;
            for (int i = 0; i < array.Length; i++)
            {
                var item = array[i];
                if (item > max)
                    max = item;
            }
            return max;
        }
    }
}
