using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Sample.Domain;
using Sample.Domain.DAL;

namespace DataInspector.PerformanceBenchmarking {
    [SimpleJob(runStrategy: RunStrategy.Throughput, launchCount: 1, warmupCount: 1, invocationCount: 1000000)]
    [MemoryDiagnoser]
    public class SingleCallWithVerificationTests {
        private readonly List<Tuple<string, object>> DirectCallInputs = new List<Tuple<string, object>> {
            new Tuple<string, object>("DerpChild.Id", 1000),
            new Tuple<string, object>("DerpChild.Uuid", "some uuid"),
            new Tuple<string, object>("DerpChild.OtherGlorp.Name", "derp_refGlorp"),
            new Tuple<string, object>("DerpChild.OtherGlorp.IsEnabled", true),
            new Tuple<string, object>("DerpChild.OtherGlorp.ValueF", 3.14159f),
            new Tuple<string, object>("GlorpChild.Name", "derp_baseGlorp"),
            new Tuple<string, object>("GlorpChild.IsEnabled", false),
            new Tuple<string, object>("GlorpChild.ValueF", 1.1f),
            new Tuple<string, object>("RootNodeId", "root_100"),
        };

        private readonly List<Tuple<string, object>> ArrayCallInputs = new List<Tuple<string, object>> {
            new Tuple<string, object>("DerpChild.AllTheFlerbs[0].FlerbId", "flerb_100"),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[1].FlerbId", "flerb_101"),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[2].FlerbId", "flerb_102"),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[3].FlerbId", "flerb_103"),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[4].FlerbId", "flerb_104"),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[5].FlerbId", "flerb_105"),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[6].FlerbId", "flerb_106"),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[7].FlerbId", "flerb_107"),

            new Tuple<string, object>("DerpChild.AllTheFlerbs[0].SubIds[0]", 1),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[1].SubIds[0]", 1),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[2].SubIds[0]", 1),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[3].SubIds[0]", 1),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[4].SubIds[0]", 1),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[5].SubIds[0]", 1),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[6].SubIds[0]", 1),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[7].SubIds[0]", 1),

            new Tuple<string, object>("DerpChild.AllTheFlerbs[0].SubIds[3]", 4),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[1].SubIds[3]", 4),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[2].SubIds[3]", 4),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[3].SubIds[3]", 4),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[4].SubIds[3]", 4),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[5].SubIds[3]", 4),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[6].SubIds[3]", 4),
            new Tuple<string, object>("DerpChild.AllTheFlerbs[7].SubIds[3]", 4),
        };

        private Random mRand;
        private Sample_Domain_Root_DataAccessLayer mDalUnderTest;
        private Root mTestModel;

        public Tuple<string, object>[] MixedDataSet => DirectCallInputs.Concat(ArrayCallInputs)
                                                                       .Shuffle()
                                                                       .ToArray();

        [GlobalSetup]
        public void Setup() {
            mDalUnderTest = new Sample_Domain_Root_DataAccessLayer();
            mRand = new Random((int)DateTime.Now.Ticks);
            mTestModel = GenerateTestObject();
        }

        [Benchmark]
        public void DirectPropertyCallTests() {
            var index = mRand.Next(DirectCallInputs.Count);
            var inputSet = DirectCallInputs[index];
            var result = mDalUnderTest.FetchValue(mTestModel, inputSet.Item1);

            if (result is null) {
                throw new Exception($"Expression {inputSet.Item1} did not appear to be recognized");
            }
            if (!result.Equals(inputSet.Item2)) {
                throw new Exception($"Expression {inputSet.Item1} did not yield expected value! Expected: {inputSet.Item2} Actual: {result}");
            }
        }

        [Benchmark]
        public void ArrayCallTests() {
            var index = mRand.Next(ArrayCallInputs.Count);
            var inputSet = ArrayCallInputs[index];
            var result = mDalUnderTest.FetchValue(mTestModel, inputSet.Item1);

            if (result is null) {
                throw new Exception($"Expression {inputSet.Item1} did not appear to be recognized");
            }
            if (!result.Equals(inputSet.Item2)) {
                throw new Exception($"Expression {inputSet.Item1} did not yield expected value! Expected: {inputSet.Item2} Actual: {result}");
            }
        }

        [Benchmark]
        public void MixedCallTests() {
            var index = mRand.Next(MixedDataSet.Length);
            var inputSet = MixedDataSet[index];
            var result = mDalUnderTest.FetchValue(mTestModel, inputSet.Item1);

            if (result is null) {
                throw new Exception($"Expression {inputSet.Item1} did not appear to be recognized");
            }
            if (!result.Equals(inputSet.Item2)) {
                throw new Exception($"Expression {inputSet.Item1} did not yield expected value! Expected: {inputSet.Item2} Actual: {result}");
            }
        }

        private static Root GenerateTestObject() {
            var r = new Root() {
                DerpChild = new Derp() {
                    Id = 1000,
                    Uuid = "some uuid",
                    AllTheFlerbs = new Flerb[] {
                        new Flerb() {
                            FlerbId = "flerb_100",
                            SubIds = new int[] { 1, 2, 3, 4 }
                        },
                        new Flerb() {
                            FlerbId = "flerb_101",
                            SubIds = new int[] { 1, 2, 3, 4 }
                        },
                        new Flerb() {
                            FlerbId = "flerb_102",
                            SubIds = new int[] { 1, 2, 3, 4 }
                        },
                        new Flerb() {
                            FlerbId = "flerb_103",
                            SubIds = new int[] { 1, 2, 3, 4 }
                        },
                        new Flerb() {
                            FlerbId = "flerb_104",
                            SubIds = new int[] { 1, 2, 3, 4 }
                        },
                        new Flerb() {
                            FlerbId = "flerb_105",
                            SubIds = new int[] { 1, 2, 3, 4 }
                        },
                        new Flerb() {
                            FlerbId = "flerb_106",
                            SubIds = new int[] { 1, 2, 3, 4 }
                        },
                        new Flerb() {
                            FlerbId = "flerb_107",
                            SubIds = new int[] { 1, 2, 3, 4 }
                        },
                    },
                    OtherGlorp = new Glorp() {
                        IsEnabled = true,
                        Name = "derp_refGlorp",
                        ValueF = 3.14159f
                    }
                },
                GlorpChild = new Glorp() {
                    IsEnabled = false,
                    Name = "derp_baseGlorp",
                    ValueF = 1.1f
                },
                RootNodeId = "root_100"
            };

            return r;
        }
    }
}

