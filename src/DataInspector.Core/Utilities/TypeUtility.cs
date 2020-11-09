using System;

namespace DataInspector.Core.Utilities {
    public static class TypeUtility {
        public static string GetFunctionNameFromCallChain(string callChain) {
            return callChain.Replace('.', '_');
        }

        public static string GetClassNameFromType(Type t) {
            return $"{t.FullName.Replace('.', '_')}_DataAccessLayer";
        }

        public static string GetUsingNamespaceForType(Type t) {
            return t.Namespace;
        }
    }
}
