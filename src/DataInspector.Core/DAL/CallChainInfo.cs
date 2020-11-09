using System;
using System.Linq;
using System.Reflection;

namespace DataInspector.Core.DAL {
    /// <summary>
    /// Information about a full call chain. Note that the root object is not included in the call chain.
    /// </summary>
    public class CallChainInfo {
        public PropertyInfo[] CallChainProperties { get; set; }

        public string CallChain { get; set; }

        public Type CallChainResolvesToType => CallChainProperties.Last().PropertyType;

        public bool IncludesArrayIndexer => CallChainProperties.Any(p => p.PropertyType.IsArray);
    }
}
