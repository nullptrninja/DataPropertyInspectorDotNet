using System;
using System.Collections.Generic;
using DataInspector.Core.DAL;
using DataInspector.Core.Expression;
using DataInspector.Parser;

namespace DataInspector.DataAccess.DAL {
    public abstract class BaseDirectPropertyCallDAL<TDataModel> : IDataModelAccessLayer<TDataModel> 
        where TDataModel : class {

        private readonly IExpressionParser<TDataModel> mParser;
        protected readonly Dictionary<string, Func<TDataModel, object>> callChainDispatchMap;
        protected readonly Dictionary<string, Func<TDataModel, int[], object>> callChainArrayDispatchMap;

        public BaseDirectPropertyCallDAL() {
            mParser = new DirectInvocationExpressionParser<TDataModel>();
            callChainDispatchMap = new Dictionary<string, Func<TDataModel, object>>();
            callChainArrayDispatchMap = new Dictionary<string, Func<TDataModel, int[], object>>();
        }

        public object FetchValue(TDataModel targetObject, string expression) {
            var parsedQuery = mParser.Parse(expression);

            if (!parsedQuery.IsValid) {
                throw new ArgumentException($"Expression: {expression ?? "<null>"} could not be parsed.");
            }

            object subExprResult = null;
            if (parsedQuery.IsArrayExpression) {
                subExprResult = Invoke(targetObject, parsedQuery.LookUpKey, parsedQuery.ArrayIndicies);
            }
            else {
                subExprResult = Invoke(targetObject, parsedQuery.LookUpKey);
            }

            return subExprResult;
        }

        public TOutput FetchValue<TOutput>(TDataModel targetObject, string expression) {
            var r = FetchValue(targetObject, expression);
            return (TOutput)r;
        }

        protected object Invoke(TDataModel targetObject, string callChain) {
            if (callChainDispatchMap.TryGetValue(callChain, out var valFn)) {
                return valFn(targetObject);
            }
            return null;
        }

        protected object Invoke(TDataModel targetObject, string callChain, params int[] indicies) {
            if (callChainArrayDispatchMap.TryGetValue(callChain, out var valFn)) {
                return valFn(targetObject, indicies);
            }
            return null;
        }
    }
}
