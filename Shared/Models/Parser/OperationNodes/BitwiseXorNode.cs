using ScriptEngine.Business.Operations;
using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.OperationNodes
{
    public class BitwiseXorNode : DiadicNode
    {
        public override object Evaluate() => Operations.BitwiseXor(this);

        public BitwiseXorNode() { }

        public BitwiseXorNode(Token token, Node lhs, Node rhs) : base(token, lhs, rhs) { }
    }
}
