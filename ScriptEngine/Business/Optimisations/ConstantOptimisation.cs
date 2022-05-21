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
    ///     Optimises simple constant values in branches, such as "3 + 5" to "8"
    /// </summary>
    internal class ConstantOptimisation
    {
        internal static AbstractSyntaxTree OptimiseConstants(ref AbstractSyntaxTree tree)
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

            if (node is MonadicNode monadicNode)
                OptimiseNode(ref tree, monadicNode.Lhs);

            var newNode = TryOptimiseConstants(node);

            if (newNode != null)
                AstHelper.ReplaceNode(ref tree, node, newNode);
        }

        private static Node TryOptimiseConstants(Node node)
        {
            switch (node)
            {
                case DiadicNode diadicNode when diadicNode.Lhs is BaseNode 
                                                 && diadicNode.Rhs is BaseNode 
                                                 && !(diadicNode.Lhs is VariableNode) 
                                                 && !(diadicNode.Rhs is VariableNode):
                {
                    var resultingValue = diadicNode.Evaluate();
                    var newNode = new BaseNode(new Token(resultingValue, TypeHelper.GetTokenTypeForValue(resultingValue)));
                    return newNode;
                }

                case MonadicNode monadicNode when monadicNode.Lhs is BaseNode 
                                                   && monadicNode.Value.ToString() == CharacterSet.ADDITION:
                {
                    var resultingValue = monadicNode.Lhs.Evaluate();
                    var newNode = new BaseNode(new Token(resultingValue, TypeHelper.GetTokenTypeForValue(resultingValue)));
                    return newNode;
                }
            }

            return null;
        }

        //private static INode MergeConstants(ref AbstractSyntaxTree tree, INode node)
        //{
        //    object resultingValue = null;

        //    switch (node)
        //    {
        //        case ITriadicNode triadicNode:
        //        {
        //            MergeConstants(ref tree, triadicNode.Condition);
        //            MergeConstants(ref tree, triadicNode.Lhs);
        //            MergeConstants(ref tree, triadicNode.Rhs);

        //            var evaluatedCondition = bool.Parse(triadicNode.Condition.Evaluate().ToString());

        //            if (triadicNode.Lhs is Node && triadicNode.Rhs is Node)
        //            {
        //                resultingValue = evaluatedCondition ? triadicNode.Lhs.Evaluate() : triadicNode.Rhs.Evaluate();
        //                AstHelper.DeleteNode(ref tree, triadicNode.Condition);
        //                AstHelper.DeleteNode(ref tree, triadicNode.Lhs);
        //                AstHelper.DeleteNode(ref tree, triadicNode.Rhs);
        //            }

        //            break;
        //        }

        //        case IDiadicNode diadicNode:
        //        {
        //            diadicNode.Lhs = MergeConstants(ref tree, diadicNode.Lhs);
        //            diadicNode.Rhs = MergeConstants(ref tree, diadicNode.Rhs);

        //            if (diadicNode.Lhs is Node 
        //                && diadicNode.Rhs is Node 
        //                && !(diadicNode.Lhs is VariableNode) 
        //                && !(diadicNode.Rhs is VariableNode))
        //            {
        //                resultingValue = diadicNode.Evaluate();
        //                AstHelper.DeleteNode(ref tree, diadicNode.Lhs);
        //                AstHelper.DeleteNode(ref tree, diadicNode.Rhs);
        //            }
        //            break;
        //        }

        //        case IMonadicNode monadicNode:
        //        {
        //            MergeConstants(ref tree, monadicNode.Lhs);

        //            if (monadicNode.Lhs is Node && monadicNode.Value.ToString() == CharacterSet.ADDITION)
        //            {
        //                resultingValue = monadicNode.Lhs.Evaluate();
        //                AstHelper.DeleteNode(ref tree, monadicNode.Lhs);
        //            }
        //            break;
        //        }
        //    }

        //    if (resultingValue != null)
        //    {
        //        var newNode = new Node(new Token(resultingValue, TypeHelper.GetTokenTypeForValue(resultingValue)));
        //        tree.Nodes[tree.Nodes.IndexOf(node)] = newNode;
        //        return newNode;
        //    }

        //    return node;
        //}
    }
}
