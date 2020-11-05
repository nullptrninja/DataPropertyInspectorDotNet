namespace DataInspector.Core.Expression {
    public interface IExpressionParser<TDataModel>
        where TDataModel : class {
        QueryableToken[] Parse(string expression);
    }
}
