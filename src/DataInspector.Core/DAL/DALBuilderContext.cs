using System;

namespace DataInspector.Core.DAL {
    public class DALBuilderContext {
        public Type RootDataModelType { get; set; }

        public string OutputNamespace { get; set; }

        public string OutputDirectory { get; set; }
    }
}
