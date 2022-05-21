using ScriptEngine.Business.Operations;
using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.OperationNodes
{
    public class LogicalAndNode : DiadicNode
    {
        public override object Evaluate() => Operations.LogicalAnd(this);

        public LogicalAndNode() { }

        public LogicalAndNode(Token token, Node lhs, Node rhs) : base(token, lhs, rhs) { }
    }
}
