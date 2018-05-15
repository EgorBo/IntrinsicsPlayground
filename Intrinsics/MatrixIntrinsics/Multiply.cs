using System;
using System.Numerics;
using System.Runtime.Intrinsics.X86;

namespace IntrinsicsPlayground
{
    public static unsafe partial class MatrixIntrinsics
    {
        public static Matrix4x4 Sum_Multiply(Matrix4x4 left, float right)
        {
            float* leftPtr = &left.M11;
            var leftRow1 = Avx.LoadVector256(leftPtr + 0);
            var leftRow3 = Avx.LoadVector256(leftPtr + 8);

            var rightVec = Avx.SetAllVector256(right);

            Avx.Store(leftPtr + 0, Avx.Multiply(leftRow1, rightVec));
            Avx.Store(leftPtr + 8, Avx.Multiply(leftRow3, rightVec));

            return left;
        }

        public static Matrix4x4 Sum_Multiply(Matrix4x4 left, Matrix4x4 right)
        {
            throw new NotImplementedException();
        }
    }
}
