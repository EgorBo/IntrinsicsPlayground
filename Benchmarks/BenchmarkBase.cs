using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace IntrinsicsPlayground
{
    //[MemoryDiagnoser]
    public class BenchmarkBase
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
}
