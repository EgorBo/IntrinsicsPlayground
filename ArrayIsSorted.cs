using System.Linq;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;

namespace IntrinsicsPlayground
{
	public class ArrayIsSorted
	{
		int[] testArray;

		[GlobalSetup]
		public void GlobalSetup()
		{
			const int count = 1024 * 32;
			testArray = Enumerable.Range(0, count).ToArray();
		}

		[Benchmark(Baseline = true)]
		public bool IsSorted_Simple()
		{
			return IsSorted_Simple(testArray);
		}

		[Benchmark]
		public bool IsSorted_Simple2()
		{
			return IsSorted_Simple2(testArray);
		}

		[Benchmark]
		public bool IsSorted_Sse41()
		{
			return IsSorted_Sse41(testArray);
		}

		[Benchmark]
		public bool IsSorted_AVX2()
		{
			return IsSorted_AVX2(testArray);
		}

		public static bool IsSorted_Simple(int[] array)
		{
			if (array.Length < 2)
				return true;

			for (int i = 0; i < array.Length - 1; i++)
			{
				if (array[i] > array[i + 1]) 
					return false;
			}
			return true;
		}

		public static bool IsSorted_Simple2(int[] array)
		{
			if (array.Length < 2)
				return true;

			// optimized by @consoleapp https://twitter.com/consoleapp/status/991380745234067458
			var current = array[0];
			for (int i = 1; i < array.Length; i++)
			{
				var next = array[i];
				if (current > next)
					return false;
				current = next;
			}
			return true;
		}

		public static unsafe bool IsSorted_Sse41(int[] array)
		{
			if (array.Length < 2)
				return true;

			if (!Sse41.IsSupported) //no SSE41 support
				return IsSorted_Simple2(array);

			int i = 0;
			fixed (int* ptr = &array[0])
			{
				if (array.Length > 4)
				{
					for (; i < array.Length - 4; i += 4) // 8 for AVX2 and 16 for AVX512
					{
						Vector128<int> curr = Sse2.LoadVector128(ptr + i);
						Vector128<int> next = Sse2.LoadVector128(ptr + i + 1);
						Vector128<int> mask = Sse2.CompareGreaterThan(curr, next);
						if (!Sse41.TestAllZeros(mask, mask))
							return false;
					}

				}
			}

			for (; i < array.Length - 1; i++)
			{
				if (array[i] > array[i + 1])
					return false;
			}
			return true;
		}

		public static unsafe bool IsSorted_AVX2(int[] array)
		{
			if (array.Length < 2)
				return true;

			if (!Avx2.IsSupported)
				return IsSorted_Simple2(array);

			fixed (int* ptr = &array[0])
			{
				int i = 0;
				if (array.Length > 8)
				{
					for (; i < array.Length - 8; i += 8) //16 for AVX512
					{
						var curr = Avx.LoadVector256(ptr + i);
						var next = Avx.LoadVector256(ptr + i + 1);
						var mask = Avx2.CompareGreaterThan(curr, next);
						if (!Avx.TestZ(mask, mask))
							return false;
					}
				}

				for (; i + 1 < array.Length; i++)
				{
					if (array[i] > array[i + 1])
						return false;
				}
			}
			return true;
		}
    }
}
