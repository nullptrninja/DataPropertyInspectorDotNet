using System;
using System.Collections.Generic;
using DataInspector.Core.DAL;
using DataInspector.Core.Expression;
using DataInspector.Parser;

namespace DataInspector.DataAccess.DAL {
    public abstract class BaseDirectPropertyCallDAL<TDataModel> : IDataModelAccessLayer<TDataModel> 
        where TDataModel : class {

        private readonly IExpressionParser<TDataModel> mParser;
        protected readonly Dictionary<string, Func<object, object>> callChainDispatchMap;
        protected readonly Dictionary<string, Func<object, int, object>> callChainArrayDispatchMap;

        public BaseDirectPropertyCallDAL() {
            mParser = new QueryableChunkExpressionParser<TDataModel>();
            callChainDispatchMap = new Dictionary<string, Func<object, object>>();
            callChainArrayDispatchMap = new Dictionary<string, Func<object, int, object>>();
        }

        public object FetchValue(TDataModel targetObject, string expression) {
            var queryTokens = mParser.Parse(expression);

            if (queryTokens.Length == 0) {
                throw new ArgumentException($"Expression: {expression ?? "<null>"} could not be parsed.");
            }
            else {
                object subExprResult = targetObject;
                for (var i = 0; i < queryTokens.Length; ++i) {
                    var qt = queryTokens[i];

                    if (qt.IsArrayExpression) {
                        subExprResult = Invoke(subExprResult, qt.Subexpression, qt.ArrayIndex);
                    }
                    else {
                        subExprResult = Invoke(subExprResult, qt.Subexpression);
                    }
                }

                return subExprResult;
            }
        }

        public TOutput FetchValue<TOutput>(TDataModel targetObject, string expression) {
            var r = FetchValue(targetObject, expression);
            return (TOutput)r;
        }

        protected object Invoke(object targetObject, string callChain) {
            if (callChainDispatchMap.TryGetValue(callChain, out var valFn)) {
                return valFn(targetObject);
            }
            return null;
        }

        protected object Invoke(object targetObject, string callChain, int arrayIndex) {
            if (callChainArrayDispatchMap.TryGetValue(callChain, out var valFn)) {
                return valFn(targetObject, arrayIndex);
            }
            return null;
        }
    }
}
