using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Models.Parser.Nodes
{
    public class AbstractSyntaxTree : ICloneable
    {
        public string Expression => Nodes.Any() ? Nodes[0]?.ToString() : throw new InvalidOperationException("Unable to get expression from AST - no nodes");

        public object Evaluated { get; set; } 

        public Node[] Nodes { get; set; }

        public AbstractSyntaxTree() { }

        public AbstractSyntaxTree(List<Node> nodes) => Nodes = nodes.ToArray();

        public object Evaluate()
        {
            Evaluated = Nodes.Length > 0 ? Nodes[0]?.Evaluate() : throw new InvalidOperationException("Unable to evaluate AST - no nodes");
            return Evaluated;
        }

        public object Clone() =>
            new AbstractSyntaxTree
            {
                Evaluated = Evaluated,
                Nodes = Nodes
            };
    }
}
