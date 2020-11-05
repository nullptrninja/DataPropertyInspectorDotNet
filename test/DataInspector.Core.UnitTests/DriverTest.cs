using System;
using DataInspector.Core.Expression;
using DataInspector.Core.Processor;
using DataInspector.Core.Processor.Compiler;
using DataInspector.Core.Processor.DAL;
using Xunit;

namespace DataInspector.Core.UnitTests {
    public class DriverTest {
        [Fact]
        public void Driver_BuildCallChainGraph() {
            DataModelCallChainBuilder<Derp> gb = new DataModelCallChainBuilder<Derp>();
            gb.BuildPropertySheet();
        }

        [Fact]
        public void Driver_InvokeCallChain() {
            var derp = new Derp() {
                A1 = new Glorp() {
                    B1 = new Flerb() {
                        T1 = "A1,B1,T1",
                        T2 = "A1,B1,T2"
                    },
                    T1 = "A1,T1",
                    AR1 = new[] { "derp", "glorp", "flerb" }
                },
                A2 = new Flerb() {
                    T1 = "A2,T1",
                    T2 = "A2,T2"
                },
                T1 = "T1"
            };

            var derp2 = new Derp2() {
                Name = "level1",
                Nodes = new[] {
                    new Derp2() {
                        Name = "level2_1",
                        Nodes = new Derp2[] {
                            new Derp2() {
                                Name = "level3_1"
                            }
                        }
                    },
                    new Derp2() {
                        Name = "level2_2",
                        Nodes = new Derp2[] {
                            new Derp2() {
                                Name = "level3_2",
                                Nodes = new Derp2[] {
                                    new Derp2() {
                                        Name = "level4_2"
                                    }
                                }
                            }
                        }
                    },
                }
            };

            DataModelCallChainBuilder<Derp2> dmccb = new DataModelCallChainBuilder<Derp2>();
            var ps = dmccb.BuildPropertySheet();
            var dpcDalFactory = new DirectPropertyCallDALFactory<Derp2>();
            var context = new DALBuilderContext() {
                Namespace = "DataInspectorNS"
            };

            using var dal = dpcDalFactory.CreateDataAccessLayer(ps, context);
            var output = dal.FetchValue<string>(derp2, "Nodes[1].Nodes[0].Nodes[0].Name");      // level4_2

            var start = DateTime.Now;
            for (var i = 0; i < 1000000; ++i) {
                dal.FetchValue(derp2, "Nodes[1].Nodes[0].Nodes[0].Name");      // level4_2
            }
            var end = DateTime.Now - start;
            Console.WriteLine(end);
        }

        [Fact]
        public void Driver_ParseExpression() {
            DataModelCallChainBuilder<Derp> gb = new DataModelCallChainBuilder<Derp>();
            gb.BuildPropertySheet();

            // Ex: All expressions are anchored to a variable.
            // The variable anchor is a special token: $input followed by the call chain specific to that type:
            // Ex: Assuming $input is of type Derp:
            //      $input.A2.T2

            var parser = new QueryableChunkExpressionParser<Derp>();
            //parser.Parse("A1.B1.T2");
            //parser.Parse("A1.AR1[0]");
            parser.Parse("AR1[2].A1.B1.C1.AR2[0]");

        }
    }

    public class Derp {
        public Glorp A1 { get; set; }
        public Flerb A2 { get; set; }
        public string T1 { get; set; }
    }

    public class Glorp {
        public Flerb B1 { get; set; }
        public string T1 { get; set; }
        public string[] AR1 { get; set; }
    }

    public class Flerb {
        public string T1 { get; set; }
        public string T2 { get; set; }
    }

    public class Derp2 {
        public string Name { get; set; }
        public Derp2[] Nodes { get; set; }
    }
}
