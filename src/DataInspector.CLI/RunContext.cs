namespace DataInspector.CLI {
    public class RunContext {
        public string InputLibrary { get; set; }

        public string InputTypeName { get; set; }

        public string OutputDirectory { get; set; }

        public string OutputNamespace { get; set; }

        public DALType DALType { get; set; }

        public bool UseLocalDeps { get; set; }
    }
}
