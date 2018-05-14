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
            if (array.Length == 1)
                return array[0];

            Vector256<int> max = Avx.SetAllVector256(int.MinValue);
            fixed (int* ptr = &array[0])
            {
                for (int i = 0; i < array.Length; i += 8) //16 for AVX512
                {
                    var current = Avx.LoadVector256(ptr + i);
                    max = Avx2.Max(current, max);
                }
            }

            // seems like there is no simple way to calculate horizontal maximum in __m256 
            // so let's just do simзle FOR for these 8 values
            var maxArray = new int[8];
            fixed (int* result = &maxArray[0])
                Avx.Store(result, max);

            return Max_Soft(maxArray);
        }

        private static int Max_Soft(int[] array) // for small chunks
        {
            int max = int.MinValue;
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
