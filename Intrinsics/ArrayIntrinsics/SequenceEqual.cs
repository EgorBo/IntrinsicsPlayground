using System.Linq;
using System.Runtime.Intrinsics.X86;

namespace IntrinsicsPlayground
{
    public static partial class ArrayIntrinsics
    {
        public static unsafe bool SequenceEqual_Avx(double[] array1, double[] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            if (array1.Length < 1) //both are empty
                return true;

            fixed (double* ptr1 = &array1[0])
            fixed (double* ptr2 = &array2[0])
            {
                for (int i = 0; i < array1.Length; i += 8) //16 for AVX512
                {
                    var vec1 = Avx.LoadVector256(ptr1 + i);
                    var vec2 = Avx.LoadVector256(ptr2 + i);
                    var ce = Avx.Compare(vec1, vec2, FloatComparisonMode.NotEqualOrderedNonSignaling);

                    if (!Avx.TestZ(ce, ce))
                        return false;
                }
            }

            return true;
        }
    }
}
