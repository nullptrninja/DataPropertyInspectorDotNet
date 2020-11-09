namespace DataInspector.Core.Expression {
    public interface IExpressionParser<TDataModel>
        where TDataModel : class {
        IParsedExpression Parse(string expression);
    }
}
