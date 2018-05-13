using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace IntrinsicsPlayground
{
    public class ArrayIsSorted
    {
        static unsafe bool IsSorted_Sse41(int[] array)
        {
            if (array.Length < 2)
                return true;

            if (!Sse41.IsSupported) //no SSE41 support
                return IsSorted_Soft(array);

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

    }
}
