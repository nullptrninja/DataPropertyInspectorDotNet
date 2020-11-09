namespace DataInspector.Core.Expression {
    /// <summary>
    /// Represents a queryable-chunk. In most cases there will only be one of these from an expression but parsers may generate
    /// multiple QueryableTokens based on querying/DAL requirements such as arrays or multi-query strategies.
    /// </summary>
    public class QueryableToken {
        public string SubExpression { get; }

        public bool IsArrayExpression { get; }

        public int ArrayIndex { get; }

        public QueryableToken(string subexpression, bool doesExpressionAccessAnArray, int arrayIndex) {
            SubExpression = subexpression;
            IsArrayExpression = doesExpressionAccessAnArray;
            ArrayIndex = arrayIndex;
        }
    }
}
