using Digitteck.ExpressionParser.ExprParameter.Bases;

namespace Digitteck.ExpressionParser.ExprParameter
{
    public abstract class Parameter : ParameterObject, IParameter<Parameter>
    {
      
        public Parameter(string name) : base(name) 
        {

        }

        public abstract Parameter Op_Add(Parameter other);
        public abstract Parameter Op_Minus(Parameter other);
        public abstract Parameter Op_Mult(Parameter other);
        public abstract Parameter Op_Div(Parameter other);
        public abstract Parameter Op_Mod(Parameter other);
        public abstract Parameter Op_Pow(Parameter other);
    }
}
