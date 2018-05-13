using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;

namespace IntrinsicsPlayground
{
	public class ArrayEqual

	{
		int[] testArray1;
		int[] testArray2;

		[GlobalSetup]
		public void GlobalSetup()
		{
			const int count = 1024 * 32;
			var range = Enumerable.Range(0, count);
			testArray1 = range.ToArray();
			testArray2 = range.ToArray();
			//testArray2[testArray2.Length - 1] = 1;
		}

		[Benchmark]
		public bool ArrayEqual_LINQ_SequenceEqual()
		{
			return Enumerable.SequenceEqual(testArray1, testArray2);
		}

		[Benchmark]
		public bool ArrayEqual_Simple()
		{
			return ArrayEqual_Simple(testArray1, testArray2);
		}

		[Benchmark(Baseline = true)]
		public bool ArrayEqual_AVX2()
		{
			return ArrayEqual_AVX2(testArray1, testArray2);
		}

		public static unsafe bool ArrayEqual_AVX2(int[] array1, int[] array2)
		{
			if (array1 == null || array2 == null)
				throw new ArgumentNullException();

			if (array1.Length != array2.Length)
				return false;

			fixed (int* ptr1 = &array1[0])
			fixed (int* ptr2 = &array2[0])
			{
				for (int i = 0; i < array1.Length; i += 8) //16 for AVX512
				{
					var vec1 = Avx.LoadVector256(ptr1 + i);
					var vec2 = Avx.LoadVector256(ptr2 + i);
					var ce = Avx2.CompareEqual(vec1, vec2);
					if (!Avx.TestZ(ce, ce))
					{
						return false;
					}
				}
			}

			return true;
		}


		public static unsafe bool ArrayEqual_Simple(int[] array1, int[] array2)
		{
			if (array1 == null || array2 == null)
				ThrowHelper.ArgumentNullException();

			if (array1.Length != array2.Length)
				return false;

			for (int i = 0; i < array1.Length; i++)
			{
				if (array1[i] != array2[i]) //bounds check for array2
					return false;
			}

			return true;
		}
	}
}
