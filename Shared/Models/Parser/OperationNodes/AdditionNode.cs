using ScriptEngine.Business.Operations;
using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.OperationNodes
{
    public class AdditionNode : DiadicNode
    {
        public override object Evaluate() => Operations.Addition(this);

        public AdditionNode() { }

        public AdditionNode(Token token, Node lhs, Node rhs) : base(token, lhs, rhs) { }
    }
}
