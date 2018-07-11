using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace IntrinsicsPlayground
{
    unsafe partial class ArrayIntrinsics
    {
        public static float Max_Avx(float[] array)
        {
            if (array.Length == 0)
                return 0;

            const int vecSize = 8;

            if (array.Length < vecSize)
                return Max_Soft(array, 0, float.MinValue);

            int i = 0;
            Vector256<float> max = Avx.SetAllVector256(float.MinValue);
            fixed (float* ptr = &array[0])
            {
                for (; i <= array.Length - vecSize; i += vecSize) //16 for AVX512
                {
                    var current = Avx.LoadVector256(ptr + i);
                    max = Avx.Max(current, max);
                }
            }

            float finalMax = ReduceMax(max);

            if (i < array.Length - 1)
                finalMax = Max_Soft(array, i, finalMax);

            return finalMax;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float Max_Soft(float[] array, int offset, float initialMax) // for small chunks
        {
            float max = initialMax;
            for (; offset < array.Length; offset++)
            {
                var item = array[offset];
                if (item > max)
                    max = item;
            }
            return max;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float ReduceMax(Vector256<float> vector)
        {
            Vector128<float> hi128 = Avx.ExtractVector128(vector, 1);
            Vector128<float> lo128 = Avx.ExtractVector128(vector, 0);

            Vector128<float> hiTmp1 = Avx.Permute(hi128, 0x1b);
            Vector128<float> hiTmp2 = Avx.Permute(hi128, 0x4e);

            Vector128<float> loTmp1 = Avx.Permute(lo128, 0x1b);
            Vector128<float> loTmp2 = Avx.Permute(lo128, 0x4e);

            hi128 = Sse.Max(hi128, hiTmp1);
            hi128 = Sse.Max(hi128, hiTmp2);

            lo128 = Sse.Max(lo128, loTmp1);
            lo128 = Sse.Max(lo128, loTmp2);

            lo128 = Sse.Max(lo128, hi128);

            return Sse.ConvertToSingle(lo128);
        }
    }
}
