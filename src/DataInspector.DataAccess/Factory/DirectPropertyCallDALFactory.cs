using System.Linq;
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
                "using {5};\n" +
                "namespace {4} {{\n" +
                "public class {0} : BaseDirectPropertyCallDAL<{1}> {{\n" +
                    "public {0}() : base() {{\n" +
                        "{2}" +
                    "}}\n" +
                    "{3}\n" +
                "}}\n}}\n";

            // 0: Normalized call chain as dict key
            // 1: Name of DAL function to call (use dalFnNameTemplate)
            const string ctorRegisterFnTemplate = "callChainDispatchMap.Add(\"{0}\", {1});\n";

            // 0: Normalized call chain as dict key
            // 1: Name of DAL function to call (use dalArrayFnNameTemplate)
            const string ctorRegisterArrayFnTemplate = "callChainArrayDispatchMap.Add(\"{0}[]\", {1});\n";

            // 0: "Friendly" call chain (complies to function naming rules)            
            const string dalFnHeaderTemplate = "private object {0}(object inputObject)";

            // 0: "Friendly" call chain (complies to function naming rules)            
            const string dalArrayFnHeaderTemplate = "private object {0}(object inputObject, int index)";

            // 0: Function name
            // 1: Call chain to getter
            // 2: Root object type
            const string dalArrayFnDefinitionTemplate =
                "{0} {{\n" +
                    "return (inputObject as {2}).{1}[index];\n" +
                "}}\n";

            // 0: Function name
            // 1: Call chain to getter
            // 2: Root object type
            const string dalFnDefinitionTemplate =
                "{0} {{\n" +
                    "return (inputObject as {2}).{1};\n" +
                "}}\n";

            var dataModelTypeName = buildContext.RootDataModelType.Name;
            var ctorCode = new StringBuilder();
            var functionBodies = new StringBuilder();            

            for (var i = 0; i < dataModelPropertySheet.CallChains.Count; ++i) {
                var cci = dataModelPropertySheet.CallChains[i];
                var callChainFriendlyName = TypeUtility.GetFunctionNameFromCallChain(cci.CallChain);

                if (cci.LastPropertyIsArray) {
                    //var arrayElementType = cci.CallChainProperties.Last().PropertyType.GetElementType().Name;
                    var rootTokenBaseType = cci.CallChainProperties.First().PropertyType.Name;
                    var dalFnName = string.Format(dalArrayFnHeaderTemplate, callChainFriendlyName);
                    var ctorArrayRegister = string.Format(ctorRegisterArrayFnTemplate, cci.CallChain.ToLower(), callChainFriendlyName);
                    ctorCode.Append(ctorArrayRegister);
                    
                    var dalFnBody = string.Format(dalArrayFnDefinitionTemplate, dalFnName, cci.CallChain, rootTokenBaseType);
                    functionBodies.Append(dalFnBody);
                }
                else {
                    var dalFnName = string.Format(dalFnHeaderTemplate, callChainFriendlyName);
                    var ctorRegister = string.Format(ctorRegisterFnTemplate, cci.CallChain.ToLower(), callChainFriendlyName);
                    ctorCode.Append(ctorRegister);

                    var dalFnBody = string.Format(dalFnDefinitionTemplate, dalFnName, cci.CallChain, dataModelTypeName);
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
    }
}
