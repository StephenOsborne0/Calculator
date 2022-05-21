using ScriptEngine.Business.Operations;
using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.OperationNodes
{
    public class EqualityNode : DiadicNode
    {
        public override object Evaluate() => Operations.Equality(this);

        public EqualityNode() { }

        public EqualityNode(Token token, Node lhs, Node rhs) : base(token, lhs, rhs) { }
    }
}
