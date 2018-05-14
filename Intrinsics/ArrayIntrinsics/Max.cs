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
                return Max_Soft(array, 0, int.MinValue);

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
            // so let's just do simple FOR for these 8 values
            var maxArray = stackalloc int[vecSize];
            Avx.Store(maxArray, max);

            int finalMax = int.MinValue;
            for (int j = 0; j < 8; j++) 
            {
                // we can't use Max_Soft here
                var item = maxArray[j];
                if (item > finalMax)
                    finalMax = item;
            }

            if (i < array.Length - 1) 
                finalMax = Max_Soft(array, i, finalMax);

            return finalMax;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Max_Soft(int[] array, int offset, int initialMax) // for small chunks
        {
            int max = initialMax;
            for (; offset < array.Length; offset++)
            {
                var item = array[offset];
                if (item > max)
                    max = item;
            }
            return max;
        }
    }
}
