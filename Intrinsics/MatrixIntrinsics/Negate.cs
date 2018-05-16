using System.Numerics;
using System.Runtime.Intrinsics.X86;

namespace IntrinsicsPlayground
{
    public static unsafe partial class MatrixIntrinsics
    {
        public static Matrix4x4 Negate_Avx(Matrix4x4 matrix)
        {
            float* matrixPtr = &matrix.M11;
            var row12 = Avx.LoadVector256(matrixPtr + 0);
            var row34 = Avx.LoadVector256(matrixPtr + 8);

            var zeroVec = Avx.SetZeroVector256<float>();
            Avx.Store(matrixPtr + 0, Avx.Subtract(zeroVec, row12)); // or _mm_xor_ps -0.0 ?
            Avx.Store(matrixPtr + 8, Avx.Subtract(zeroVec, row34));

            return matrix;
        }
    }
}
