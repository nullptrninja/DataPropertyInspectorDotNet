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
            throw new NotImplementedException();
        }

        public object FetchValue(TDataModel targetObject, IParsedExpression expression) {
            return FetchValueByReflection(targetObject, expression);
        }

        public TOutput FetchValue<TOutput>(TDataModel targetObject, IParsedExpression expression) {
            throw new NotImplementedException();
        }

        private object FetchValueByReflection(TDataModel rootObject, IParsedExpression expression) {

            var currentType = mCachedType;
            var currentObject = (object)rootObject;

            for (var i = 0; i < expression.Tokens.Length; ++i) {
                var token = expression.Tokens[i];

                var targetProp = GetPropertyByName(currentType, token.SubExpression);
                currentType = targetProp.DeclaringType;
                if (token.IsArrayExpression) {
                    currentObject = targetProp.GetValue(currentObject, new object[] { token.ArrayIndex });                    
                }
                else {
                    currentObject = targetProp.GetValue(currentObject);
                }
            }

            return currentObject;
        }

        private PropertyInfo[] GetPropertiesForType(Type t) {
            if (t.IsArray) {
                var props = t.GetElementType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
                return props;
            }
            else {
                var props = t.GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
                return props;
            }
        }

        private PropertyInfo GetPropertyByName(Type t, string propName) {
            var prop = GetPropertiesForType(t).Single(p => p.Name.Equals(propName, StringComparison.InvariantCultureIgnoreCase));
            return prop;
        }
    }
}
