using ScriptEngine.Business.Operations;
using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.OperationNodes
{
    public class DivisionNode : DiadicNode
    {
        public override object Evaluate() => Operations.Division(this);

        public DivisionNode() { }

        public DivisionNode(Token token, Node lhs, Node rhs) : base(token, lhs, rhs) { }
    }
}
