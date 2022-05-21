namespace Shared.Models.Parser
{
    public class CharacterSet
    {
        public const string ADDITION = "+";
        public const string SUBTRACTION = "-";
        public const string MULTIPLICATION = "*";
        public const string DIVISION = "/";
        public const string BITWISE_OR = "|";
        public const string BITWISE_AND = "&";
        public const string LESS_THAN = "<";
        public const string GREATER_THAN = ">";
        public const string NOT = "!";
        public const string MODULUS = "%";
        public const string BITWISE_XOR = "^";
        public const string ASSIGNMENT = "=";
        public const string ONES_COMPLEMENT = "~";
        public const string TERNARY_CONDITIONAL = "?";
        public const string TERNARY_BRANCH = ":";
        public const string DECIMAL_POINT = ".";

        public const string LOGICAL_OR = "||";
        public const string LOGICAL_AND = "&&";
        public const string BITSHIFT_LEFT = "<<";
        public const string BITSHIFT_RIGHT = ">>";
        public const string EQUALITY = "==";
        public const string LESS_THAN_EQUAL = "<=";
        public const string GREATER_THAN_EQUAL = ">=";
        public const string NOT_EQUAL = "!=";
        public const string NOT_EQUAL2 = "<>";

        public const string ASSIGNMENT_ADDITION = "+=";
        public const string ASSIGNMENT_SUBTRACTION = "-=";
        public const string ASSIGNMENT_MULTIPLICATION = "*=";
        public const string ASSIGNMENT_DIVISION = "/=";
        public const string ASSIGNMENT_BITWISE_OR = "|=";
        public const string ASSIGNMENT_BITWISE_AND = "&=";
        public const string ASSIGNMENT_MODULUS = "%=";
        public const string ASSIGNMENT_BITWISE_XOR = "^=";
        public const string ASSIGNMENT_ONES_COMPLEMENT = "~=";
        public const string ASSIGNMENT_LOGICAL_AND = "&&=";
        public const string ASSIGNMENT_LOGICAL_OR = "||=";

        public const string INCREMENT = "++";
        public const string DECREMENT = "--";
        public const string SQUARE = "**";
        public const string SQUARE_ROOT = "//";

        public const string EOL = ";";
        public const string NEWLINE = "\n";
        public const string CARRIAGE_RETURN = "\r";
        public const string TAB = "\t";
        public const string SPACE = " ";

        public const string LEFT_BRACKET = "(";
        public const string RIGHT_BRACKET = ")";
        public const string LEFT_SQUARE_BRACKET = "[";
        public const string RIGHT_SQUARE_BRACKET = "]";
        public const string LEFT_BRACE = "{";
        public const string RIGHT_BRACE = "}";

        public const string SINGLE_QUOTE_STRING = "'";
        public const string DOUBLE_QUOTE_STRING = "\"";
        public const string VARIABLE = "_";

        public const string SPECIAL_NUMERIC_FORMAT = "#";
        public const string BINARY = "b";
        public const string OCTAL = "q";
        public const string HEXADECIMAL = "x";

        public const string DELIMITER = ",";

        public static string[] Operators = 
        {
            ADDITION,
            SUBTRACTION,
            MULTIPLICATION,
            DIVISION,
            BITWISE_OR,
            BITWISE_AND,
            LESS_THAN,
            GREATER_THAN,
            LOGICAL_OR,
            LOGICAL_AND,
            BITSHIFT_LEFT,
            BITSHIFT_RIGHT,
            NOT,
            MODULUS,
            BITWISE_XOR,
            ASSIGNMENT,
            ONES_COMPLEMENT,
            TERNARY_CONDITIONAL,
            TERNARY_BRANCH,
            DECIMAL_POINT,
            ASSIGNMENT_ADDITION,
            ASSIGNMENT_SUBTRACTION,
            ASSIGNMENT_MULTIPLICATION,
            ASSIGNMENT_DIVISION,
            EQUALITY,
            ASSIGNMENT_BITWISE_AND,
            ASSIGNMENT_BITWISE_OR,
            LESS_THAN_EQUAL,
            GREATER_THAN_EQUAL,
            NOT_EQUAL,
            ASSIGNMENT_MODULUS,
            ASSIGNMENT_BITWISE_XOR,
            ASSIGNMENT_ONES_COMPLEMENT,
        };
    }
}
