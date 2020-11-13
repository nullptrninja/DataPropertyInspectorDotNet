using BenchmarkDotNet.Running;

namespace DataInspector.PerformanceBenchmarking {
    class Program {
        static void Main(string[] args) {
            //BenchmarkRunner.Run<SingleCallDALTests>();
            //BenchmarkRunner.Run<LargeBatchedDALTests>();
            BenchmarkRunner.Run<LargeBatchedRulesEngineTests>();
        }
    }
}
