namespace DataInspector.Core.Expression {
    public class QueryableToken {
        public string Subexpression { get; }

        public bool IsArrayExpression { get; }

        public int ArrayIndex { get; }

        public QueryableToken(string subexpression) : this(subexpression, false, -1) { }

        public QueryableToken(string subexpression, bool doesExpressionResolveToArray, int arrayIndex) {
            this.Subexpression = subexpression;
            this.IsArrayExpression = doesExpressionResolveToArray;
            this.ArrayIndex = arrayIndex;
        }
    }
}
