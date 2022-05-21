using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptEngine.Business.Helpers;
using Shared.Models.Parser;
using Shared.Models.Parser.Tokens;

namespace ScriptEngine.Business.Analysers
{
    public class LexicalAnalyser
    {
        private readonly string _input;
        private int _currentIndex;
        private List<Token> _tokens;
        private Token _currentToken;
        private Token _nextToken;

        public LexicalAnalyser(Expression expression) => _input = expression.OriginalExpression;

        public List<Token> ParseTokens()
        {
            _tokens = new List<Token>();
            _currentIndex = 0;

            try
            {
                while (_currentIndex < _input.Length)
                {
                    var token = TryGetNextToken() ?? 
                                throw new InvalidOperationException($"Failed to parse character: {_input[_currentIndex]} at index {_currentIndex}");
                    _tokens.Add(token);
                    _currentIndex++;
                }

                return RefineTokens(_tokens);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                Console.WriteLine(invalidOperationException.Message);
                return null;
            }
        }

        private Token TryGetNextToken()
        {
            var c = _input[_currentIndex];

            if (CharacterHelper.IsNewlineChar(c))
                return new Token(c, BasicTokenType.Newline);

            if (CharacterHelper.IsWhitespaceChar(c))
                return new Token(c, BasicTokenType.Whitespace);

            if (CharacterHelper.IsNumericChar(c))
                return new Token(c, BasicTokenType.Numeric);

            if (CharacterHelper.IsAlphaChar(c))
                return new Token(c, BasicTokenType.Alpha);

            return CharacterHelper.IsSymbol(c) ? new Token(c, BasicTokenType.Symbol) : null;
        }

        public List<Token> RefineTokens(List<Token> tokens)
        {
            _tokens = tokens;
            var refinedTokens = new List<Token>();
            _currentIndex = 0;

            while (_currentIndex < _tokens.Count)
            {
                var token = TryGetToken() ?? throw new InvalidOperationException($"Failed to generate valid token at index: {_currentIndex}");
                refinedTokens.Add(token);
                _currentIndex++;
            }

            VerifyTokenValidity(refinedTokens);
            FormatTokens(refinedTokens);
            return refinedTokens;
        }

        private void RefreshCurrentToken() => _currentToken = _tokens[_currentIndex];

        private void RefreshNextToken() => _nextToken = _currentIndex + 1 < _tokens.Count ? _tokens[_currentIndex + 1] : null;

        private Token TryGetToken()
        {
            var sb = new StringBuilder();
            var tokenType = TokenType.None;
            var foundToken = false;

            RefreshCurrentToken();

            while (_currentIndex < _tokens.Count)
            {
                RefreshCurrentToken();
                RefreshNextToken();

                if (_currentToken == null)
                    throw new ArgumentException($"Token value cannot be null, at index: {_currentIndex}");

                var foundTokenCount = 0;

                switch (tokenType)
                {
                    //If type is currently unknown, try to guess type
                    case TokenType.None:
                        {
                            sb = new StringBuilder();

                            //Check for 2 character tokens first - we can skip a lot of work
                            var bothTokenValues = _currentToken.Value + _nextToken?.Value.ToString();

                            switch (bothTokenValues)
                            {
                                case CharacterSet.ASSIGNMENT_LOGICAL_AND:
                                case CharacterSet.ASSIGNMENT_LOGICAL_OR:
                                    tokenType = TokenType.Operator;
                                    foundToken = true;
                                    foundTokenCount = 3;
                                    break;
                                case CharacterSet.ASSIGNMENT_ADDITION:
                                case CharacterSet.ASSIGNMENT_SUBTRACTION:
                                case CharacterSet.ASSIGNMENT_MULTIPLICATION:
                                case CharacterSet.ASSIGNMENT_DIVISION:
                                case CharacterSet.ASSIGNMENT_MODULUS:
                                case CharacterSet.ASSIGNMENT_ONES_COMPLEMENT:
                                case CharacterSet.ASSIGNMENT_BITWISE_AND:
                                case CharacterSet.ASSIGNMENT_BITWISE_OR:
                                case CharacterSet.ASSIGNMENT_BITWISE_XOR:
                                case CharacterSet.LOGICAL_OR:
                                case CharacterSet.LOGICAL_AND:
                                case CharacterSet.INCREMENT:
                                case CharacterSet.DECREMENT:
                                case CharacterSet.SQUARE:
                                case CharacterSet.SQUARE_ROOT:
                                case CharacterSet.EQUALITY:
                                case CharacterSet.LESS_THAN_EQUAL:
                                case CharacterSet.GREATER_THAN_EQUAL:
                                case CharacterSet.NOT_EQUAL:
                                case CharacterSet.NOT_EQUAL2:
                                case CharacterSet.BITSHIFT_LEFT:
                                case CharacterSet.BITSHIFT_RIGHT:
                                    tokenType = TokenType.Operator;
                                    foundToken = true;
                                    foundTokenCount = 2;
                                    break;
                                default:

                                    foundTokenCount = 1;

                                    //Then single character tokens
                                    switch (_currentToken.Value.ToString())
                                    {
                                        case CharacterSet.ADDITION:
                                        case CharacterSet.SUBTRACTION:
                                        case CharacterSet.MULTIPLICATION:
                                        case CharacterSet.DIVISION:
                                        case CharacterSet.MODULUS:
                                        case CharacterSet.BITWISE_AND:
                                        case CharacterSet.BITWISE_OR:
                                        case CharacterSet.BITWISE_XOR:
                                        case CharacterSet.ONES_COMPLEMENT:
                                        case CharacterSet.GREATER_THAN:
                                        case CharacterSet.LESS_THAN:
                                        case CharacterSet.ASSIGNMENT:
                                        case CharacterSet.NOT:
                                            tokenType = TokenType.Operator;
                                            foundToken = true;
                                            break;
                                        case CharacterSet.EOL:
                                            tokenType = TokenType.Eol;
                                            foundToken = true;
                                            break;
                                        case CharacterSet.NEWLINE:
                                            tokenType = TokenType.NewLine;
                                            foundToken = true;
                                            break;
                                        case CharacterSet.CARRIAGE_RETURN:
                                            tokenType = TokenType.CarriageReturn;
                                            foundToken = true;
                                            break;
                                        case CharacterSet.TAB:
                                            tokenType = TokenType.Tab;
                                            foundToken = true;
                                            break;
                                        case CharacterSet.SPACE:
                                            tokenType = TokenType.Space;
                                            foundToken = true;
                                            break;
                                        case CharacterSet.TERNARY_CONDITIONAL:
                                            tokenType = TokenType.TernaryConditional;
                                            foundToken = true;
                                            break;
                                        case CharacterSet.TERNARY_BRANCH:
                                            tokenType = TokenType.TernaryBranch;
                                            foundToken = true;
                                            break;
                                        case CharacterSet.LEFT_BRACKET:
                                        case CharacterSet.RIGHT_BRACKET:
                                            tokenType = TokenType.Bracket;
                                            foundToken = true;
                                            break;
                                        case CharacterSet.LEFT_BRACE:
                                        case CharacterSet.RIGHT_BRACE:
                                            tokenType = TokenType.Brace;
                                            foundToken = true;
                                            break;
                                        case CharacterSet.LEFT_SQUARE_BRACKET:
                                        case CharacterSet.RIGHT_SQUARE_BRACKET:
                                            tokenType = TokenType.SquareBracket;
                                            foundToken = true;
                                            break;

                                        //Then some other special types
                                        case CharacterSet.SPECIAL_NUMERIC_FORMAT
                                            when _nextToken?.Value.ToString().ToLower() == CharacterSet.BINARY:
                                            tokenType = TokenType.Binary;
                                            break;
                                        case CharacterSet.SPECIAL_NUMERIC_FORMAT
                                            when _nextToken?.Value.ToString().ToLower() == CharacterSet.OCTAL:
                                            tokenType = TokenType.Octal;
                                            break;
                                        case CharacterSet.SPECIAL_NUMERIC_FORMAT
                                            when _nextToken?.Value.ToString().ToLower() == CharacterSet.HEXADECIMAL:
                                            tokenType = TokenType.Hexadecimal;
                                            break;
                                        case CharacterSet.DOUBLE_QUOTE_STRING:
                                            tokenType = TokenType.DoubleQuoteString;
                                            break;
                                        case CharacterSet.SINGLE_QUOTE_STRING:
                                            tokenType = TokenType.SingleQuoteString;
                                            break;
                                        case CharacterSet.VARIABLE
                                            when _nextToken?.Value is char c && CharacterHelper.IsAlphaNumericChar(c):
                                            tokenType = TokenType.Variable;
                                            break;

                                        //If all else fails, resort to basic types
                                        default:
                                            {
                                                if (_currentToken.Value is char c)
                                                {
                                                    if (CharacterHelper.IsSymbol(c))
                                                    {
                                                        tokenType = TokenType.Symbol;
                                                        foundToken = true;
                                                    }
                                                    else if (CharacterHelper.IsNumericChar(c))
                                                    {
                                                        if (c == '0')
                                                        {
                                                            switch (_nextToken?.Value.ToString().ToLower())
                                                            {
                                                                case CharacterSet.HEXADECIMAL:
                                                                    tokenType = TokenType.Hexadecimal;
                                                                    break;
                                                                case CharacterSet.OCTAL:
                                                                    tokenType = TokenType.Octal;
                                                                    break;
                                                                case CharacterSet.BINARY:
                                                                    tokenType = TokenType.Binary;
                                                                    break;
                                                            }
                                                        }

                                                        if (tokenType == TokenType.None)
                                                        {
                                                            tokenType = _nextToken?.Value.ToString() ==
                                                                        CharacterSet.DECIMAL_POINT
                                                                ? TokenType.Float
                                                                : TokenType.Integer;
                                                            foundToken = !(_nextToken?.Value is char n)
                                                                         || !(CharacterHelper.IsNumericChar(n)
                                                                              || tokenType == TokenType.Float);
                                                        }
                                                    }
                                                    else if (CharacterHelper.IsAlphaChar(c))
                                                    {
                                                        tokenType = TokenType.Instruction;
                                                        foundToken = !(_nextToken?.Value is char n)
                                                                     || !CharacterHelper.IsAlphaChar(n);
                                                    }
                                                }

                                                break;
                                            }
                                    }

                                    break;
                            }

                            break;
                        }

                    case TokenType.Binary:
                        {
                            if (_nextToken?.Value is char c && !CharacterHelper.IsBinaryChar(c))
                                foundToken = true;
                            break;
                        }

                    case TokenType.Octal:
                        {
                            if (_nextToken?.Value is char c && !CharacterHelper.IsOctalChar(c))
                                foundToken = true;
                            break;
                        }

                    case TokenType.Hexadecimal:
                        {
                            if (_nextToken?.Value is char c && !CharacterHelper.IsHexChar(c))
                                foundToken = true;
                            break;
                        }

                    case TokenType.SingleQuoteString:
                        {
                            if (_currentToken.Value.ToString() == "'")
                                foundToken = true;
                            break;
                        }

                    case TokenType.DoubleQuoteString:
                        {
                            if (_currentToken.Value.ToString() == "\"")
                                foundToken = true;
                            break;
                        }

                    case TokenType.Variable:
                        {
                            if (_nextToken?.Value is char c && !CharacterHelper.IsAlphaNumericChar(c))
                                foundToken = true;
                            break;
                        }

                    case TokenType.Integer:
                        {
                            if (_nextToken?.Value.ToString() == CharacterSet.DECIMAL_POINT)
                                tokenType = TokenType.Float;
                            else if (_nextToken?.Value is char c && !CharacterHelper.IsNumericChar(c))
                                foundToken = true;
                            break;
                        }

                    case TokenType.Float:
                        {
                            if (_nextToken?.Value is char c && !(CharacterHelper.IsNumericChar(c) || c == 'e' || c == 'E'))
                                foundToken = true;
                            break;
                        }

                    case TokenType.Instruction:
                        {
                            if (_nextToken?.Value is char c && !CharacterHelper.IsAlphaChar(c))
                                foundToken = true;
                            break;
                        }
                }

                if (foundTokenCount > 1)
                {
                    sb.Append(string.Concat(_currentToken.Value, _nextToken.Value));
                    _currentIndex += foundTokenCount - 1;
                }
                else
                {
                    sb.Append(_currentToken.Value);
                }

                if (_currentIndex + 1 >= _tokens.Count)
                    foundToken = true;

                if (foundToken)
                    return new Token(sb.ToString(), tokenType);

                _currentIndex++;
            }

            return tokenType == TokenType.None ? null : new Token(_currentToken.Value.ToString(), tokenType);
        }

        private static void VerifyTokenValidity(IReadOnlyCollection<Token> tokens)
        {
            try
            {
                if (tokens == null || tokens.Count == 0)
                    throw new InvalidOperationException("Error: Failed to parse any tokens");

                if (tokens.Any(t => t == null || t.Type == TokenType.None))
                    throw new InvalidOperationException("Error: Null token found");

                VerifyBrackets(tokens.ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\r\n");
            }
        }

        private static void VerifyBrackets(IReadOnlyList<Token> tokens)
        {
            var bracketsOpenCount = 0;
            var bracesOpenCount = 0;
            var squareBracketsOpenCount = 0;

            var characterIndex = 0;

            var bracketsOpenIndex = 0;
            var bracesOpenIndex = 0;
            var squareBracketsOpenIndex = 0;

            for (var i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];

                switch (token.Type)
                {
                    case TokenType.Bracket:
                        switch (token.Value)
                        {
                            case CharacterSet.LEFT_BRACKET:
                                bracketsOpenCount++;
                                bracketsOpenIndex = characterIndex;
                                break;
                            case CharacterSet.RIGHT_BRACKET:
                                bracketsOpenCount--;
                                break;
                        }

                        break;

                    case TokenType.SquareBracket:
                        switch (token.Value)
                        {
                            case CharacterSet.LEFT_SQUARE_BRACKET:
                                squareBracketsOpenCount++;
                                squareBracketsOpenIndex = characterIndex;
                                break;
                            case CharacterSet.RIGHT_SQUARE_BRACKET:
                                squareBracketsOpenCount--;
                                break;
                        }

                        break;

                    case TokenType.Brace:
                        switch (token.Value)
                        {
                            case CharacterSet.LEFT_BRACE:
                                bracesOpenCount++;
                                bracesOpenIndex = characterIndex;
                                break;
                            case CharacterSet.RIGHT_BRACE:
                                bracesOpenCount--;
                                break;
                        }

                        break;
                }

                if (bracketsOpenCount < 0)
                    throw new InvalidOperationException($"Error: Extra closing bracket at index {i}.");

                if (squareBracketsOpenCount < 0)
                    throw new InvalidOperationException($"Error: Extra closing square bracket at index {i}.");

                if (bracesOpenCount < 0)
                    throw new InvalidOperationException($"Error: Extra closing brace at index {i}.");

                characterIndex += token.Value.ToString().Length;
            }

            if (bracketsOpenCount > 0)
                throw new InvalidOperationException($"Error: Unclosed bracket at index {bracketsOpenIndex}.");

            if (squareBracketsOpenCount > 0)
                throw new InvalidOperationException($"Error: Unclosed square bracket at index {squareBracketsOpenIndex}.");

            if (bracesOpenCount > 0)
                throw new InvalidOperationException($"Error: Unclosed brace at index {bracesOpenIndex}.");
        }

        private static void FormatTokens(IEnumerable<Token> tokens)
        {
            //Format Binary / Hex / Octal strings
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Binary || token.Type == TokenType.Hexadecimal ||
                    token.Type == TokenType.Octal)
                    token.Value = token.Value.ToString().Substring(0, 2).ToLower() +
                                  token.Value.ToString().Substring(2).ToUpper();
            }
        }
    }
}