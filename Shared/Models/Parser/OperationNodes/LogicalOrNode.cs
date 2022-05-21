using ScriptEngine.Business.Operations;
using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.OperationNodes
{
    public class LogicalOrNode : DiadicNode
    {
        public override object Evaluate() => Operations.LogicalOr(this);

        public LogicalOrNode() { }

        public LogicalOrNode(Token token, Node lhs, Node rhs) : base(token, lhs, rhs) { }
    }
}
