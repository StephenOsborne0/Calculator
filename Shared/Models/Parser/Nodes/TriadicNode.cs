using System.Collections.Generic;
using System.Linq;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.Nodes
{
    public abstract class TriadicNode : Node
    {
        public virtual Node Condition { get; set; }

        public virtual Node Lhs { get; set; }

        public virtual Node Rhs { get; set; }

        public override List<Node> GetAllNodes() => new List<Node> { this }.Concat(Condition?.GetAllNodes()).Concat(Lhs?.GetAllNodes()).Concat(Rhs?.GetAllNodes()).ToList();

        public override string ToString()
        {
            var condition = Condition?.ToString();
            var lhs = Lhs?.ToString();
            var rhs = Rhs?.ToString();

            if (Condition is DiadicNode || Condition is TriadicNode)
                condition = $"({condition})";

            if (Lhs is DiadicNode || Lhs is TriadicNode)
                lhs = $"({lhs})";

            if (Rhs is DiadicNode || Lhs is TriadicNode)
                rhs = $"({rhs})";

            return $"{condition} {CharacterSet.TERNARY_CONDITIONAL} {lhs} {CharacterSet.TERNARY_BRANCH} {rhs}";
        }

        protected TriadicNode() { }

        protected TriadicNode(Token token, Node condition, Node lhs, Node rhs) : base(token)
        {
            Condition = condition;
            Lhs = lhs;
            Rhs = rhs;
        }
    }
}
