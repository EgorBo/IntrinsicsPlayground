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

            if (array.Length == 1)
                return array[0];

            Vector256<float> sum = Avx.SetZeroVector256<float>();
            fixed (float* ptr = &array[0])
            {
                for (int i = 0; i < array.Length; i += 8)
                {
                    var current = Avx.LoadVector256(ptr + i);
                    sum = Avx.Add(current, sum);
                }
            }

            // store __m256 into float[8] and sum all values
            var result = stackalloc float[8];
            Avx.Store(result, sum);

            return *result + *(result + 1) + *(result + 2) + *(result + 3)
                + *(result + 4)+ *(result + 5)+ *(result + 6)+ *(result + 7);
        }

        public static float Sum_Avx_ha(float[] array)
        {
            if (array.Length < 1)
                return 0f;

            if (array.Length == 1)
                return array[0];

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
            // based on this answer: https://stackoverflow.com/a/18616679/298088
            var t1 = Avx.HorizontalAdd(sum, sum);
            var t2 = Avx.HorizontalAdd(t1, t1);
            var t3 = Avx.ExtractVector128(t2, 1);
            var resultV = Sse.Add(Avx.GetLowerHalf(t2), t3);

            return Sse.ConvertToSingle(resultV);
        }
    }
}
