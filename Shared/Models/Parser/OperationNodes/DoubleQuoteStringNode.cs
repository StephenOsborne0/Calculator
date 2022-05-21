using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.OperationNodes
{
    public class DoubleQuoteStringNode : BaseNode
    {
        public override string ToString() => $"\"{Value}\"";

        public DoubleQuoteStringNode() { }

        public DoubleQuoteStringNode(Token token) : base(token)
        {
            Value = token.Value.ToString().Substring(1, token.Value.ToString().Length - 2);
        }
    }
}
