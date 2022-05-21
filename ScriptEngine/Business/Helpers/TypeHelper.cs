using Shared.Models.Parser.Tokens;

namespace ScriptEngine.Business.Helpers
{
    public class TypeHelper
    {
        public static TokenType GetTokenTypeForValue(object value)
        {
            if (float.TryParse(value.ToString(), out _))
                return TokenType.Float;
            if (int.TryParse(value.ToString(), out _))
                return TokenType.Integer;
            if (bool.TryParse(value.ToString(), out _))
                return TokenType.Boolean;
            if (value.ToString().ToLower().StartsWith("0x"))
                return TokenType.Hexadecimal;
            if (value.ToString().ToLower().StartsWith("0q"))
                return TokenType.Octal;
            if (value.ToString().ToLower().StartsWith("0b"))
                return TokenType.Binary;
            return TokenType.None;
        }
    }
}
