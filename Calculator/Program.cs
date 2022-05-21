using System;
using System.Linq;
using ScriptEngine.Business.Parsers;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var str = Console.ReadLine();

                try
                {
                    var parser = new Parser();
                    var expression = parser.Parse(str);
                    var result = parser.Evaluate(expression);
                    Console.WriteLine(result.AbstractSyntaxTrees.First().Evaluated);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}