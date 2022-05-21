using ScriptEngine.Business.Operations;
using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.OperationNodes
{
    public class BitshiftNode : DiadicNode
    {
        public override object Evaluate() => Operations.Bitshift(this);

        public BitshiftNode() { }

        public BitshiftNode(Token token, Node lhs, Node rhs) : base(token, lhs, rhs) { }
    }
}
