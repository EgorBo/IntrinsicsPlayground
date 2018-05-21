using System;
using System.Numerics;
using Xunit;

namespace IntrinsicsPlayground.Tests
{
    public class MatrixIntrinsicsTests
    {
        private static Matrix4x4 GenerateMatrix(int i) => new Matrix4x4(
            i + 0f,  i + 1f,  i + 2f,  i + 3f,
            i + 4f,  i + 5f,  i + 6f,  i + 7f,
            i + 8f,  i + 9f,  i + 10f, i + 11f,
            i + 12f, i + 13f, i + 14f, i + 15f);

        [Fact]
        public void MatrixIntrinsicsTests_Sum()
        {
            for (int i = 0; i < 1024; i++)
            {
                var m1 = GenerateMatrix(i);
                var m2 = GenerateMatrix(-i-1);

                var expected = m1 + m2;
                var actual = MatrixIntrinsics.Sum_Avx(m1, m2);

                Assert.True(actual.Equals(expected));
            }
        }

        [Fact]
        public void MatrixIntrinsicsTests_Negate()
        {
            for (int i = 0; i < 1024; i++)
            {
                var m = GenerateMatrix(i);

                var expected = Matrix4x4.Negate(m);
                var actual = MatrixIntrinsics.Negate_Avx(m);

                Assert.True(actual.Equals(expected));
            }
        }

        [Fact]
        public void MatrixIntrinsicsTests_Multiply()
        {
            for (int i = 0; i < 1024; i++)
            {
                var m1 = GenerateMatrix(i);

                var expected = m1 * (i + 1);
                var actual = MatrixIntrinsics.Multiply_Avx(m1, i + 1);

                Assert.True(actual.Equals(expected));
            }
        }
    }
}
