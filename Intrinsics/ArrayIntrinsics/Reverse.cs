using System;
using System.Runtime.Intrinsics.X86;

namespace IntrinsicsPlayground
{
    unsafe partial class ArrayIntrinsics
    {
        public static void Reverse_Sse2(int[] array) // also for float, double, byte, etc
        {
            if (array.Length < 2)
                return;

            int i = 0, len = array.Length;
            const int batchSize = 4; // 8/16 for AVX2/AVX512
            // but _mm256_permute4x64_pd is not exposed in the nuget package yet

            if (len < batchSize * 2)
            {
                Array.Reverse(array, 0, array.Length);
                return;
            }

            var chunks = len / batchSize / 2;
            fixed (int* ptr = &array[0])
            {
                for (; i < chunks * batchSize; i += batchSize)
                {
                    var leftPtr = ptr + i;
                    var rightPtr = ptr + len - batchSize - i;

                    // load first 4 values and the last ones
                    // shuffle vectors using _mm_shuffle_epi32 
                    // and write them back
                    var leftV = Sse2.LoadVector128(leftPtr);
                    var rightV = Sse2.LoadVector128(rightPtr);

                    Sse2.Store(rightPtr, Sse2.Shuffle(leftV, 0x1b)); //0x1b is _MM_SHUFFLE(0,1,2,3), it'd be nice to expose it to C# in S.R.I.E
                    Sse2.Store(leftPtr, Sse2.Shuffle(rightV, 0x1b));
                }
            }

            if (chunks * batchSize != len) //in case if there elements in the middle left unreversed
                Array.Reverse(array, chunks * batchSize, len - chunks * batchSize * 2);
        }
    }
}
