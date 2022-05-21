using System.Collections.Generic;
using System.Linq;
using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.OperationNodes
{
    public class InstructionNode : MultiNode
    {
        public override object Evaluate()
        {
            var nodeValues = string.Join(", ", ChildNodes.Select(x => x.Evaluate().ToString()));
            return $"{Value} ({nodeValues})";
        }

        public override string ToString()
        {
            var nodeValues = string.Join(", ", ChildNodes.Select(x => x.ToString()));
            return $"{Value} ({nodeValues})";
        }

        public InstructionNode() { }

        public InstructionNode(Token token, List<Node> parameterNodes) : base(token, parameterNodes.ToArray()) { }
    }
}
