using ScriptEngine.Business.Operations;
using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.OperationNodes
{
    public class ComparisonNode : DiadicNode
    {
        public override object Evaluate() => Operations.Comparison(this);

        public ComparisonNode() { }

        public ComparisonNode(Token token, Node lhs, Node rhs) : base(token, lhs, rhs) { }
    }
}
