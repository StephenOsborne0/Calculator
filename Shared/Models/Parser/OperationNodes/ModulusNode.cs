using ScriptEngine.Business.Operations;
using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.OperationNodes
{
    public class ModulusNode : DiadicNode
    {
        public override object Evaluate() => Operations.Modulus(this);

        public ModulusNode() { }

        public ModulusNode(Token token, Node lhs, Node rhs) : base(token, lhs, rhs) { }
    }
}
