using ScriptEngine.Business.Operations;
using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.OperationNodes
{
    public class BitwiseAndNode : DiadicNode
    {
        public override object Evaluate() => Operations.BitwiseAnd(this);

        public BitwiseAndNode() { }

        public BitwiseAndNode(Token token, Node lhs, Node rhs) : base(token, lhs, rhs) { }
    }
}
