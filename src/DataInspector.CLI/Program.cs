using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;
using System.Text;
using DataInspector.Core.DAL;
using DataInspector.Core.Utilities;
using DataInspector.DataAccess.Factory;

namespace DataInspector.CLI {
    public class Program {
        
        static int Main(string[] args) {
            var rootCommand = new RootCommand {
                new Option<string>(
                    "--inputLib",
                    description: "The full path to the library that contains the target data type"),
                new Option<string>(
                    "--inputTypeName",
                    description: "The fully qualified namespace of the data type that the DAL will be based on"),
                new Option<string>(
                    "--outputDir",
                    description: "The directory where the generated DAL file will be copied to"),
                new Option<string>(
                    "--outputNs",
                    description: "The namespace that the generated DAL will use"),
                new Option<DALType>(
                    "--daltype",
                    getDefaultValue: () => DALType.Direct,
                    description: "Which type of data access layer to generate")
            };

            rootCommand.Description = "Generates a data access layer from a type in an external assembly.";

            RunContext context = null;
            rootCommand.Handler = CommandHandler.Create<string, string, string, string, DALType>((inputLib, inputTypeName, outputDir, outputNS, dalType) => {
                context = new RunContext {
                    InputLibrary = inputLib,
                    InputTypeName = inputTypeName,
                    OutputDirectory = outputDir,
                    OutputNamespace = outputNS,
                    DALType = dalType,
                };

                RunGenerator(context);
            });

            return rootCommand.InvokeAsync(args).Result;
        }

        private static void RunGenerator(RunContext context) {
            var contextConverter = new DALContextCreator();
            var dalContext = contextConverter.ConvertRunContext(context);

            var callChainData = new DataModelCallChainBuilder().BuildPropertySheet(dalContext.RootDataModelType);
            
            var dalFactory = GetFactory(context.DALType);
            var generatedDalCode = dalFactory.EmitDataAccessLayerCode(callChainData, dalContext);

            WriteFile(context.OutputDirectory, TypeUtility.GetClassNameFromType(dalContext.RootDataModelType) + ".cs", generatedDalCode);
        }

        private static IDataModelAccessLayerFactory GetFactory(DALType dalType) {
            switch (dalType) {
                case DALType.Direct:
                    return new DirectPropertyCallDALFactory();

                case DALType.Reflective:
                    return new ReflectiveDALFactory();

                default:
                    throw new NotSupportedException($"No supported DAL Factory of type: {dalType}");
            }
        }

        private static void WriteFile(string outputDir, string fileName, string contents) {
            string fullPath = Path.Combine(outputDir, fileName);

            using var stream = new StreamWriter(fullPath, false, Encoding.UTF8);
            stream.WriteLine(contents);
        }
    }
}
