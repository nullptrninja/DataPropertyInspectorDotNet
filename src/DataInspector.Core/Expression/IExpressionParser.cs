namespace DataInspector.Core.Expression {
    public interface IExpressionParser<TDataModel>
        where TDataModel : class {
        IParsedQuery Parse(string expression);
    }
}
