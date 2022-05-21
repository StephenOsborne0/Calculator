using System;
using System.Linq;
using ScriptEngine.Business.Analysers;
using Shared.Models.Parser;

namespace ScriptEngine.Business.Parsers
{
    public class Parser
    {
        public Parser() { }

        //public Parser(IEnumerable<ISerializableKeyValuePair> variables)
        //{
        //    VariableManager.ClearVariables();

        //    foreach (var variable in variables)
        //        VariableManager.AddVariable(variable.Key, variable.Value);
        //}

        public Expression Parse(string input)
        {
            var expression = new Expression(input);
            var tokens = new LexicalAnalyser(expression).ParseTokens();
            expression.AbstractSyntaxTrees = new SyntacticalAnalyser().GenerateExpressions(tokens).ToArray();
            return expression;
        }

        public Expression Evaluate(Expression expression)
        {
            if (expression.AbstractSyntaxTrees == null || expression.AbstractSyntaxTrees.Length == 0)
                throw new Exception("No abstract syntax trees to evaluate");

            expression.AbstractSyntaxTrees.ToList().ForEach(ast => ast.Evaluate());
            return expression;
        }

        //public List<ISerializableKeyValuePair> GetVariables() => VariableManager.GetVariables();

        //public void ClearVariables() => VariableManager.ClearVariables();
    }
}
