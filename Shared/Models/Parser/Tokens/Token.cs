namespace Shared.Models.Parser.Tokens
{
    public class Token
    {
        public object Value { get; set; }

        internal BasicTokenType BasicType { get; set; }

        public TokenType Type { get; set; }

        public Token() { }

        public Token(object value, BasicTokenType basicType)
        {
            Value = value;
            BasicType = basicType;
        }

        public Token(object value, TokenType type)
        {
            Value = value;
            Type = type;
        }
    }

    public enum BasicTokenType
    {
        None,
        Whitespace,
        Newline,
        Alpha,
        Numeric,
        Symbol
    }

    public enum TokenType
    {
        None,
        Integer,
        Float,
        Binary,
        Octal,
        Hexadecimal,
        SingleQuoteString,
        DoubleQuoteString,
        Operator,
        Symbol,
        Variable,
        Instruction,
        Space,
        Tab,
        NewLine,
        Eol,
        TernaryBranch,
        TernaryConditional,
        CarriageReturn,
        Bracket,
        SquareBracket,
        Brace,
        Boolean
    }
}
