using System;
using System.Linq;
using IntrinsicsPlayground.Misc.Sorting;
using Xunit;

namespace IntrinsicsPlayground.Tests
{
    public class SortTests
    {
        [Fact]
        public void AllSortingAlgorithms()
        {
            var bubbleSort = typeof(BubbleSort);
            var rand = new Random();
            var unsortedArray = Enumerable.Range(0, 1024).OrderBy(i => rand.Next()).ToArray();
            var sortedArrayRef = unsortedArray.ToArray();
            Array.Sort(sortedArrayRef);

            foreach (var sortType in bubbleSort.Assembly.GetTypes().Where(t => t.Namespace == bubbleSort.Namespace))
            {
                var array = unsortedArray.ToArray();
                var method = sortType.GetMethod("Sort", new[] {typeof(int[])});
                method.Invoke(null, new object[]{ array });

                Assert.True(array.SequenceEqual(sortedArrayRef));
            }
        }
    }
}
