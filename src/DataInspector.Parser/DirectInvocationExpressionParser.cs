using System;
using System.Collections.Generic;
using DataInspector.Core.Expression;

namespace DataInspector.Parser{
    public class DirectInvocationExpressionParser<TInputModel> : IExpressionParser<TInputModel>
        where TInputModel : class {

        /// <summary>
        /// Parses an expression for processing using a "direct invocation" strategy; the entire expression is broken up
        /// into sub-queries in order to determine if they are direct property calls or array index accesses. Queries that
        /// are all-properties will yield a single QueryableToken with the full call chain. For expressions that include array
        /// accessors, this will yield multiple QueryableTokens for each sub-expression. Each token will include the array index
        /// that was specified for that sub-expression.
        /// 
        /// Supported expression formats include (the root object is implied here):
        /// "age" (two token property call chain)
        /// "property1.property2" (three token property call chain)
        /// "property1.arrayProperty2[INDEX0] (three token property with array indexer call chain)
        /// "arrayProperty1[INDEX0].arrayProperty2[INDEX1].Id (nested array indexer call chain)
        /// "arrayProperty1[INDEX0] (any array indexer IF the array is a primitive data type)
        /// </summary>
        /// <param name="expression">Expression to parse</param>
        /// <returns>ParsedQuery with parsing information</returns>
        public IParsedQuery Parse(string expression) {
            var tokens = expression.Split(".", StringSplitOptions.RemoveEmptyEntries);
            var parsedTokens = new List<QueryableToken>();
            var subExprBuffer = new List<string>(tokens.Length);

            for (var i = 0; i < tokens.Length; ++i) {
                var token = tokens[i];
                var isArrayExpression = token.EndsWith(']');

                if (isArrayExpression) {
                    var indexerStartPos = token.IndexOf('[');
                    var indexer = token.Substring(indexerStartPos + 1, token.Length - indexerStartPos - 2);
                    if (!int.TryParse(indexer, out var index)) {
                        throw new ArgumentException($"Invalid indexer in array expression: {token}");
                    }

                    var subExprWithoutIndexValue = token.Substring(0, indexerStartPos);
                    subExprBuffer.Add(subExprWithoutIndexValue);     // We strip out the index value because this will be our DAL lookup key

                    var p = new QueryableToken(string.Join('.', subExprBuffer), true, index);
                    parsedTokens.Add(p);
                    subExprBuffer.Clear();                    
                }
                else {
                    subExprBuffer.Add(token);
                }
            }

            // Add final aggregate subexpr if anything is left. It won't be an array in this case since the array-check
            // within the loop would've already cleared it out if it was.
            if (subExprBuffer.Count > 0) {
                var aggPt = new QueryableToken(string.Join('.', subExprBuffer), false, -1);
                parsedTokens.Add(aggPt);
            }

            var result = new DirectInvocationParsedQuery(parsedTokens.ToArray());
            return result;
        }
    }
}
