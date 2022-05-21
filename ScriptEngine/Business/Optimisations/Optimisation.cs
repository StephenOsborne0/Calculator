using System.Collections.Generic;
using Shared.Models.Parser.Nodes;

namespace ScriptEngine.Business.Optimisations
{
    /// <summary>
    ///     Handles the order of all optimisations
    /// </summary>
    public class Optimisation
    {
        public static List<AbstractSyntaxTree> OptimiseASTs(List<AbstractSyntaxTree> input)
        {
            var output = new List<AbstractSyntaxTree>();

            foreach (var tree in input)
                output.Add(OptimiseAST(tree));

            return output;
        }

        public static AbstractSyntaxTree OptimiseAST(AbstractSyntaxTree tree)
        {
            SquareOptimisation.OptimiseSquareConstants(ref tree);
            ConstantOptimisation.OptimiseConstants(ref tree);
            AssignmentOptimisation.OptimiseAssignmentOperations(tree);
            return tree;
        }
    }
}
