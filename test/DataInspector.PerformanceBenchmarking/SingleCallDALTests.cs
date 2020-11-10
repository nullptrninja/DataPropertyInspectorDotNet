using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Sample.Domain;
using Sample.Domain.DAL;

namespace DataInspector.PerformanceBenchmarking {
    [SimpleJob(runStrategy: RunStrategy.Throughput, launchCount: 1, warmupCount: 1, invocationCount: 1000000)]
    public class SingleCallDALTests {
        private readonly List<string> DirectCallInputs = new List<string> {
            "DerpChild.Id",
            "DerpChild.Uuid",
            "DerpChild.ReferentialGlorp.Name",
            "DerpChild.ReferentialGlorp.IsEnabled",
            "DerpChild.ReferentialGlorp.ValueF",
            "GlorpChild.Name",
            "GlorpChild.IsEnabled",
            "GlorpChild.ValueF",
            "RootNodeId"
        };

        private readonly List<string> ArrayCallInputs = new List<string> {
            "DerpChild.AllTheFlerbs[0].FlerbId",
            "DerpChild.AllTheFlerbs[1].FlerbId",
            "DerpChild.AllTheFlerbs[2].FlerbId",
            "DerpChild.AllTheFlerbs[3].FlerbId",
            "DerpChild.AllTheFlerbs[4].FlerbId",
            "DerpChild.AllTheFlerbs[5].FlerbId",
            "DerpChild.AllTheFlerbs[6].FlerbId",
            "DerpChild.AllTheFlerbs[7].FlerbId",

            "DerpChild.AllTheFlerbs[0].SubIds[0]",
            "DerpChild.AllTheFlerbs[1].SubIds[0]",
            "DerpChild.AllTheFlerbs[2].SubIds[0]",
            "DerpChild.AllTheFlerbs[3].SubIds[0]",
            "DerpChild.AllTheFlerbs[4].SubIds[0]",
            "DerpChild.AllTheFlerbs[5].SubIds[0]",
            "DerpChild.AllTheFlerbs[6].SubIds[0]",
            "DerpChild.AllTheFlerbs[7].SubIds[0]",

            "DerpChild.AllTheFlerbs[0].SubIds[3]",
            "DerpChild.AllTheFlerbs[1].SubIds[3]",
            "DerpChild.AllTheFlerbs[2].SubIds[3]",
            "DerpChild.AllTheFlerbs[3].SubIds[3]",
            "DerpChild.AllTheFlerbs[4].SubIds[3]",
            "DerpChild.AllTheFlerbs[5].SubIds[3]",
            "DerpChild.AllTheFlerbs[6].SubIds[3]",
            "DerpChild.AllTheFlerbs[7].SubIds[3]",
        };

        private Random mRand;
        private Sample_Domain_Root_DataAccessLayer mDalUnderTest;
        private Root mTestModel;

        public string[] MixedDataSet => DirectCallInputs.Concat(ArrayCallInputs)
                                                                       .Shuffle()
                                                                       .ToArray();

        [GlobalSetup]
        public void Setup() {
            mDalUnderTest = new Sample_Domain_Root_DataAccessLayer();
            mRand = new Random((int)DateTime.Now.Ticks);
            mTestModel = GenerateTestObject();
        }

        [Benchmark]
        public void BatchedDirectPropertyCallTests() {
            var index = mRand.Next(DirectCallInputs.Count);
            var expr = DirectCallInputs[index];
            mDalUnderTest.FetchValue(mTestModel, expr);
        }

        [Benchmark]
        public void BatchedArrayCallTests() {
            var index = mRand.Next(ArrayCallInputs.Count);
            var expr = ArrayCallInputs[index];
            mDalUnderTest.FetchValue(mTestModel, expr);
        }

        [Benchmark]
        public void BatchedMixedCallTests() {
            var index = mRand.Next(MixedDataSet.Length);
            var expr = MixedDataSet[index];
            mDalUnderTest.FetchValue(mTestModel, expr);
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
                    ReferentialGlorp = new Glorp() {
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

