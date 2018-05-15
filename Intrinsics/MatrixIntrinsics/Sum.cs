using System.Numerics;
using System.Runtime.Intrinsics.X86;

namespace IntrinsicsPlayground
{
    public static unsafe partial class MatrixIntrinsics
    {
        public static Matrix4x4 Sum_Avx(Matrix4x4 left, Matrix4x4 right)
        {
            float* leftPtr = &left.M11;
            var leftRow1 = Avx.LoadVector256(leftPtr + 0);
            var leftRow3 = Avx.LoadVector256(leftPtr + 8);

            float* rightPtr = &right.M11;
            var rightRow1 = Avx.LoadVector256(rightPtr + 0);
            var rightRow3 = Avx.LoadVector256(rightPtr + 8);

            Avx.Store(leftPtr + 0, Avx.Add(leftRow1, rightRow1));
            Avx.Store(leftPtr + 8, Avx.Add(leftRow3, rightRow3));

            return left;
        }
    }
}
