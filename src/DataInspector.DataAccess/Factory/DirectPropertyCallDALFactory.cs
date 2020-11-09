using System.Collections.Generic;
using System.Text;
using DataInspector.Core.DAL;
using DataInspector.Core.Utilities;

namespace DataInspector.DataAccess.Factory {
    public class DirectPropertyCallDALFactory : IDataModelAccessLayerFactory {
        
        public string EmitDataAccessLayerCode(DataModelPropertySheet dataModelPropertySheet, DALBuilderContext buildContext) {
            var generatedCode = GenerateCodeFrom(dataModelPropertySheet, buildContext);
            return generatedCode;
        }

        private static string GenerateCodeFrom(DataModelPropertySheet dataModelPropertySheet, DALBuilderContext buildContext) {
            // 0: Class name (probably DM typename?)
            // 1: DataModel type name
            // 2: Ctor body
            // 3: DAL Function definitions
            // 4: Namespace to house the DAL type
            // 5: TDataModel namespace
            const string baseTemplate =                
                "using System;\n" +
                "using DataInspector.DataAccess.DAL;\n" +
                "using {5};\n\n" +
                "namespace {4} {{\n" +
                "\tpublic class {0} : BaseDirectPropertyCallDAL<{1}> {{\n" +
                    "\t\tpublic {0}() : base() {{\n" +
                        "{2}" +
                    "\t\t}}\n" +
                    "{3}\n" +
                "\t}}\n}}\n";

            // 0: Normalized call chain as dict key
            // 1: Name of DAL function to call (use dalFnNameTemplate)
            const string ctorRegisterFnTemplate = "\t\t\tcallChainDispatchMap.Add(\"{0}\", {1});\n";

            // 0: Normalized call chain as dict key
            // 1: Name of DAL function to call (use dalArrayFnNameTemplate)
            const string ctorRegisterArrayFnTemplate = "\t\t\tcallChainArrayDispatchMap.Add(\"{0}\", {1});\n";

            // 0: "Friendly" call chain (complies to function naming rules)
            // 1: Root object type
            const string dalFnHeaderTemplate = "private object {0}({1} inputObject)";

            // 0: "Friendly" call chain (complies to function naming rules)
            // 1: Root object type
            const string dalArrayFnHeaderTemplate = "private object {0}({1} inputObject, int[] indicies)";

            // 0: Function name
            // 1: Call chain to getter
            // 2: Root object type
            const string dalArrayFnDefinitionTemplate =
                "\t\t{0} {{\n" +
                    "\t\t\treturn inputObject.{1};\n" +
                "\t\t}}\n";

            // 0: Function name
            // 1: Call chain to getter
            // 2: Root object type
            const string dalFnDefinitionTemplate =
                "\t\t{0} {{\n" +
                    "\t\t\treturn inputObject.{1};\n" +
                "\t\t}}\n";

            var dataModelTypeName = buildContext.RootDataModelType.Name;
            var ctorCode = new StringBuilder();
            var functionBodies = new StringBuilder();            

            foreach (var cci in dataModelPropertySheet.CallChains) {
                var callChainFriendlyName = TypeUtility.GetFunctionNameFromCallChain(cci.CallChain);
                var dalLookupKey = CallChainUtility.GetDALLookUpKey(cci);

                if (cci.IncludesArrayIndexer) {                                        
                    var dalFnName = string.Format(dalArrayFnHeaderTemplate, callChainFriendlyName, dataModelTypeName);
                    var renderedCallChainTemplate = BuildArrayCallChainTemplate(cci, "indicies");

                    var ctorArrayRegisterCall = string.Format(ctorRegisterArrayFnTemplate, dalLookupKey, callChainFriendlyName);
                    ctorCode.Append(ctorArrayRegisterCall);

                    var dalFnBody = string.Format(dalArrayFnDefinitionTemplate, dalFnName, renderedCallChainTemplate);
                    functionBodies.Append(dalFnBody);
                }
                else {
                    var dalFnName = string.Format(dalFnHeaderTemplate, callChainFriendlyName, dataModelTypeName);
                    var ctorRegisterCall = string.Format(ctorRegisterFnTemplate, dalLookupKey, callChainFriendlyName);
                    ctorCode.Append(ctorRegisterCall);

                    var dalFnBody = string.Format(dalFnDefinitionTemplate, dalFnName, cci.CallChain);
                    functionBodies.Append(dalFnBody);
                }
            }

            var functionDefinitionCodeBody = functionBodies.ToString();
            var ctorSetupCode = ctorCode.ToString();
            var code = string.Format(baseTemplate,
                                     TypeUtility.GetClassNameFromType(buildContext.RootDataModelType),
                                     dataModelTypeName,
                                     ctorSetupCode,
                                     functionDefinitionCodeBody,
                                     buildContext.OutputNamespace,
                                     TypeUtility.GetUsingNamespaceForType(buildContext.RootDataModelType));

            return code;
        }

        private static string BuildArrayCallChainTemplate(CallChainInfo cci, string indexerVarName) {
            var tokens = new List<string>(cci.CallChainProperties.Length);
            var indexer = 0;
            foreach (var call in cci.CallChainProperties) {
                var pt = call.PropertyType;
                var callAsStr = pt.IsArray ? $"{call.Name}[{indexerVarName}[{indexer++}]]" : $"{call.Name}";
                tokens.Add(callAsStr);
            }

            return string.Join('.', tokens);
        }
    }
}
