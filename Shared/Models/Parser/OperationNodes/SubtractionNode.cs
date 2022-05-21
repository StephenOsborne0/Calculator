using ScriptEngine.Business.Operations;
using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.OperationNodes
{
    public class SubtractionNode : DiadicNode
    {
        public override object Evaluate() => Operations.Subtraction(this);

        public SubtractionNode() { }

        public SubtractionNode(Token token, Node lhs, Node rhs) : base(token, lhs, rhs) { }
    }
}
