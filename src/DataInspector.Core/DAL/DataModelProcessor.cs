using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DataInspector.Core.Expression;

namespace DataInspector.Core.DAL {
    public class DataModelProcessor<TDataModel>
        where TDataModel : class {

        private readonly IExpressionParser<TDataModel> expressionParser;

        public DataModelProcessor(IExpressionParser<TDataModel> exprParser) {
            expressionParser = exprParser;
        }
    }
}
