using System.Collections.Generic;
using Shared.Models.Parser.Nodes;

namespace Shared.Models.Parser
{
    public class Expression
    {
        public string OriginalExpression { get; set; }

        public AbstractSyntaxTree[] AbstractSyntaxTrees { get; set; }

        public Expression() { }

        public Expression(string input) => OriginalExpression = input;
    }
}
