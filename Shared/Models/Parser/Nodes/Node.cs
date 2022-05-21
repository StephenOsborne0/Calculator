using System.Collections.Generic;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.Nodes
{
    public class Node
    {
        public virtual Token Token { get; set; }

        public virtual object Value { get; set; }

        public virtual object Evaluate() => Value;

        public new virtual string ToString() => Value?.ToString();

        public virtual List<Node> GetAllNodes() => new List<Node> { this };

        protected Node() { }

        protected Node(Token token)
        {
            Token = token;
            Value = token.Value;
        }
    }
}
