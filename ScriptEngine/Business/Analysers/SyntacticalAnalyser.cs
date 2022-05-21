using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Models.Parser;
using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.OperationNodes;
using Shared.Models.Parser.Tokens;

namespace ScriptEngine.Business.Analysers
{
    public class SyntacticalAnalyser
    {
        private List<Node> Nodes { get; set; }

        private List<Token> _tokens;

        private Token CurrentToken => _currentIndex < _tokens.Count ? _tokens[_currentIndex] : null;

        private Token NextToken => (_currentIndex + 1) < _tokens.Count ? _tokens[_currentIndex + 1] : null;

        private int _currentIndex;

        public List<AbstractSyntaxTree> GenerateExpressions(List<Token> tokens)
        {
            var expressions = new List<AbstractSyntaxTree>();
            var expressionTokens = new List<Token>();

            foreach (var currentToken in tokens)
            {
                if (currentToken.Type == TokenType.Eol)
                {
                    expressions.Add(GenerateExpression(expressionTokens));
                    expressionTokens = new List<Token>();
                }
                else
                {
                    expressionTokens.Add(currentToken);
                }
            }

            if (expressionTokens.Count > 0)
                expressions.Add(GenerateExpression(expressionTokens));

            return expressions;
        }

        public AbstractSyntaxTree GenerateExpression(List<Token> tokens)
        {
            try
            {
                _tokens = tokens.Where(t => t.Type != TokenType.Space && t.Type != TokenType.Tab).ToList();

                if (_tokens == null || _tokens.Count == 0)
                    return null;

                Nodes = new List<Node>();
                _currentIndex = 0;

                ExpressionString();

                Nodes.Reverse();

                return new AbstractSyntaxTree(Nodes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private void ConsumeToken()
        {
#if (DEBUG)
            //Console.WriteLine($"Consuming token: {CurrentToken?.Value}");
#endif
            _currentIndex++;
        }

        private void ReturnToken()
        {
#if (DEBUG)
            //Console.WriteLine("Returning token");
#endif
            _currentIndex--;
        }

        private Node AppendNode(Node node)
        {
            Nodes.Add(node);
            return node;
        }

        private void RemoveNode()
        {
            ReturnToken();
            Nodes.RemoveAt(Nodes.Count - 1);
        }

        private Node ExpressionString()
        {
            if (CurrentToken?.Type != TokenType.Instruction)
                return Expression();

            var token = CurrentToken;
            ConsumeToken();

            if (CurrentToken?.Value.ToString() != CharacterSet.LEFT_BRACKET)
                return new ErrorNode("INSTRUCTION: Failed to parse opening bracket for instruction parameters");

            ConsumeToken();

            var parameters = new List<Node>();
            Parameters(ref parameters);

            if (parameters.Count == 0)
                return new ErrorNode("INSTRUCTION: Failed to parse instruction parameters to nodes");

            if (CurrentToken?.Value.ToString() != CharacterSet.RIGHT_BRACKET)
                return new ErrorNode("INSTRUCTION: Failed to parse closing bracket for instruction parameters");

            ConsumeToken();

            return AppendNode(new InstructionNode(token, parameters));
        }

        private List<Node> Parameters(ref List<Node> parameters)
        {
            var expression = Expression();

            parameters.Add(expression ?? new ErrorNode("PARAMETERS: Failed to parse parameter for instruction"));

            if (CurrentToken?.Value.ToString() == CharacterSet.DELIMITER)
            {
                ConsumeToken();
                Parameters(ref parameters);
            }

            return parameters;
        }

        /// <summary>
        ///     Start of the order of precedence
        /// </summary>
        /// <returns></returns>
        private Node Expression() => AssignmentExpression();

        private Node AssignmentExpression()
        {
            var node = TernaryExpression();

            if (CurrentToken?.Type == TokenType.Operator
                && CurrentToken?.Value.ToString() == CharacterSet.ASSIGNMENT
                || CurrentToken?.Value.ToString() == CharacterSet.ASSIGNMENT_ADDITION
                || CurrentToken?.Value.ToString() == CharacterSet.ASSIGNMENT_SUBTRACTION
                || CurrentToken?.Value.ToString() == CharacterSet.ASSIGNMENT_MULTIPLICATION
                || CurrentToken?.Value.ToString() == CharacterSet.ASSIGNMENT_DIVISION
                || CurrentToken?.Value.ToString() == CharacterSet.ASSIGNMENT_MODULUS
                || CurrentToken?.Value.ToString() == CharacterSet.ASSIGNMENT_BITWISE_AND
                || CurrentToken?.Value.ToString() == CharacterSet.ASSIGNMENT_BITWISE_OR
                || CurrentToken?.Value.ToString() == CharacterSet.ASSIGNMENT_BITWISE_XOR
                || CurrentToken?.Value.ToString() == CharacterSet.ASSIGNMENT_LOGICAL_AND
                || CurrentToken?.Value.ToString() == CharacterSet.ASSIGNMENT_LOGICAL_OR)
            {
                var token = CurrentToken;
                ConsumeToken();

                var lhs = node;
                var rhs = AssignmentExpression();

                return rhs == null
                    ? AppendNode(new ErrorNode("ASSIGNMENT: Failed to parse assignment expression to nodes"))
                    : AppendNode(new AssignmentNode(token, lhs, rhs));
            }

            return node;
        }

        private Node TernaryExpression()
        {
            var node = LogicalOrExpression();

            if (CurrentToken?.Type == TokenType.TernaryConditional
                && CurrentToken?.Value.ToString() == CharacterSet.TERNARY_CONDITIONAL)
            {
                var token = CurrentToken;
                ConsumeToken();

                var condition = node;
                var lhs = Expression();

                if (CurrentToken?.Type == TokenType.TernaryBranch
                    && CurrentToken?.Value.ToString() == CharacterSet.TERNARY_BRANCH)
                {
                    ConsumeToken();

                    var rhs = Expression();

                    return lhs == null || rhs == null
                        ? AppendNode(new ErrorNode("TERNARY: Failed to parse ternary branch expression to nodes"))
                        : AppendNode(new TernaryNode(token, condition, lhs, rhs));
                }
                return AppendNode(new ErrorNode("TERNARY: Failed to parse ternary condition expression to nodes"));
            }

            return node;
        }

        private Node LogicalOrExpression()
        {
            var node = LogicalAndExpression();

            if (CurrentToken?.Type == TokenType.Operator
                && CurrentToken?.Value.ToString() == CharacterSet.LOGICAL_OR)
            {
                var token = CurrentToken;
                ConsumeToken();

                var lhs = node;
                var rhs = LogicalOrExpression();

                return rhs == null
                    ? AppendNode(new ErrorNode("LOGICAL OR: Failed to parse logical or expression to nodes"))
                    : AppendNode(new LogicalOrNode(token, lhs, rhs));
            }

            return node;
        }

        private Node LogicalAndExpression()
        {
            var node = BitwiseOrExpression();

            if (CurrentToken?.Type == TokenType.Operator
                && CurrentToken?.Value.ToString() == CharacterSet.LOGICAL_AND)
            {
                var token = CurrentToken;
                ConsumeToken();

                var lhs = node;
                var rhs = LogicalAndExpression();

                return rhs == null
                    ? AppendNode(new ErrorNode("LOGICAL AND: Failed to parse logical and expression to nodes"))
                    : AppendNode(new LogicalAndNode(token, lhs, rhs));
            }

            return node;
        }

        private Node BitwiseOrExpression()
        {
            var node = BitwiseXorExpression();

            if (CurrentToken?.Type == TokenType.Operator
                && CurrentToken?.Value.ToString() == CharacterSet.BITWISE_OR)
            {
                var token = CurrentToken;
                ConsumeToken();

                var lhs = node;
                var rhs = BitwiseOrExpression();

                return rhs == null
                    ? AppendNode(new ErrorNode("BITWISE OR: Failed to parse bitwise or expression to nodes"))
                    : AppendNode(new BitwiseOrNode(token, lhs, rhs));
            }

            return node;
        }

        private Node BitwiseXorExpression()
        {
            var node = BitwiseAndExpression();

            if (CurrentToken?.Type == TokenType.Operator
                && CurrentToken?.Value.ToString() == CharacterSet.BITWISE_XOR)
            {
                var token = CurrentToken;
                ConsumeToken();

                var lhs = node;
                var rhs = BitwiseXorExpression();

                return rhs == null
                    ? AppendNode(new ErrorNode("BITWISE XOR: Failed to parse bitwise xor expression to nodes"))
                    : AppendNode(new BitwiseXorNode(token, lhs, rhs));
            }

            return node;
        }

        private Node BitwiseAndExpression()
        {
            var node = EqualityExpression();

            if (CurrentToken?.Type == TokenType.Operator
                && CurrentToken?.Value.ToString() == CharacterSet.BITWISE_AND)
            {
                var token = CurrentToken;
                ConsumeToken();

                var lhs = node;
                var rhs = BitwiseAndExpression();

                return rhs == null
                    ? AppendNode(new ErrorNode("BITWISE AND: Failed to parse bitwise and expression to nodes"))
                    : AppendNode(new BitwiseAndNode(token, lhs, rhs));
            }

            return node;
        }

        private Node EqualityExpression()
        {
            var node = ComparisonExpression();

            if (CurrentToken?.Type == TokenType.Operator
                && CurrentToken?.Value.ToString() == CharacterSet.EQUALITY
                || CurrentToken?.Value.ToString() == CharacterSet.NOT_EQUAL
                || CurrentToken?.Value.ToString() == CharacterSet.NOT_EQUAL)
            {
                var token = CurrentToken;
                ConsumeToken();

                var lhs = node;
                var rhs = ComparisonExpression();

                return rhs == null
                    ? AppendNode(new ErrorNode("COMPARISON: Failed to parse equality expression to nodes"))
                    : AppendNode(new EqualityNode(token, lhs, rhs));
            }

            return node;
        }

        private Node ComparisonExpression()
        {
            var node = BitshiftExpression();

            if (CurrentToken?.Type == TokenType.Operator
                && CurrentToken?.Value.ToString() == CharacterSet.LESS_THAN
                || CurrentToken?.Value.ToString() == CharacterSet.GREATER_THAN
                || CurrentToken?.Value.ToString() == CharacterSet.LESS_THAN_EQUAL
                || CurrentToken?.Value.ToString() == CharacterSet.GREATER_THAN_EQUAL)
            {
                var token = CurrentToken;
                ConsumeToken();

                var lhs = node;
                var rhs = ComparisonExpression();

                return rhs == null
                    ? AppendNode(new ErrorNode("COMPARISON: Failed to parse comparison expression to nodes"))
                    : AppendNode(new ComparisonNode(token, lhs, rhs));
            }

            return node;
        }

        private Node BitshiftExpression()
        {
            var node = AdditionExpression();

            if (CurrentToken?.Type == TokenType.Operator
                && CurrentToken?.Value.ToString() == CharacterSet.BITSHIFT_LEFT
                || CurrentToken?.Value.ToString() == CharacterSet.BITSHIFT_RIGHT)
            {
                var token = CurrentToken;
                ConsumeToken();

                var lhs = node;
                var rhs = BitshiftExpression();

                return rhs == null
                    ? AppendNode(new ErrorNode("BITSHIFT: Failed to parse bitshift expression to nodes"))
                    : AppendNode(new BitshiftNode(token, lhs, rhs));
            }

            return node;
        }

        private Node AdditionExpression()
        {
            var node = MultiplicationExpression();

            if (CurrentToken?.Type == TokenType.Operator
                && CurrentToken?.Value.ToString() == CharacterSet.ADDITION)
            {
                var token = CurrentToken;
                ConsumeToken();

                var lhs = node;
                var rhs = AdditionExpression();

                return rhs == null
                    ? AppendNode(new ErrorNode("ADDITION: Failed to parse addition expression to nodes"))
                    : AppendNode(new AdditionNode(token, lhs, rhs));
            }

            if (CurrentToken?.Type == TokenType.Operator
                && CurrentToken?.Value.ToString() == CharacterSet.SUBTRACTION)
            {
                var token = CurrentToken;
                ConsumeToken();

                var lhs = node;
                var rhs = AdditionExpression();

                return rhs == null
                    ? AppendNode(new ErrorNode("ADDITION: Failed to parse subtraction expression to nodes"))
                    : AppendNode(new SubtractionNode(token, lhs, rhs));
            }

            return node;
        }

        private Node MultiplicationExpression()
        {
            var node = UnaryExpression();

            if (CurrentToken?.Type == TokenType.Operator
                && CurrentToken?.Value.ToString() == CharacterSet.DIVISION)
            {
                var token = CurrentToken;
                ConsumeToken();

                var lhs = node;
                var rhs = MultiplicationExpression();

                return rhs == null
                    ? AppendNode(new ErrorNode("MULTIPLICATION: Failed to parse division expression to nodes"))
                    : AppendNode(new DivisionNode(token, lhs, rhs));
            }

            if (CurrentToken?.Type == TokenType.Operator
                && CurrentToken?.Value.ToString() == CharacterSet.MULTIPLICATION)
            {
                var token = CurrentToken;
                ConsumeToken();

                var lhs = node;
                var rhs = MultiplicationExpression();

                return rhs == null
                    ? AppendNode(new ErrorNode("MULTIPLICATION: Failed to parse multiplication expression to nodes"))
                    : AppendNode(new MultiplicationNode(token, lhs, rhs));
            }

            if (CurrentToken?.Type == TokenType.Operator
                && CurrentToken?.Value.ToString() == CharacterSet.MODULUS)
            {
                var token = CurrentToken;
                ConsumeToken();

                var lhs = node;
                var rhs = MultiplicationExpression();

                return rhs == null
                    ? AppendNode(new ErrorNode("MULTIPLICATION: Failed to parse modulus expression to nodes"))
                    : AppendNode(new ModulusNode(token, lhs, rhs));
            }

            return node;
        }

        private Node UnaryExpression()
        {
            if (CurrentToken?.Type == TokenType.Bracket
                && CurrentToken?.Value.ToString() == CharacterSet.LEFT_BRACKET)
            {
                ConsumeToken();

                var lhs = Expression();

                if (lhs != null && CurrentToken.Value.ToString() == CharacterSet.RIGHT_BRACKET)
                {
                    ConsumeToken();
                    return lhs;
                }

                return AppendNode(new ErrorNode("UNARY: Failed to parse bracketed expression as nodes"));
            }

            if (CurrentToken?.Type == TokenType.SquareBracket
                && CurrentToken?.Value.ToString() == CharacterSet.LEFT_SQUARE_BRACKET)
            {
                ConsumeToken();

                var lhs = Expression();

                if (lhs != null && CurrentToken.Value.ToString() == CharacterSet.RIGHT_SQUARE_BRACKET)
                {
                    ConsumeToken();
                    return lhs;
                }

                return AppendNode(new ErrorNode("UNARY: Failed to parse square bracketed expression as nodes"));
            }

            if (CurrentToken?.Type == TokenType.Brace
                && CurrentToken?.Value.ToString() == CharacterSet.LEFT_BRACE)
            {
                ConsumeToken();

                var lhs = Expression();

                if (lhs != null && CurrentToken.Value.ToString() == CharacterSet.RIGHT_BRACE)
                {
                    ConsumeToken();
                    return lhs;
                }

                return AppendNode(new ErrorNode("UNARY: Failed to parse braced expression as nodes"));
            }

            if (CurrentToken?.Type == TokenType.Operator
                && CurrentToken?.Value.ToString() == CharacterSet.NOT
                || CurrentToken?.Value.ToString() == CharacterSet.ADDITION
                || CurrentToken?.Value.ToString() == CharacterSet.SUBTRACTION
                || CurrentToken?.Value.ToString() == CharacterSet.SQUARE_ROOT
                || CurrentToken?.Value.ToString() == CharacterSet.ONES_COMPLEMENT
                || CurrentToken?.Value.ToString() == CharacterSet.INCREMENT
                || CurrentToken?.Value.ToString() == CharacterSet.DECREMENT
                || CurrentToken?.Value.ToString() == CharacterSet.SQUARE)
            {
                var token = CurrentToken;
                ConsumeToken();

                var lhs = UnaryExpression();

                return lhs == null
                    ? AppendNode(new ErrorNode("MULTIPLICATION: Failed to parse multiplication expression to nodes"))
                    : AppendNode(new UnaryNode(token, lhs));
            }

            return BaseExpression();
        }

        private Node BaseExpression()
        {
            if (CurrentToken?.Type == TokenType.Integer
                || CurrentToken?.Type == TokenType.Float
                || CurrentToken?.Type == TokenType.Hexadecimal
                || CurrentToken?.Type == TokenType.Octal
                || CurrentToken?.Type == TokenType.Binary
                || CurrentToken?.Type == TokenType.Boolean)
            {
                var token = CurrentToken;
                ConsumeToken();
                return AppendNode(new BaseNode(token));
            }

            if (CurrentToken?.Type == TokenType.Variable)
            {
                var token = CurrentToken;
                ConsumeToken();
                return AppendNode(new VariableNode(token));
            }

            if (CurrentToken?.Type == TokenType.DoubleQuoteString)
            {
                var token = CurrentToken;
                ConsumeToken();
                return AppendNode(new DoubleQuoteStringNode(token));
            }

            if (CurrentToken?.Type == TokenType.SingleQuoteString)
            {
                var token = CurrentToken;
                ConsumeToken();
                return AppendNode(new SingleQuoteStringNode(token));
            }

            ConsumeToken();
            return AppendNode(new ErrorNode("BASE: Failed to parse token to node"));
        }
    }
}
