using System.Collections.Generic;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.Nodes
{
    public class BaseNode : Node
    {
        public override Token Token { get; set; }

        public override object Value { get; set; }

        public override object Evaluate() => Value;

        public override string ToString() => Value?.ToString();

        public override List<Node> GetAllNodes() => new List<Node> {this};

        public BaseNode() { }

        public BaseNode(Token token) : base(token) { }
    }
}
