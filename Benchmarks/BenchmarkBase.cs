using System.Linq;
using System.Numerics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Exporters;

namespace IntrinsicsPlayground
{
    [MemoryDiagnoser]
    [MarkdownExporter]
    public class ArrayBenchmarkBase
    {
        public int[] ArrayOfInts { get; set; }

        public float[] ArrayOfFloats { get; set; }

        public float[] ArrayOfFloats2 { get; set; }

        public double[] ArrayOfDoubles { get; set; }


        [GlobalSetup]
        public void GlobalSetup()
        {
            ArrayOfInts = Enumerable.Range(0, TestArrayLength).ToArray();
            ArrayOfDoubles = Enumerable.Range(0, TestArrayLength).Select(i => (double)i).ToArray();
            ArrayOfFloats = Enumerable.Range(0, TestArrayLength).Select(i => (float)i).ToArray();

            ArrayOfFloats2 = ArrayOfFloats.ToArray();
            ArrayOfFloats2[ArrayOfFloats.Length - 1] = -1; // so it's _almost_ equal to ArrayOfDoubles
        }

        [Params(10, 121, 32 * 1024)]
        public int TestArrayLength { get; set; }
    }

    [MemoryDiagnoser]
    public class MatrixBenchmarkBase
    {
        public static Matrix4x4 GenerateMatrix(int i) => new Matrix4x4(
            i + 0f, i + 1f, i + 2f, i + 3f,
            i + 4f, i + 5f, i + 6f, i + 7f,
            i + 8f, i + 9f, i + 10f, i + 11f,
            i + 12f, i + 13f, i + 14f, i + 15f);

        public static unsafe Matrix4x4 FromArray(float[] array, int offset)
        {
            fixed (float* ptr = &array[offset])
                return *(Matrix4x4*)(void*)ptr;
        }
    }
}
