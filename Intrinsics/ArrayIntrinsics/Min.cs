using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace IntrinsicsPlayground
{
    unsafe partial class ArrayIntrinsics
    {
        public static int Min_Avx2(int[] array)
        {
            if (array.Length == 0)
                return 0;
            if (array.Length == 1)
                return array[0];

            Vector256<int> min = Avx.SetAllVector256(int.MinValue);
            fixed (int* ptr = &array[0])
            {
                for (int i = 0; i < array.Length; i += 8) //16 for AVX512
                {
                    var current = Avx.LoadVector256(ptr + i);
                    min = Avx2.Min(current, min);
                }
            }

            // seems like there is no simple way to calculate horizontal minimum in __m256 
            // so let's just do simple FOR for these 8 values
            var minArray = new int[8];
            fixed (int* result = &minArray[0])
                Avx.Store(result, min);

            return Min_Soft(minArray);
        }

        private static int Min_Soft(int[] array) // for small chunks
        {
            int min = int.MaxValue;
            for (int i = 0; i < array.Length; i++)
            {
                var item = array[i];
                if (item < min)
                    min = item;
            }
            return min;
        }
    }
}
