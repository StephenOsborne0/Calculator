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
    ///     Optimises assignment operations in branches, such as "_a = _a + 1" to "_a += 1"
    /// </summary>
    public class AssignmentOptimisation
    {
        internal static AbstractSyntaxTree OptimiseAssignmentOperations(AbstractSyntaxTree tree)
        {
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

            var newNode = TryOptimiseAssignmentOperation(node);

            if (newNode != node)
                AstHelper.ReplaceNode(ref tree, node, newNode);
        }

        private static Node TryOptimiseAssignmentOperation(Node node)
        {
            if (node is DiadicNode diadicNode 
                && diadicNode.Lhs is VariableNode variableNode 
                && diadicNode.Rhs is DiadicNode operationNode 
                && operationNode.Lhs is VariableNode operationNodeLhs 
                && operationNodeLhs.Name == variableNode.Name 
                && IsValidAssignmentOperationNode(operationNode))
            {
                var tokenString = string.Concat(operationNode.Value.ToString(), CharacterSet.ASSIGNMENT);
                var newNode = new AssignmentNode(new Token(tokenString, TokenType.Operator), variableNode, operationNode.Rhs);
                return newNode;
            }

            return node;
        }

        private static bool IsValidAssignmentOperationNode(Node node) =>
            node.Value.ToString() == CharacterSet.ADDITION ||
            node.Value.ToString() == CharacterSet.SUBTRACTION ||
            node.Value.ToString() == CharacterSet.MULTIPLICATION ||
            node.Value.ToString() == CharacterSet.DIVISION ||
            node.Value.ToString() == CharacterSet.MODULUS ||
            node.Value.ToString() == CharacterSet.BITWISE_OR ||
            node.Value.ToString() == CharacterSet.BITWISE_AND;
    }
}
