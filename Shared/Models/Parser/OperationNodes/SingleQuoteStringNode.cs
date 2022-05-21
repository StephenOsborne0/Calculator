using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.OperationNodes
{
    public class SingleQuoteStringNode : BaseNode
    {
        public override string ToString() => $"\'{Value}\'";

        public SingleQuoteStringNode() { }

        public SingleQuoteStringNode(Token token) : base(token)
        {
            Value = token.Value.ToString().Substring(1, token.Value.ToString().Length - 2);
        }
    }
}
