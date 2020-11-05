using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DataInspector.Core.DAL {
    public class DataModelPropertySheet {

        public IDictionary<Type, PropertyInfo[]> PropertiesForType { get; }
        public IList<CallChainInfo> CallChains { get; }

        public DataModelPropertySheet(IDictionary<Type, PropertyInfo[]> propsForTypeMapping, IList<CallChainInfo> terminalCallChains) {
            PropertiesForType = propsForTypeMapping;
            CallChains = terminalCallChains;
        }

        public IEnumerable<string> GetRequiredAssemblyLocations() {
            return PropertiesForType.Select(a => a.Key.Assembly.Location);
        }
    }
}
