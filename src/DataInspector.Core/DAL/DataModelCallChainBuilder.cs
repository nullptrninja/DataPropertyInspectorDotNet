using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DataInspector.Core.DAL {
    public class DataModelCallChainBuilder  {

        // A deny-list of reference types we don't want to explore. Value types are always excluded.
        private readonly static HashSet<Type> NonExplorableTypes = new HashSet<Type>() { typeof(string) };
        private readonly static string[] ArrayPropertyDenyList = new[] { "IsSynchronized", "IsFixedSize", "IsReadOnly", "SyncRoot", "Rank", "LongLength", "Length" };

        private IDictionary<Type, PropertyInfo[]> mPropertiesForType;
        private IList<CallChainInfo> mCallChains;

        public DataModelPropertySheet BuildPropertySheet(Type rootType) {
            // The TypeMap finds all "explorable" types and their sub-properties
            BuildTypeMap(rootType);

            // We then discover all of the terminal call chains (all calls that can result in a primitive value)
            BuildCallChains(rootType);

            var ps = new DataModelPropertySheet(mPropertiesForType, mCallChains);
            return ps;
        }

        private void BuildTypeMap(Type rootType) {
            mPropertiesForType = new Dictionary<Type, PropertyInfo[]>();
            var propertiesToProcess = new Stack<PropertyInfo>();

            // Local helpers
            void AddPropertiesToProcess(PropertyInfo[] props) {
                // Only process types that we haven't mapped yet
                foreach (var p in props.Where(p => !mPropertiesForType.ContainsKey(p.PropertyType))) {
                    propertiesToProcess.Push(p);
                }
            }

            // Prime the stack
            var initialProps = GetPropertiesFromType(rootType);
            AddPropertiesToProcess(initialProps);
            AddPropertiesToPTMap(rootType, initialProps);

            while (propertiesToProcess.Count > 0) {
                var propertyToProcess = propertiesToProcess.Pop();

                // Ignore value types and certain reference types based on denied list
                if (!IsExplorableType(propertyToProcess.PropertyType) || mPropertiesForType.ContainsKey(propertyToProcess.PropertyType)) {
                    continue;
                }

                var subProps = GetPropertiesFromType(propertyToProcess.PropertyType);
                if (subProps.Length > 0) {
                    AddPropertiesToPTMap(propertyToProcess.PropertyType, subProps);
                }
            }
        }

        private void BuildCallChains(Type rootType) {
            mCallChains = new List<CallChainInfo>();
            var currentCallChain = new List<PropertyInfo>();
            var callChainsToProcess = new Stack<List<PropertyInfo>>();

            void AddPropertiesToCallChainsToProcess(IEnumerable<PropertyInfo> props) {
                var callsPerLevel = new List<PropertyInfo>(props);
                callChainsToProcess.Push(callsPerLevel);
            }

            CallChainInfo CreateCallChainInfo(List<PropertyInfo> callChainToConvert) {
                var chainAsString = string.Join('.', callChainToConvert.Select(p => p.Name));
                var chain = new CallChainInfo() {
                    CallChain = chainAsString,
                    CallChainProperties = callChainToConvert.ToArray()
                };

                return chain;
            }

            // Prime the stack
            var initialProps = GetPropertiesFromType(rootType);
            AddPropertiesToCallChainsToProcess(initialProps);

            while (callChainsToProcess.Count > 0) {
                var subCallsToProcess = callChainsToProcess.Peek();

                if (subCallsToProcess.Count == 0) {
                    callChainsToProcess.Pop();

                    // Remove the last node since we're done processing it. Since currentCallChain isn't primed
                    // like subCallsToProccess is, there's gonna be one missing, so just do empty check first.
                    if (currentCallChain.Count > 0) {
                        currentCallChain.Remove(currentCallChain.Last());
                    }
                    continue;
                }

                var subCall = subCallsToProcess.Last();                
                subCallsToProcess.RemoveAt(subCallsToProcess.Count - 1);
                
                var propertiesForSubCall = GetPropertiesFromType(subCall.PropertyType);

                // For arrays, add them in and keep cycling.
                if (subCall.PropertyType.IsArray) {
                    currentCallChain.Add(subCall);
                    AddPropertiesToCallChainsToProcess(propertiesForSubCall);

                    mCallChains.Add(CreateCallChainInfo(currentCallChain));        // Copy the current call chain
                    continue;
                }
                else if (propertiesForSubCall.Length > 0) {
                    // Regular exploration, keep digging into the property tree
                    currentCallChain.Add(subCall);
                    AddPropertiesToCallChainsToProcess(propertiesForSubCall);
                }
                else {
                    // This a leaf-property
                    currentCallChain.Add(subCall);
                    var callChain = CreateCallChainInfo(currentCallChain);
                    mCallChains.Add(callChain);
                    currentCallChain.Remove(currentCallChain.Last());
                }
            }
        }

        private void AddPropertiesToPTMap(Type thisLevelType, PropertyInfo[] allPropertiesForThisLevelType) {
            var filteredProps = allPropertiesForThisLevelType;
            mPropertiesForType.TryAdd(thisLevelType, filteredProps);
        }

        private static bool IsExplorableType(Type t) {
            return !t.IsValueType && !NonExplorableTypes.Contains(t);
        }

        private static PropertyInfo[] GetPropertiesFromType(Type t) {
            if (!IsExplorableType(t)) {
                return new PropertyInfo[0];
            }


            if (t.IsArray) {
                var properties = t.GetElementType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                                  .Where(p => !ArrayPropertyDenyList.Contains(p.Name))
                                  .ToArray();
                return properties;
            }
            else {
                var properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
                return properties;
            }
        }
    }
}
