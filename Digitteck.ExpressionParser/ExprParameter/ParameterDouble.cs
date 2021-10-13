using Digitteck.ExpressionParser.ExprParameter.Bases;
using Digitteck.ExpressionParser.ExprParameter.PrimaryOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprParameter
{
    public class ParameterDouble : ParameterBase<double>
    {
        public new double GetValue() => this.Value;

        public ParameterDouble(string name, double value) : base(name, value)
        {
            this.Value = value;
        }
        public static ParameterDouble Create(Double value)
        {
            return new ParameterDouble(ParametersManager.NextName(), value);
        }

        public override Parameter Op_Add(Parameter other)
        {
            switch (other)
            {
                case ParameterInt32 other32:
                    return OverflowAct.AddWithOverflow(this.Value, other32.GetValue());
                case ParameterInt64 other64:
                    return OverflowAct.AddWithOverflow(this.Value , other64.GetValue());
                case ParameterSingle otherSingle:
                    return OverflowAct.AddWithOverflow(this.Value , otherSingle.GetValue());
                case ParameterDouble otherDouble:
                    return OverflowAct.AddWithOverflow(this.Value , otherDouble.GetValue());
                default:
                    throw new Exception("Unreachable");
            }
        }

        public override Parameter Op_Minus(Parameter other)
        {
            switch (other)
            {
                case ParameterInt32 other32:
                    return OverflowAct.SubWithOverflow(this.Value , other32.GetValue());
                case ParameterInt64 other64:
                    return OverflowAct.SubWithOverflow(this.Value , other64.GetValue());
                case ParameterSingle otherSingle:
                    return OverflowAct.SubWithOverflow(this.Value , otherSingle.GetValue());
                case ParameterDouble otherDouble:
                    return OverflowAct.SubWithOverflow(this.Value , otherDouble.GetValue());
                default:
                    throw new Exception("Unreachable");
            }
        }

        public override Parameter Op_Mult(Parameter other)
        {    
            switch (other)
            {
                case ParameterInt32 other32:
                    return OverflowAct.MultWithOverflow(this.Value , other32.GetValue());
                case ParameterInt64 other64:
                    return OverflowAct.MultWithOverflow(this.Value , other64.GetValue());
                case ParameterSingle otherSingle:
                    return OverflowAct.MultWithOverflow(this.Value ,otherSingle.GetValue());
                case ParameterDouble otherDouble:
                    return OverflowAct.MultWithOverflow(this.Value , otherDouble.GetValue());
                default:
                    throw new Exception("Unreachable");
            }
        }

        public override Parameter Op_Div(Parameter other)
        {
            switch (other)
            {
                case ParameterInt32 other32:
                    return OverflowAct.DivWithOverflow(this.Value , other32.GetValue());
                case ParameterInt64 other64:
                    return OverflowAct.DivWithOverflow(this.Value , other64.GetValue());
                case ParameterSingle otherSingle:
                    return OverflowAct.DivWithOverflow(this.Value , otherSingle.GetValue());
                case ParameterDouble otherDouble:
                    return OverflowAct.DivWithOverflow(this.Value , otherDouble.GetValue());
                default:
                    throw new Exception("Unreachable");
            }
        }

        public override Parameter Op_Mod(Parameter other)
        {
            switch (other)
            {
                case ParameterInt32 other32:
                    return OverflowAct.ModWithOverflow(this.Value , other32.GetValue());
                case ParameterInt64 other64:
                    return OverflowAct.ModWithOverflow(this.Value , other64.GetValue());
                case ParameterSingle otherSingle:
                    return OverflowAct.ModWithOverflow(this.Value , otherSingle.GetValue());
                case ParameterDouble otherDouble:
                    return OverflowAct.ModWithOverflow(this.Value , otherDouble.GetValue());
                default:
                    throw new Exception("Unreachable");
            }
        }

        public override Parameter Op_Pow(Parameter other)
        {
            switch (other)
            {
                case ParameterInt32 other32:
                    return OverflowAct.PowWithOverflow(this.Value, other32.GetValue());
                case ParameterInt64 other64:
                    return OverflowAct.PowWithOverflow(this.Value, other64.GetValue());
                case ParameterSingle otherSingle:
                    return OverflowAct.PowWithOverflow(this.Value, otherSingle.GetValue());
                case ParameterDouble otherDouble:
                    return OverflowAct.PowWithOverflow(this.Value, otherDouble.GetValue());
                default:
                    throw new Exception("Unreachable");
            }
        }

        public static explicit operator ParameterDouble(ParameterInt32 that)
        {
            return new ParameterDouble(that.Name, that.GetValue());
        }
    }
}
