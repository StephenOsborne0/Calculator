using ScriptEngine.Business.Operations;
using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.OperationNodes
{
    public class TernaryNode : TriadicNode
    {
        public override object Evaluate() => Operations.Ternary(this);

        public TernaryNode() { }

        public TernaryNode(Token token, Node condition, Node lhs, Node rhs) : base(token, condition, lhs, rhs) { }
    }
}
