using DataInspector.Core.DAL;
using DataInspector.Core.Utilities;

namespace DataInspector.DataAccess.Factory {
    public class ReflectiveDALFactory : IDataModelAccessLayerFactory {
        public string EmitDataAccessLayerCode(DataModelPropertySheet dataModelPropertySheet, DALBuilderContext buildContext) {
            // 0: Class name (probably DM typename?)
            // 1: DataModel type name
            // 2: Namespace to house the DAL type
            // 3: TDataModel namespace
            const string baseTemplate =
                "using System;\n" +
                "using DataInspector.DataAccess.DAL;\n" +
                "using {3};\n\n" +
                "namespace {2} {{\n" +
                "\tpublic class {0} : ReflectiveDAL<{1}> {{\n" +
                    "\t\tpublic {0}() : base() {{ }}\n" +
                "\t}}\n}}\n";

            var dataModelTypeName = buildContext.RootDataModelType.Name;
            var renderedTemplate = string.Format(baseTemplate,
                                                 TypeUtility.GetClassNameFromType(buildContext.RootDataModelType),         // Class name
                                                 dataModelTypeName,
                                                 buildContext.OutputNamespace,
                                                 TypeUtility.GetUsingNamespaceForType(buildContext.RootDataModelType));

            return renderedTemplate;
        }
    }
}
