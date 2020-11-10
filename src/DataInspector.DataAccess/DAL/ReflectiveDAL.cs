using System;
using System.Linq;
using System.Reflection;
using DataInspector.Core.DAL;
using DataInspector.Core.Expression;
using DataInspector.Parser;

namespace DataInspector.DataAccess.DAL {
    public class ReflectiveDAL<TDataModel> : IDataModelAccessLayer<TDataModel>
        where TDataModel : class {

        private Type mCachedType;
        private readonly IExpressionParser<TDataModel> mParser;

        public ReflectiveDAL() {
            mCachedType = typeof(TDataModel);
            mParser = new ReflectiveInvocationExpressionParser<TDataModel>();
        }

        public object FetchValue(TDataModel targetObject, string expression) {
            var parsedExpr = mParser.Parse(expression);
            return FetchValue(targetObject, parsedExpr);
        }

        public TOutput FetchValue<TOutput>(TDataModel targetObject, string expression) {
            var val = FetchValue(targetObject, expression);
            return (TOutput)val;
        }

        public object FetchValue(TDataModel targetObject, IParsedExpression expression) {
            return FetchValueByReflection(targetObject, expression);
        }

        public TOutput FetchValue<TOutput>(TDataModel targetObject, IParsedExpression expression) {
            var val = FetchValue(targetObject, expression);
            return (TOutput)val;
        }

        private object FetchValueByReflection(TDataModel rootObject, IParsedExpression expression) {

            var currentType = mCachedType;
            var currentObject = (object)rootObject;

            for (var i = 0; i < expression.Tokens.Length; ++i) {
                var token = expression.Tokens[i];

                var nextProp = GetPropertyByName(currentType, token.SubExpression);
                if (token.IsArrayExpression) {
                    currentObject = (nextProp.GetValue(currentObject) as Array).GetValue(token.ArrayIndex);
                    currentType = nextProp.PropertyType.GetElementType();
                }
                else {
                    currentObject = nextProp.GetValue(currentObject);
                    currentType = nextProp.PropertyType;
                }                
            }

            return currentObject;
        }

        private PropertyInfo GetPropertyByName(Type t, string propName) {
            var prop = t.GetProperty(propName);
            return prop;
        }
    }
}
