using System.Linq;
using Shared.Models.Parser.Nodes;

namespace ScriptEngine.Business.Helpers
{
    public class AstHelper
    {
        public static void DeleteNode(ref AbstractSyntaxTree tree, Node node)
        {
            switch (node)
            {
                case TriadicNode triadicNode:
                {
                    DeleteNode(ref tree, triadicNode.Condition);
                    DeleteNode(ref tree, triadicNode.Lhs);
                    DeleteNode(ref tree, triadicNode.Rhs);
                    break;
                }

                case DiadicNode diadicNode:
                {
                    DeleteNode(ref tree, diadicNode.Lhs);
                    DeleteNode(ref tree, diadicNode.Rhs);
                    break;
                }

                case MonadicNode monadicNode:
                {
                    DeleteNode(ref tree, monadicNode.Lhs);
                    break;
                }
            }

            tree.Nodes.ToList().Remove(node);
        }

        public static void ReplaceNode(ref AbstractSyntaxTree tree, Node nodeA, Node nodeB)
        {
            switch (nodeA)
            {
                case TriadicNode triadicNode:
                {
                    DeleteNode(ref tree, triadicNode.Condition);
                    DeleteNode(ref tree, triadicNode.Lhs);
                    DeleteNode(ref tree, triadicNode.Rhs);
                    break;
                }

                case DiadicNode diadicNode:
                {
                    DeleteNode(ref tree, diadicNode.Lhs);
                    DeleteNode(ref tree, diadicNode.Rhs);
                    break;
                }

                case MonadicNode monadicNode:
                {
                    DeleteNode(ref tree, monadicNode.Lhs);
                    break;
                }
            }

            tree.Nodes.ToList().InsertRange(tree.Nodes.ToList().IndexOf(nodeA), nodeB.GetAllNodes().AsEnumerable());
            ReplaceNodeReferences(ref tree, nodeA, nodeB);
            tree.Nodes.ToList().Remove(nodeA);
        }

        private static void ReplaceNodeReferences(ref AbstractSyntaxTree tree, Node nodeA, Node nodeB)
        {
            foreach (var node in tree.Nodes)
            {
                switch (node)
                {
                    case TriadicNode triadicNode:
                    {
                        if (triadicNode.Condition == nodeA)
                            triadicNode.Condition = nodeB;
                        if (triadicNode.Lhs == nodeA)
                            triadicNode.Lhs = nodeB;
                        if (triadicNode.Rhs == nodeA)
                            triadicNode.Rhs = nodeB;
                        break;
                    }

                    case DiadicNode diadicNode:
                    {
                        if (diadicNode.Lhs == nodeA)
                            diadicNode.Lhs = nodeB;
                        if (diadicNode.Rhs == nodeA)
                            diadicNode.Rhs = nodeB;
                        break;
                    }

                    case MonadicNode monadicNode:
                    {
                        if (monadicNode.Lhs == nodeA)
                            monadicNode.Lhs = nodeB;
                        break;
                    }
                }
            }
        }
    }
}
