using Shared.Models.Parser.Nodes;
using Shared.Models.Parser.Tokens;
using Shared.Models.Parser.Variables;

namespace Shared.Models.Parser.OperationNodes
{
    public class VariableNode : BaseNode
    {
        public string Name { get; set; }

        public override object Value
        {
            get
            {
                if (!VariableManager.VariableExists(Name))
                    VariableManager.AddVariable(Name, 0);
                return VariableManager.GetVariable(Name);
            }
            set => VariableManager.SetVariable(Name, value);
        }

        public override string ToString() => $"{Name}";

        public VariableNode() { }

        public VariableNode(Token token) : base(token)
        {
            Name = token.Value.ToString();
        }
    }
}
