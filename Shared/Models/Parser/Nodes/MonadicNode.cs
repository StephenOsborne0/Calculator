using System.Collections.Generic;
using System.Linq;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.Nodes
{
    public abstract class MonadicNode : Node
    {
        public virtual Node Lhs { get; set; }

        public override List<Node> GetAllNodes() => new List<Node> { this }.Concat(Lhs?.GetAllNodes()).ToList();

        public override string ToString()
        {
            var lhs = Lhs?.ToString();

            if (Lhs is DiadicNode || Lhs is TriadicNode)
                lhs = $"({lhs})";

            return $"{lhs} {Value}";
        }

        protected MonadicNode() { }

        protected MonadicNode(Token token, Node lhs) : base(token)
        {
            Lhs = lhs;
        }
    }
}
