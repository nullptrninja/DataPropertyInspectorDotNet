namespace DataInspector.Core.Expression {
    /// <summary>
    /// The result of an <see cref="IExpressionParser"/>. Includes relevant metadata in order to execute the
    /// expression inside of an <see cref="IDataModelAccessLayer"/>.
    /// </summary>
    public interface IParsedQuery {
        //QueryableToken[] Tokens { get; }

        bool IsValid { get; }

        bool IsArrayExpression { get; }

        int[] ArrayIndicies {get;}

        string LookUpKey { get; }
    }
}
