using System;
using System.Linq;
using IntrinsicsPlayground.Misc;
using Xunit;

namespace IntrinsicsPlayground.Tests
{
    public class SortTests
    {
        [Fact]
        public void DualPivotQuicksortTest()
        {
            var rand = new Random();
            var array1 = Enumerable.Range(0, 1024).OrderBy(i => rand.Next()).ToArray();
            var array2 = array1.ToArray();

            Array.Sort(array1);
            DualPivotQuicksort.Sort(array2);

            Assert.True(array1.SequenceEqual(array2));
        }
    }
}
