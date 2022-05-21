using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.Nodes
{
    public abstract class DiadicNode : Node
    {
        public virtual Node Lhs { get; set; }

        public virtual Node Rhs { get; set; }

        public override List<Node> GetAllNodes() => new List<Node> {this}.Concat(Lhs?.GetAllNodes()).Concat(Rhs?.GetAllNodes()).ToList();

        public override string ToString()
        {
            var lhs = Lhs?.ToString();
            var rhs = Rhs?.ToString();

            if (Lhs is DiadicNode || Lhs is TriadicNode)
                lhs = $"({lhs})";

            if (Rhs is DiadicNode || Lhs is TriadicNode)
                rhs = $"({rhs})";

            return $"{lhs} {Value} {rhs}";
        }

        protected DiadicNode() { }

        protected DiadicNode(Token token, Node lhs, Node rhs) : base(token)
        {
            Lhs = lhs;
            Rhs = rhs;
        }
    }
}
