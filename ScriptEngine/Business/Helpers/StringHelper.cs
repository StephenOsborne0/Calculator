using System.Linq;
using System.Text.RegularExpressions;
using Shared.Models.Parser;

namespace ScriptEngine.Business.Helpers
{
    public class StringHelper
    {
        /// <summary>
        ///     Checks to see if the specified string is in the list of supported operators.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsOperator(string str) => CharacterSet.Operators.Contains(str);

        public static bool IsSymbol(string str)
        {
            foreach (var c in str)
                if (!CharacterHelper.IsSymbol(c))
                    return false;
            return true;
        }

        /// <summary>
        ///     Checks to see if the specified string is an integer value.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInteger(string str) => int.TryParse(str, out _);

        /// <summary>
        ///     Checks to see if the specified string is a float value.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsFloat(string str) => float.TryParse(str, out _);

        /// <summary>
        ///     Checks to see if the specified string is a double value.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDouble(string str) => double.TryParse(str, out _);

        /// <summary>
        ///     Checks to see if all characters in the string are a hex character.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool IsHexString(string str)
        {
            foreach (var c in str)
                if (!CharacterHelper.IsHexChar(c))
                    return false;
            return true;
        }

        /// <summary>
        ///     Checks to see if all characters in the string are an octal character.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool IsOctalString(string str)
        {
            foreach (var c in str)
                if (!CharacterHelper.IsOctalChar(c))
                    return false;
            return true;
        }

        /// <summary>
        ///     Checks to see if all characters in the string are a binary character.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool IsBinaryString(string str)
        {
            foreach (var c in str)
                if (!CharacterHelper.IsBinaryChar(c))
                    return false;
            return true;
        }

        /// <summary>
        ///     Checks to see if the string is of float notation.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool IsFloatString(string str)
        {
            if (!str.Contains('.'))
                return false;

            var shortFloatRegex = new Regex(@"\d+.\d+");
            var longFloatRegex = new Regex(@"\d+.\d+e\d+");

            return shortFloatRegex.IsMatch(str) || longFloatRegex.IsMatch(str);
        }
    }
}
