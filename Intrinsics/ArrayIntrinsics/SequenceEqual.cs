using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace IntrinsicsPlayground
{
    public static unsafe partial class ArrayIntrinsics
    {
        public static bool SequenceEqual_Avx(float[] array1, float[] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            if (array1.Length == 0)
                return true;//SequenceEqual_Soft(array1, array2, 0);

            int i = 0;
            fixed (float* ptr1 = &array1[0])
            fixed (float* ptr2 = &array2[0])
            {
                if (array1.Length < 8)
                    return SequenceEqual_Soft(ptr1, ptr2, 0, array1.Length);

                for (; i <= array1.Length - 8; i += 8) //16 for AVX512
                {
                    var vec1 = Avx.LoadVector256(ptr1 + i);
                    var vec2 = Avx.LoadVector256(ptr2 + i);
                    var ce = Avx.Compare(vec1, vec2, FloatComparisonMode.NotEqualOrderedNonSignaling);

                    if (!Avx.TestZ(ce, ce))
                        return false;
                }

                // TODO: shift the last `ce` vector to ignore garbage values
                // so we won't have to call SequenceEqual_Soft (in case if array1.Length % 8 != 0)

                return SequenceEqual_Soft(ptr1, ptr2, i, array1.Length);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool SequenceEqual_Soft(float* array1, float* array2, int offset, int count)
        {
            for (; offset < count; offset++)
                if (*(array1 + offset) != *(array2 + offset))
                    return false;
            return true;
        }
    }
}
