using System.Collections.Generic;
using System.Linq;
using DataInspector.Core.Expression;

namespace DataInspector.Parser {
    public class ReflectiveInvocationParsedExpression : IParsedExpression {
        public QueryableToken[] Tokens { get; }

        public bool IsValid => Tokens.Any();

        public bool IsArrayExpression => Tokens.Any(t => t.IsArrayExpression);

        public int[] ArrayIndicies => Tokens.Where(t1 => t1.IsArrayExpression).Select(t2 => t2.ArrayIndex).ToArray();

        public string LookUpKey => GetDALLookUpKey(Tokens);

        public ReflectiveInvocationParsedExpression(QueryableToken[] queryTokens) {
            Tokens = queryTokens;
        }

        // TEMP: Dupe code, temporary; will refactor later
        private static string GetDALLookUpKey(QueryableToken[] tokens) {
            var calls = new List<string>();
            foreach (var qt in tokens) {
                var o = qt.IsArrayExpression ? $"{qt.SubExpression}[]" : $"{qt.SubExpression}";
                calls.Add(o);
            }

            return string.Join('.', calls).ToLower();
        }
    }
}
