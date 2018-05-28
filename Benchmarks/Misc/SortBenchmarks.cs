using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Exporters;

namespace IntrinsicsPlayground.Benchmarks.Misc
{
    public class SortingAlreadySortedArray : SortBenchmarkBase
    {
        protected override int[] CurrentArray => _alreadySortedAsc;
    }

    public class SortingAlreadySortedButReversedArray : SortBenchmarkBase
    {
        protected override int[] CurrentArray => _alreadySortedDesc;
    }

    public class SortingRandomArray : SortBenchmarkBase
    {
        protected override int[] CurrentArray => _fullRandom;
    }
    public class SortingNearlySortedArray : SortBenchmarkBase
    {
        protected override int[] CurrentArray => _nearlySortedAsc;
    }

    [MarkdownExporterAttribute.GitHub]
    [RPlotExporter]
    public class SortBenchmarkBase
    {
        protected int[] _alreadySortedAsc,
            _alreadySortedDesc,
            _nearlySortedAsc,
            _fullRandom;

        // random array will be inited only once 
        protected static int[] _staticRandomArray;

        static SortBenchmarkBase()
        {
            var rand = new Random();
            var range = Enumerable.Range(0, 10000).ToArray();
            _staticRandomArray = range.OrderBy(i => rand.Next()).ToArray();
        }

        protected virtual int[] CurrentArray { get; }

        [Params(10, 100, 1000, 10000)] public int Count { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            var range = Enumerable.Range(0, Count).ToArray();

            _alreadySortedAsc = range.ToArray();
            _alreadySortedDesc = range.Reverse().ToArray();

            _nearlySortedAsc = _alreadySortedAsc.ToArray();
            for (int i = 0; i < _nearlySortedAsc.Length - 1; i += 7)
                _nearlySortedAsc[i] = 0;

            _fullRandom = _staticRandomArray.Take(Count).ToArray();
        }

        [Benchmark]
        public int[] ArraySort()
        {
            var array = CurrentArray.ToArray();
            Array.Sort(array);
            return array;
        }

        [Benchmark]
        public int[] DualPivotQuicksort()
        {
            var array = CurrentArray.ToArray();
            IntrinsicsPlayground.Misc.Sorting.DualPivotQuicksort.Sort(array);
            return array;
        }

        [Benchmark]
        public int[] RadixSort()
        {
            var array = CurrentArray.ToArray();
            IntrinsicsPlayground.Misc.Sorting.RadixSort.Sort(array);
            return array;
        }


        //[Benchmark]
        //public int[] ClassicQuickSort()
        //{
        //    var array = CurrentArray.ToArray();
        //    IntrinsicsPlayground.Misc.Sorting.ClassicQuickSort.Sort(array);
        //    return array;
        //}

        //[Benchmark]
        //public int[] HeapSort()
        //{
        //    var array = CurrentArray.ToArray();
        //    IntrinsicsPlayground.Misc.Sorting.HeapSort.Sort(array);
        //    return array;
        //}

        //[Benchmark]
        //public int[] InsertionSort()
        //{
        //    var array = CurrentArray.ToArray();
        //    IntrinsicsPlayground.Misc.Sorting.InsertionSort.Sort(array);
        //    return array;
        //}

        //[Benchmark]
        //public int[] MergeSort()
        //{
        //    var array = CurrentArray.ToArray();
        //    IntrinsicsPlayground.Misc.Sorting.MergeSort.Sort(array);
        //    return array;
        //}

        //[Benchmark]
        //public int[] LinqOrderBy()
        //{
        //    return CurrentArray.OrderBy(i => i).ToArray();
        //}

        //[Benchmark]
        //public int[] BubbleSort()
        //{
        //    var array = CurrentArray.ToArray();
        //    IntrinsicsPlayground.Misc.Sorting.BubbleSort.Sort(array);
        //    return array;
        //}
    }
}
