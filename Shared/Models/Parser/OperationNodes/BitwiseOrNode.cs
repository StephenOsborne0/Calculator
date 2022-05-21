using ScriptEngine.Business.Operations;
using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.OperationNodes
{
    public class BitwiseOrNode : DiadicNode
    {
        public override object Evaluate() => Operations.BitwiseOr(this);

        public BitwiseOrNode() { }

        public BitwiseOrNode(Token token, Node lhs, Node rhs) : base(token, lhs, rhs) { }
    }
}
