using System.Linq;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;

namespace IntrinsicsPlayground
{
    public class ArrayEqual
    {
        double[] _testArray1;
        double[] _testArray2;

        [GlobalSetup]
        public void GlobalSetup()
        {
            const int count = 1024 * 32;
            var range = Enumerable.Range(0, count);
            _testArray1 = range.Select(i => (double)i).ToArray();
            _testArray2 = _testArray1.ToArray(); // so both are equal
        }

        [Benchmark]
        public bool ArrayEqual_LINQ_SequenceEqual()
        {
            return Enumerable.SequenceEqual(_testArray1, _testArray2);
        }

        [Benchmark]
        public bool ArrayEqual_Simple()
        {
            return ArrayEqual_Simple(_testArray1, _testArray2);
        }

        [Benchmark(Baseline = true)]
        public bool ArrayEqual_AVX2()
        {
            return ArrayEqual_AVX2(_testArray1, _testArray2);
        }

        public static unsafe bool ArrayEqual_AVX2(double[] array1, double[] array2)
        {
            if (array1 == null || array2 == null)
                ThrowHelper.ArgumentNullException();

            if (array1.Length != array2.Length)
                return false;

            fixed (double* ptr1 = &array1[0])
            fixed (double* ptr2 = &array2[0])
            {
                for (int i = 0; i < array1.Length; i += 8) //16 for AVX512
                {
                    var vec1 = Avx.LoadVector256(ptr1 + i);
                    var vec2 = Avx.LoadVector256(ptr2 + i);
                    var ce = Avx.Compare(vec1, vec2, FloatComparisonMode.NotEqualOrderedNonSignaling);

                    if (!Avx.TestZ(ce, ce))
                        return false;
                }
            }

            return true;
        }


        public static bool ArrayEqual_Simple(double[] array1, double[] array2)
        {
            if (array1 == null || array2 == null)
                ThrowHelper.ArgumentNullException();

            if (array1.Length != array2.Length)
                return false;

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i]) //bounds check for array2, also compare using eps?
                    return false;
            }

            return true;
        }
    }
}
