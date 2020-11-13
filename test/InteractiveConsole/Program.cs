using System;
using Newtonsoft.Json;
using Sample.Domain;
using Sample.Domain.DAL;

namespace InteractiveConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var dal = new Sample_Domain_Root_DataAccessLayer();
            var targetModel = GenerateTestObject();
            var modelAsString = JsonConvert.SerializeObject(targetModel, Formatting.Indented);

            Console.WriteLine($"Your sample data model:\n{modelAsString}");
            while (true)
            {
                Console.Write("\n$INPUT.");
                var expr = Console.ReadLine();

                if (string.IsNullOrEmpty(expr))
                {
                    Console.WriteLine($"Your sample data model:\n{modelAsString}");
                    continue;
                }

                var eval = dal.FetchValue(targetModel, expr);
                if (eval is null)
                {
                    Console.WriteLine("Invalid expression or call-chain does not exist");
                }
                else
                {
                    Console.WriteLine($"Value: {eval}\n");
                }
            }

        }

        private static Root GenerateTestObject()
        {
            var r = new Root()
            {
                DerpChild = new Derp()
                {
                    Id = 1000,
                    Uuid = "some uuid",
                    AllTheFlerbs = new Flerb[] {
                        new Flerb() {
                            FlerbId = "flerb_100",
                            SubIds = new int[] { 1, 2, 3, 4 }
                        },
                        new Flerb() {
                            FlerbId = "flerb_101"
                            // No SubIds
                        }
                    },
                    OtherGlorp = new Glorp()
                    {
                        IsEnabled = true,
                        Name = "derp_refGlorp",
                        ValueF = 3.14159f
                    }
                },
                GlorpChild = new Glorp()
                {
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
