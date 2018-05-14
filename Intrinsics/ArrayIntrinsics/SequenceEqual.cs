using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace IntrinsicsPlayground
{
    public static partial class ArrayIntrinsics
    {
        public static unsafe bool SequenceEqual_Avx(double[] array1, double[] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            if (array1.Length < 8)
                return SequenceEqual_Soft(array1, array2, 0);

            int i = 0;
            fixed (double* ptr1 = &array1[0])
            fixed (double* ptr2 = &array2[0])
            {
                for (; i <= array1.Length - 8; i += 8) //16 for AVX512
                {
                    var vec1 = Avx.LoadVector256(ptr1 + i);
                    var vec2 = Avx.LoadVector256(ptr2 + i);
                    var ce = Avx.Compare(vec1, vec2, FloatComparisonMode.NotEqualOrderedNonSignaling);

                    if (!Avx.TestZ(ce, ce))
                        return false;
                }
            }

            return SequenceEqual_Soft(array1, array2, i);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool SequenceEqual_Soft(double[] array1, double[] array2, int offset)
        {
            for (; offset < array1.Length; offset++)
                if (array1[offset] != array2[offset])
                    return false;
            return true;
        }
    }
}
