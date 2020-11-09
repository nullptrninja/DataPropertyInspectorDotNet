using System.Collections.Generic;
using System.Linq;
using DataInspector.Core.DAL;
using DataInspector.Core.Expression;

namespace DataInspector.Core.Utilities {
    public static class CallChainUtility {
        public static string GetDALLookUpKey(CallChainInfo cci) {
            if (cci.CallChainProperties.Any(c => c.PropertyType.IsArray)) {
                var tokens = new List<string>(cci.CallChainProperties.Length);

                foreach (var call in cci.CallChainProperties) {
                    var pt = call.PropertyType;
                    var callAsStr = pt.IsArray ? $"{call.Name}[]" : $"{call.Name}";
                    tokens.Add(callAsStr.ToLower());
                }

                return string.Join('.', tokens);
            }
            else {
                return cci.CallChain.ToLower();
            }
        }
    }
}
