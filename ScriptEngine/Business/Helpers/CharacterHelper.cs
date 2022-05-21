namespace ScriptEngine.Business.Helpers
{
    public class CharacterHelper
    {
        /// <summary>
        ///     Checks if the specified character is an alphabetical character.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsAlphaChar(char c) => IsUppercaseAlphaChar(c) || IsLowercaseAlphaChar(c);

        /// <summary>
        ///     Checks if the specified character is an alphabetical character or a numerical character.
        /// </summary>
        /// <param name="c"></param>
        public static bool IsAlphaNumericChar(char c) => IsAlphaChar(c) || IsNumericChar(c);

        /// <summary>
        ///     Checks if the specified character is a hexadecimal character (0-9, A-F).
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsHexChar(char c) => IsUppercaseHexAlphaChar(c) || IsLowercaseHexAlphaChar(c) || IsNumericChar(c);

        /// <summary>
        ///     Checks if the specified character is within the '0' and '7' ASCII range.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsOctalChar(char c) => c >= '0' && c <= '7';

        /// <summary>
        ///     Checks if the specified character is within the '0' and '1' ASCII range.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsBinaryChar(char c) => c == '0' || c == '1';

        /// <summary>
        ///     Checks if the specified character is a whitespace character.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsWhitespaceChar(char c) => c == '\t' || c == ' ';

        /// <summary>
        ///     Checks if the specified character is a new line character.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsNewlineChar(char c) => c == '\n';

        /// <summary>
        ///     Checks if the specified character is a symbol character.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsSymbol(char c) =>
            c >= '!' && c <= '/' ||
            c >= ':' && c <= '@' ||
            c >= '[' && c <= '`' ||
            c >= '{';

        /// <summary>
        ///     Checks if the specified character is within the 'A' and 'Z' ASCII range.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsUppercaseAlphaChar(char c) => c >= 'A' && c <= 'Z';

        /// <summary>
        ///     Checks if the specified character is within the 'a' and 'z' ASCII range.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsLowercaseAlphaChar(char c) => c >= 'a' && c <= 'z';

        /// <summary>
        ///     Checks if the specified character is within the 'A' and 'F' ASCII range.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsUppercaseHexAlphaChar(char c) => c >= 'A' && c <= 'F';

        /// <summary>
        ///     Checks if the specified character is within the 'a' and 'f' ASCII range.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsLowercaseHexAlphaChar(char c) => c >= 'a' && c <= 'f';

        /// <summary>
        ///     Checks if the specified character is within the '0' and '9' ASCII range.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsNumericChar(char c) => c >= '0' && c <= '9';
    }
}
