using System;
using System.Linq;
using System.Reflection;

namespace DataInspector.Core.DAL {
    public class CallChainInfo {
        public PropertyInfo[] CallChainProperties { get; set; }

        public string CallChain { get; set; }

        public Type CallChainResolvesToType => CallChainProperties.Last().PropertyType;

        public bool LastPropertyIsArray => CallChainResolvesToType.IsArray;
    }
}
