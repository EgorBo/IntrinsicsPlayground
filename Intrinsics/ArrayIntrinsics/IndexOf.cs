using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace IntrinsicsPlayground
{
    unsafe partial class ArrayIntrinsics
    {        
        public static int IndexOf_Avx2(int[] array, int element)
        {
            if (array.Length < 1)
                return -1;
            
            if (array.Length == 1)
                return array[0];

            int i = 0;
            fixed (int* ptr = &array[0])
            {
                if (array.Length > 8 * 2)
                {
                    var elementVec = Avx.SetAllVector256(element);
                    for (; i < array.Length - 8; i += 8) //16 for AVX512
                    {
                        var curr = Avx.LoadVector256(ptr + i);
                        var mask = Avx2.CompareEqual(curr, elementVec);
                        if (!Avx.TestZ(mask, mask))
                        {
                            return FindIndex_Soft(array, i, element);
                        }
                    }
                }

            }
            return FindIndex_Soft(array, i, element);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int FindIndex_Soft(int[] array, int offset, int value)
        {
            for (int i = offset; i < array.Length; i++)
                if (array[i] == value)
                    return i;
            return -1;
        }
    }
}
