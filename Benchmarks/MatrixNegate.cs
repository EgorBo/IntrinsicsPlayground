using System;
using System.Linq;
using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace IntrinsicsPlayground
{
    public class MatrixNegate : MatrixBenchmarkBase
    {
        [Benchmark]
        public Matrix4x4[] MatrixNegate_BCL()
        {
            var result = new Matrix4x4[1024];
            for (int i = 0; i < result.Length; i++)
            {
                Matrix4x4 m = GenerateMatrix(i);
                result[i] = Matrix4x4.Negate(m);
            }
            return result;
        }

        [Benchmark(Baseline = true)]
        public Matrix4x4[] MatrixNegate_Avx()
        {
            var result = new Matrix4x4[1024];
            for (int i = 0; i < result.Length; i++)
            {
                Matrix4x4 m = GenerateMatrix(i);
                result[i] = MatrixIntrinsics.Negate_Avx(m);
            }
            return result;
        }
    }
}
