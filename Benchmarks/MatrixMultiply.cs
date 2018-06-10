using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace IntrinsicsPlayground
{
    public class MatrixMultiply : MatrixBenchmarkBase
    {
        [Benchmark]
        public Matrix4x4[] MatrixMultiply_ByScalar_BCL()
        {
            var result = new Matrix4x4[1024];
            for (int i = 0; i < result.Length; i++)
            {
                Matrix4x4 m = GenerateMatrix(i + 1);
                result[i] = Matrix4x4.Multiply(m, i / 2f);
            }
            return result;
        }

        [Benchmark(Baseline = true)]
        public Matrix4x4[] MatrixMultiply_ByScalar_Avx()
        {
            var result = new Matrix4x4[1024];
            for (int i = 0; i < result.Length; i++)
            {
                Matrix4x4 m = GenerateMatrix(i + 1);
                result[i] = MatrixIntrinsics.Multiply_Avx(m, i / 2f);
            }
            return result;
        }
    }
}
