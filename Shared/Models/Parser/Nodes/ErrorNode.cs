using System.Collections.Generic;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.Nodes
{
    public class ErrorNode : Node
    {
        public override Token Token { get; set; }

        public override object Value { get; set; }

        public override object Evaluate() => Value.ToString();

        public override List<Node> GetAllNodes() => new List<Node> {this};

        public ErrorNode() { }

        public ErrorNode(string errorMessage) => Value = errorMessage;
    }
}
