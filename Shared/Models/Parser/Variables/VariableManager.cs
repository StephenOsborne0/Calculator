using System.Collections.Generic;
using System.Linq;

namespace Shared.Models.Parser.Variables
{
    internal static class VariableManager
    {
        private static List<ISerializableKeyValuePair> _variables = new List<ISerializableKeyValuePair>();

        internal static bool VariableExists(string variableName) => ContainsKey(variableName);

        internal static object GetVariable(string variableName) => VariableExists(variableName) 
                ? _variables.First(variable => variable.Key == variableName).Value 
                : null;

        private static bool ContainsKey(string key) => _variables.Count(variable => variable.Key == key) > 0;

        internal static void SetVariable(string variableName, object value)
        {
            if (VariableExists(variableName))
                _variables.First(variable => variable.Key == variableName).Value = value;
        }

        internal static bool AddVariable(string variableName, object value)
        {
            if (VariableExists(variableName))
                return false;

            _variables.Add(new SerializableKeyValuePair(variableName, value));
            return true;
        }

        public static void ClearVariables() => _variables = new List<ISerializableKeyValuePair>();

        public static List<ISerializableKeyValuePair> GetVariables() => _variables;
    }
}
