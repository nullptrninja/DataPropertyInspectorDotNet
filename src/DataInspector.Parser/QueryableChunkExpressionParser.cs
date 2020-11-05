using System;
using System.Collections.Generic;
using DataInspector.Core.Expression;

namespace DataInspector.Parser{
    public class QueryableChunkExpressionParser<TInputModel> : IExpressionParser<TInputModel>
        where TInputModel : class {

        /// <summary>
        /// Parses an expression into tokens for further processing. The expression can be a variable (single token)
        /// or an object with nested properties. For example:
        /// "age" (a single token variable)
        /// "rootObject.age" (two token property call chain)
        /// "rootObject.property1.property2" (three token property call chain)
        /// </summary>
        /// <param name="expression">Expression to parse</param>
        /// <returns>ParsedTokens with parsing information on each token</returns>
        public QueryableToken[] Parse(string expression) {
            var tokens = expression.ToLower().Split(".", StringSplitOptions.RemoveEmptyEntries);
            var parsedTokens = new List<QueryableToken>();
            var subExprBuffer = new List<string>();

            for (var i = 0; i < tokens.Length; ++i) {
                var token = tokens[i];
                var isArrayReference = token.EndsWith(']');

                if (isArrayReference) {
                    var indexerStartPos = token.IndexOf('[');
                    var indexer = token.Substring(indexerStartPos + 1, token.Length - indexerStartPos - 2);
                    if (!Int32.TryParse(indexer, out var index)) {
                        throw new ArgumentException($"Invalid indexer in array expression: {token}");
                    }

                    var subExprWithoutIndexValue = token.Substring(0, indexerStartPos);
                    subExprBuffer.Add($"{subExprWithoutIndexValue}[]");     // We strip out the index value because this will be our DAL lookup key

                    var p = new QueryableToken(string.Join('.', subExprBuffer), true, index);
                    parsedTokens.Add(p);
                    subExprBuffer.Clear();                    
                }
                else {
                    subExprBuffer.Add(token);
                }
            }

            // Add final aggregate subexpr if anything is left. It won't be an array in this case
            if (subExprBuffer.Count > 0) {
                var aggPt = new QueryableToken(string.Join('.', subExprBuffer), false, -1);
                parsedTokens.Add(aggPt);
            }

            return parsedTokens.ToArray();
        }
    }
}
