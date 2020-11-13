using System;
using Alt.MSFTRulesEngine;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using RulesEngine.Models;
using Sample.Domain;

namespace DataInspector.PerformanceBenchmarking
{
    [SimpleJob(runStrategy: RunStrategy.Throughput, launchCount: 1, warmupCount: 1, invocationCount: 1)]
    public class LargeBatchedRulesEngineTests {
        private const int BatchSize = 200000;

        private RulesEngineBasedProcessor<Root> mRulesProcessor;
        private Root mTestModel;

        [GlobalSetup]
        public void Setup() {
            mRulesProcessor = new RulesEngineBasedProcessor<Root>(GenerateWorkflows());
            mTestModel = GenerateTestObject();
        }

        [Benchmark]
        public void BatchedDirectPropertyCallTests() {
            for (var i = 0; i < BatchSize; ++i) {
                if (!mRulesProcessor.MatchesAnyRule(mTestModel))
                {
                    throw new Exception("Rule check failed unexpectedly");
                }
            }
        }

        private static WorkflowRules[] GenerateWorkflows()
        {
             return new WorkflowRules[] {
                 new WorkflowRules()
                 {
                     WorkflowName = "PreprocessorWorkflowChecks",
                     Rules = new Rule[]
                     {
                         new Rule()
                         {
                             RuleName = "ContainsDerp",
                             RuleExpressionType = RuleExpressionType.LambdaExpression,
                             Expression = "input1.GlorpChild.Name.Contains(\"derp\")",
                             SuccessEvent = "WriteToSlack"
                         },
                         new Rule()
                         {
                             RuleName = "OtherGlorpIsEnabled",
                             RuleExpressionType = RuleExpressionType.LambdaExpression,
                             Expression = "DerpChild.OtherGlorp.IsEnabled",
                             SuccessEvent = "WriteToLog"
                         },
                         new Rule()         // We'll count this as 2 condition checks
                         {
                             RuleName = "FirstFlerbContainsSubId4",
                             RuleExpressionType = RuleExpressionType.LambdaExpression,
                             Expression = "DerpChild.AllTheFlerbs[0].SubIds?.Length > 0 && DerpChild.AllTheFlerbs[0].SubIds[0].Contains(4)",
                             SuccessEvent = "WriteToLog"
                         },
                         new Rule()
                         {
                             RuleName = "LastFlerbHasSubIds",
                             RuleExpressionType = RuleExpressionType.LambdaExpression,
                             Expression = "DerpChild.AllTheFlerbs.Last().SubIds.Count() > 0",
                             SuccessEvent = "WriteToLog"
                         },
                     },
                 },
             };
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

