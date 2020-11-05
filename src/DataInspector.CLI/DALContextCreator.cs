using System;
using System.Linq;
using System.Reflection;
using DataInspector.Core.DAL;

namespace DataInspector.CLI {
    internal class DALContextCreator {
        public DALBuilderContext ConvertRunContext(RunContext runContext) {            
            // Attempt to load the specified library
            if (!TryLoadAssembly(runContext.InputLibrary, out var coreAssembly)) {
                throw new Exception($"Unable to load library: {runContext.InputLibrary}");
            }

            var coreTypeInfo = coreAssembly.DefinedTypes.Single(ti => ti.FullName.Equals(runContext.InputTypeName, StringComparison.InvariantCultureIgnoreCase));
            var coreType = coreTypeInfo.UnderlyingSystemType;

            var dalContextDesc = new DALBuilderContext() {
                RootDataModelType = coreType,
                OutputDirectory = runContext.OutputDirectory,
                OutputNamespace = runContext.OutputNamespace
            };

            return dalContextDesc;
        }

        private static bool TryLoadAssembly(string libraryPath, out Assembly asm) {
            try {
                asm = Assembly.LoadFile(libraryPath);
                return true;
            }
            catch (Exception) {
                asm = null;
                return false;
            }
        }
    }
}
