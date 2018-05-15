using System;
using System.Linq;
using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace IntrinsicsPlayground
{
    public class MatrixSum : MatrixBenchmarkBase
    {
        [Benchmark]
        public Matrix4x4 MatrixSum_BCL()
        {
            var sum = Matrix4x4.Identity;
            for (int i = 0; i < 1024; i++)
            {
                Matrix4x4 m = GenerateMatrix(i);
                sum = m + sum;
            }

            return sum;
        }

        [Benchmark(Baseline = true)]
        public Matrix4x4 MatrixSum_Avx()
        {
            var sum = Matrix4x4.Identity;
            for (int i = 0; i < 1024; i++)
            {
                Matrix4x4 m = GenerateMatrix(i);
                sum = MatrixIntrinsics.Sum_Avx(m, sum);
            }

            return sum;
        }
    }
}
