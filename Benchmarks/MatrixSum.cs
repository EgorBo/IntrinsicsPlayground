using System;
using System.Linq;
using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace IntrinsicsPlayground
{
    [MemoryDiagnoser]
    public class MatrixSum
    {
        public float[] ArrayOfFloats { get; set; }

        [GlobalSetup]
        public void GlobalSetup() => ArrayOfFloats = Enumerable.Range(0, 1024).Select(i => i / 2.0f).ToArray();

        [Benchmark]
        public Matrix4x4 MatrixSum_BCL()
        {
            var sum = Matrix4x4.Identity;
            for (int i = 0; i < ArrayOfFloats.Length - 16; i++)
            {
                Matrix4x4 m = FromArray(ArrayOfFloats, i);
                sum = m + sum;
            }

            return sum;
        }

        [Benchmark(Baseline = true)]
        public Matrix4x4 MatrixSum_Avx()
        {
            var sum = Matrix4x4.Identity;
            for (int i = 0; i < ArrayOfFloats.Length - 16; i++)
            {
                Matrix4x4 m = FromArray(ArrayOfFloats, i);
                sum = MatrixIntrinsics.Sum_Avx(m, sum);
            }

            return sum;
        }

        private static unsafe Matrix4x4 FromArray(float[] array, int offset)
        {
            fixed (float* ptr = &array[offset])
                return *(Matrix4x4*)(void*) ptr;
        }
    }
}
