using ScriptEngine.Business.Operations;
using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.OperationNodes
{
    public class UnaryNode : MonadicNode
    {
        public override object Evaluate() => Operations.Unary(this);

        public override string ToString()
        {
            var lhs = Lhs?.ToString();

            if (Lhs is DiadicNode || Lhs is TriadicNode)
                lhs = $"({lhs})";

            return $"{Value}{lhs}";
        }

        public UnaryNode() { }

        public UnaryNode(Token token, Node lhs) : base(token, lhs) { }
    }
}
