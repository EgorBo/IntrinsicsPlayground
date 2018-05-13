using System;
using System.Linq;
using BenchmarkDotNet.Running;

namespace IntrinsicsPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
			//BenchmarkRunner.Run<ArrayMax>();
			//BenchmarkRunner.Run<ArrayIsSorted>();
			//BenchmarkRunner.Run<ArrayReverse>();
			//BenchmarkRunner.Run<ArraySum>();
			BenchmarkRunner.Run<ArrayEqual>();
        }
    }
}
