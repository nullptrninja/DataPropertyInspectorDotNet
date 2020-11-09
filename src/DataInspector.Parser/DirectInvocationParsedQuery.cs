﻿using System.Collections.Generic;
using System.Linq;

namespace DataInspector.Core.Expression {
    public class DirectInvocationParsedQuery : IParsedQuery {
        public bool IsValid => Tokens.Any();

        public bool IsArrayExpression => Tokens.Any(t => t.IsArrayExpression && t.ArrayIndex >= 0);

        public int[] ArrayIndicies => Tokens.Select(q => q.ArrayIndex).ToArray();

        public string LookUpKey => GetDALLookUpKey(Tokens);

        private QueryableToken[] Tokens { get; }

        public DirectInvocationParsedQuery(QueryableToken[] queryTokens) {
            Tokens = queryTokens;
        }

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
