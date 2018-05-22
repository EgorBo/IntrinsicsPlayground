using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using IntrinsicsPlayground.Misc;

namespace IntrinsicsPlayground.Benchmarks.Misc
{
    //[MemoryDiagnoser]
    public class SortBenchmarks
    {
        private int[] _alreadySortedAsc,
            _alreadySortedDesc,
            _nearlySortedAsc,
            _fullRandom1,
            _fullRandom2;

        [Params(100, 100000)]
        public int Count { get; set; }


        [GlobalSetup]
        public void GlobalSetup()
        {
            var rand = new Random();
            var range = Enumerable.Range(0, Count).ToArray();

            _alreadySortedAsc = range.ToArray();
            _alreadySortedDesc = range.Reverse().ToArray();

            _nearlySortedAsc = _alreadySortedAsc.ToArray();
            for (int i = 0; i < _nearlySortedAsc.Length - 1; i += rand.Next(5, 15))
                _nearlySortedAsc[i] = 0;

            _fullRandom1 = range.OrderBy(i => rand.Next()).ToArray();
            _fullRandom2 = range.OrderBy(i => rand.Next()).ToArray();
        }


        [Benchmark]
        public int[] AlreadySortedAsc_Java()
        {
            var array = _alreadySortedAsc.ToArray();
            DualPivotQuicksort.Sort(array);
            return array;
        }

        [Benchmark]
        public int[] AlreadySortedAsc__NET()
        {
            var array = _alreadySortedAsc.ToArray();
            Array.Sort(array);
            return array;
        }


        [Benchmark]
        public int[] AlreadySortedButReversed_Java()
        {
            var array = _alreadySortedDesc.ToArray();
            DualPivotQuicksort.Sort(array);
            return array;
        }

        [Benchmark]
        public int[] AlreadySortedButReversed__NET()
        {
            var array = _alreadySortedDesc.ToArray();
            Array.Sort(array);
            return array;
        }


        [Benchmark]
        public int[] NearlySortedAsc_Java()
        {
            var array = _nearlySortedAsc.ToArray();
            DualPivotQuicksort.Sort(array);
            return array;
        }

        [Benchmark]
        public int[] NearlySortedAsc__NET()
        {
            var array = _nearlySortedAsc.ToArray();
            Array.Sort(array);
            return array;
        }


        [Benchmark]
        public int[] FullRandom1_Java()
        {
            var array = _fullRandom1.ToArray();
            DualPivotQuicksort.Sort(array);
            return array;
        }

        [Benchmark]
        public int[] FullRandom1__NET()
        {
            var array = _fullRandom1.ToArray();
            Array.Sort(array);
            return array;
        }


        [Benchmark]
        public int[] FullRandom2_Java()
        {
            var array = _fullRandom2.ToArray();
            DualPivotQuicksort.Sort(array);
            return array;
        }

        [Benchmark]
        public int[] FullRandom2__NET()
        {
            var array = _fullRandom2.ToArray();
            Array.Sort(array);
            return array;
        }
    }
}
