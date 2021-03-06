using ScriptEngine.Business.Operations;
using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.OperationNodes
{
    public class AssignmentNode : DiadicNode
    {
        public override object Evaluate() => Operations.Assignment(this);

        public AssignmentNode() { }

        public AssignmentNode(Token token, Node lhs, Node rhs) : base(token, lhs, rhs) { }
    }
}
