using System;
using System.Linq;
using ScriptEngine.Business.Helpers;
using Shared.Models.Parser;
using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.OperationNodes;
using Shared.Models.Parser.Tokens;

namespace ScriptEngine.Business.Optimisations
{
    /// <summary>
    ///     Optimises squaring constants in branches, such as "3 * 3" to "**3"
    /// </summary>
    internal class SquareOptimisation
    {
        internal static AbstractSyntaxTree OptimiseSquareConstants(ref AbstractSyntaxTree tree)
        {
            if (tree == null)
                return null;

            if (!tree.Nodes.Any())
                return tree;

            try
            {
                OptimiseNode(ref tree, tree.Nodes[0]);
                return tree;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        private static void OptimiseNode(ref AbstractSyntaxTree tree, Node node)
        {
            if (node is TriadicNode triadicNode)
            {
                OptimiseNode(ref tree, triadicNode.Condition);
                OptimiseNode(ref tree, triadicNode.Lhs);
                OptimiseNode(ref tree, triadicNode.Rhs);
            }

            if (node is DiadicNode diadicNode)
            {
                OptimiseNode(ref tree, diadicNode.Lhs);
                OptimiseNode(ref tree, diadicNode.Rhs);
            }

            var newNode = TryOptimiseMultiplicationOperation(node);

            if (newNode != null)
                AstHelper.ReplaceNode(ref tree, node, newNode);

            //if (newNode != node)
            //    tree.Nodes[tree.Nodes.IndexOf(node)] = newNode;
        }

        private static Node TryOptimiseMultiplicationOperation(Node node)
        {
            return node is DiadicNode diadicNode
                   && diadicNode.Value.ToString() == CharacterSet.MULTIPLICATION
                   && diadicNode.Lhs is BaseNode
                   && diadicNode.Rhs is BaseNode
                   && !(diadicNode.Lhs is VariableNode)
                   && !(diadicNode.Rhs is VariableNode)
                   && diadicNode.Lhs.Value.ToString() == diadicNode.Rhs.Value.ToString()
                ? new UnaryNode(new Token(CharacterSet.SQUARE, TokenType.Operator), diadicNode.Lhs)
                : null;
        }
    }
}
