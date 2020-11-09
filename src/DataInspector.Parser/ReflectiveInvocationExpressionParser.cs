using System;
using System.Collections.Generic;
using DataInspector.Core.Expression;

namespace DataInspector.Parser {
    public class ReflectiveInvocationExpressionParser<TInputModel> : IExpressionParser<TInputModel>
        where TInputModel : class {
        public IParsedExpression Parse(string expression) {
            var tokens = expression.Split(".", StringSplitOptions.RemoveEmptyEntries);
            var parsedTokens = new List<QueryableToken>();

            for (var i = 0; i < tokens.Length; ++i) {
                var token = tokens[i];
                var isArrayExpression = token.EndsWith(']');

                if (isArrayExpression) {
                    var indexerStartPos = token.IndexOf('[');
                    var indexer = token.Substring(indexerStartPos + 1, token.Length - indexerStartPos - 2);
                    if (!int.TryParse(indexer, out var index)) {
                        throw new ArgumentException($"Invalid indexer in array expression: {token}");
                    }

                    var exprWithoutIndices = token.Substring(0, indexerStartPos);                    
                    var p = new QueryableToken(string.Join('.', exprWithoutIndices), true, index);
                    parsedTokens.Add(p);
                }
                else {
                    var p = new QueryableToken(token, false, -1);
                    parsedTokens.Add(p);
                }
            }

            var parsedExpr = new ReflectiveInvocationParsedExpression(parsedTokens.ToArray());
            return parsedExpr;
        }
    }
}
