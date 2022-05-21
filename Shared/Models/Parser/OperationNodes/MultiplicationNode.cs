using ScriptEngine.Business.Operations;
using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.OperationNodes
{
    public class MultiplicationNode : DiadicNode
    {
        public override object Evaluate() => Operations.Multiplication(this);

        public MultiplicationNode() { }

        public MultiplicationNode(Token token, Node lhs, Node rhs) : base(token, lhs, rhs) { }
    }
}
