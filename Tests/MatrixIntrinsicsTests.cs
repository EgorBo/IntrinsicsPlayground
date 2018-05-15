using System;
using System.Linq;
using System.Numerics;
using Xunit;

namespace IntrinsicsPlayground.Tests
{
    public class MatrixIntrinsicsTests
    {
        [Fact]
        public unsafe void MatrixIntrinsicsTests_Sum()
        {
            var arrayOfFloats = Enumerable.Range(0, 1500).Select(n => n / 2.0f).ToArray();
            for (int i = 32; i < 1024; i++)
            {
                Matrix4x4 m1, m2;
                fixed (float* ptr = &arrayOfFloats[0])
                {
                    m1 = *(Matrix4x4*)(void*)(ptr + 0);
                    m2 = *(Matrix4x4*)(void*)(ptr + i);

                    var expected = m1 + m2;
                    var actual = MatrixIntrinsics.Sum_Avx(m1, m2);

                    Assert.True(actual.Equals(expected));
                }
            }
        }
    }
}
