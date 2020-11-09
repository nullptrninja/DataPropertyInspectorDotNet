using BenchmarkDotNet.Running;

namespace DataInspector.PerformanceBenchmarking {
    class Program {
        static void Main(string[] args) {
            //BenchmarkRunner.Run<DirectInvokeDALTests>();
            BenchmarkRunner.Run<LargeBatchedDirectInvokeDALTests>();
        }
    }
}
