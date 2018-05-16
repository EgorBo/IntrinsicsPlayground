using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;

namespace IntrinsicsPlayground.Benchmarks
{
    public unsafe class FixedVsStackallocVsUnsafe
    {
        // what is the most efficient way to convert VectorXXX to Y[]?

        [Benchmark]
        public float[] FixedArray()
        {
            var result = new float[1024];
            for (int i = 0; i < result.Length; i++)
            {
                var vec = Avx.SetAllVector256(i / 3.0f);
                float[] array = new float[8];
                fixed (float* ptr = &array[0])
                    Avx.Store(ptr, vec);
                result[i] = array[0] + array[7];
            }
            return result;
        }

        [Benchmark]
        public float[] StackallocArray()
        {
            var result = new float[1024];
            for (int i = 0; i < result.Length; i++)
            {
                var vec = Avx.SetAllVector256(i / 3.0f);
                float* array = stackalloc float[8];
                Avx.Store(array, vec);
                result[i] = array[0] + array[7];
            }
            return result;
        }

        [Benchmark]
        public float[] UnsafeAsPointer()
        {
            var result = new float[1024];
            for (int i = 0; i < result.Length; i++)
            {
                var vec = Avx.SetAllVector256(i / 3.0f);
                float[] array = new float[8];
                Unsafe.Write(Unsafe.AsPointer(ref array[0]), vec);
                result[i] = array[0] + array[7];
            }
            return result;
        }
    }
}
