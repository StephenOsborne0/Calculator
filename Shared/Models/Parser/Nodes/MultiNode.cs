using System.Collections.Generic;
using System.Linq;
using Shared.Models.Parser.Tokens;

namespace Shared.Models.Parser.Nodes
{
    public abstract class MultiNode : Node
    {
        public virtual Node[] ChildNodes { get; set; }

        public override List<Node> GetAllNodes()
        {
            var nodes = new List<Node> {this};
            ChildNodes.ToList().ForEach(node => nodes.Concat(node.GetAllNodes()));
            return nodes.ToList();
        }

        public override string ToString()
        {
            var nodeValues = string.Join(", ", ChildNodes?.Select(x => x.Value.ToString()));
            return $"{Value} ({nodeValues})";
        }

        protected MultiNode() { }

        protected MultiNode(Token token, Node[] childNodes) : base(token) => ChildNodes = childNodes;
    }
}