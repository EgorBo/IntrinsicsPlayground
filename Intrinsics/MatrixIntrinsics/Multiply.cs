using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace IntrinsicsPlayground
{
    public static unsafe partial class MatrixIntrinsics
    {
        public static Matrix4x4 Multiply_Avx(Matrix4x4 left, float right)
        {
            float* leftPtr = &left.M11;
            var leftRow1 = Avx.LoadVector256(leftPtr + 0);
            var leftRow3 = Avx.LoadVector256(leftPtr + 8);

            var rightVec = Avx.SetAllVector256(right);

            Avx.Store(leftPtr + 0, Avx.Multiply(leftRow1, rightVec));
            Avx.Store(leftPtr + 8, Avx.Multiply(leftRow3, rightVec));

            return left;
        }

        // TODO: not efficient yet
        public static Matrix4x4 Multiply_Avx(Matrix4x4 left, Matrix4x4 right)
        {
            float* leftPtr = &left.M11;
            var leftRow1 = Sse.LoadVector128(leftPtr + 0);
            var leftRow2 = Sse.LoadVector128(leftPtr + 4);
            var leftRow3 = Sse.LoadVector128(leftPtr + 8);
            var leftRow4 = Sse.LoadVector128(leftPtr + 12);

            // masked load?
            var rightColumn1 = Sse.SetVector128(right.M41, right.M31, right.M21, right.M11);
            var rightColumn2 = Sse.SetVector128(right.M42, right.M32, right.M22, right.M12);
            var rightColumn3 = Sse.SetVector128(right.M43, right.M33, right.M23, right.M13);
            var rightColumn4 = Sse.SetVector128(right.M44, right.M34, right.M24, right.M14);

            float* buf = stackalloc float[4];
            return new Matrix4x4(
                HorizontalSum(Sse.Multiply(leftRow1, rightColumn1), buf),
                HorizontalSum(Sse.Multiply(leftRow1, rightColumn2), buf),
                HorizontalSum(Sse.Multiply(leftRow1, rightColumn3), buf),
                HorizontalSum(Sse.Multiply(leftRow1, rightColumn4), buf),

                HorizontalSum(Sse.Multiply(leftRow2, rightColumn1), buf),
                HorizontalSum(Sse.Multiply(leftRow2, rightColumn2), buf),
                HorizontalSum(Sse.Multiply(leftRow2, rightColumn3), buf),
                HorizontalSum(Sse.Multiply(leftRow2, rightColumn4), buf),

                HorizontalSum(Sse.Multiply(leftRow3, rightColumn1), buf),
                HorizontalSum(Sse.Multiply(leftRow3, rightColumn2), buf),
                HorizontalSum(Sse.Multiply(leftRow3, rightColumn3), buf),
                HorizontalSum(Sse.Multiply(leftRow3, rightColumn4), buf),

                HorizontalSum(Sse.Multiply(leftRow4, rightColumn1), buf),
                HorizontalSum(Sse.Multiply(leftRow4, rightColumn2), buf),
                HorizontalSum(Sse.Multiply(leftRow4, rightColumn3), buf),
                HorizontalSum(Sse.Multiply(leftRow4, rightColumn4), buf));
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float HorizontalSum(Vector128<float> vec, float* buf)
        {
            vec = Sse3.HorizontalAdd(vec, vec);
            vec = Sse3.HorizontalAdd(vec, vec);
            Sse.Store(buf, vec);
            return *buf;
        }
    }
}
