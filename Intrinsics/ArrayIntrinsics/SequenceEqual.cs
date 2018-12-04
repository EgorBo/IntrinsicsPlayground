using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
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

                return SequenceEqual_Soft(ptr1, ptr2, i, array1.Length);
            }
        }

        public static bool SequenceEqual_Sse(float[] array1, float[] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            if (array1.Length == 0)
                return true;//SequenceEqual_Soft(array1, array2, 0);

            int i = 0;
            fixed (float* ptr1 = &array1[0])
            fixed (float* ptr2 = &array2[0])
            {
                if (array1.Length < 4)
                    return SequenceEqual_Soft(ptr1, ptr2, 0, array1.Length);

                for (; i <= array1.Length - 4; i += 4)
                {
                    Vector128<float> vec1 = Sse.LoadVector128(ptr1 + i);
                    Vector128<float> vec2 = Sse.LoadVector128(ptr2 + i);
                    var ce = Sse.MoveMask(Sse.CompareNotEqual(vec1, vec2)) == 0;
                    if (!ce)
                        return false;
                }

                return SequenceEqual_Soft(ptr1, ptr2, i, array1.Length);
            }
        }

        public static bool SequenceEqual_Sse_aligned(float[] array1, float[] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            if (array1.Length == 0)
                return true;//SequenceEqual_Soft(array1, array2, 0);

            int i = 0;
            fixed (float* ptr1 = &array1[0])
            fixed (float* ptr2 = &array2[0])
            {
                var aligned1 = (float*)(((ulong)ptr1 + 31UL) & ~31UL);
                var aligned2 = (float*)(((ulong)ptr2 + 31UL) & ~31UL);
                var pos1 = (int)(aligned1 - ptr1);
                var pos2 = (int)(aligned2 - ptr2);
                var pos = Math.Max(pos1, pos2);

                if (array1.Length < 4)
                    return SequenceEqual_Soft(ptr1, ptr2, 0, array1.Length);

                if (pos > 0)
                {
                    SequenceEqual_Soft(ptr1, ptr2, 0, pos);
                    i = pos;
                }

                for (; i <= array1.Length - 4; i += 4)
                {
                    Vector128<float> vec1 = Sse.LoadVector128(ptr1 + i);
                    Vector128<float> vec2 = Sse.LoadVector128(ptr2 + i);
                    var ce = Sse.MoveMask(Sse.CompareNotEqual(vec1, vec2)) == 0;
                    if (!ce)
                        return false;
                }

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
