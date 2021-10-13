using Digitteck.ExpressionParser.ExprCommon;
using System;

namespace Digitteck.ExpressionParser.ExprParameter
{
    public abstract class ParameterBase<T> : Parameter
    {
        protected T Value { get;  set; }

        public override object GetValue() => Value;

        public abstract override Parameter Op_Add(Parameter other);
        public abstract override Parameter Op_Minus(Parameter other);
        public abstract override Parameter Op_Mult(Parameter other);
        public abstract override Parameter Op_Div(Parameter other);
        public abstract override Parameter Op_Mod(Parameter other);
        public abstract override Parameter Op_Pow(Parameter other);

        public ParameterBase(string name, T value) : base(name)
        {
            this.Value = value;
        }
    }
}
