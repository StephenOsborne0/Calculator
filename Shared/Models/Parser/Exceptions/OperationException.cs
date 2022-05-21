using System;

namespace Shared.Models.Parser.Exceptions
{
    public class OperationException : Exception
    {
        public object NodeAValue { get; }

        public object NodeBValue { get; }

        public object Sender { get; }

        public new string Message { get; }

        public OperationException(object sender, object nodeAValue, object nodeBValue)
        {
            Sender = sender;
            NodeAValue = nodeAValue;
            NodeBValue = nodeBValue;
            Message = $"Unable to perform {sender} operation on \"${nodeAValue}\"" + (nodeBValue  == null ? "" : $" and \"{nodeBValue}\"");
        }
    }
}
