using Digitteck.ExpressionParser.ExprParameter;
using Digitteck.ExpressionParser.ExprParameter.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.FunctionWrapper.Attributes
{
    public class MinValueAttribute : System.Attribute, IArgumentValid
    {
        private double MinValue { get; set; }

        public MinValueAttribute(double minValue)
        {
            this.MinValue = minValue;
        }

        public string ErrorMessage =>
            string.Format("Value must be greater then {0}", this.MinValue);

        public bool IsValid(ParameterObject Context)
        {
            switch (Context)
            {
                case ParameterInt32 contextInt32:
                    return contextInt32.GetValue() >= this.MinValue;
                case ParameterInt64 contextInt64:
                    return contextInt64.GetValue() >= this.MinValue;
                case ParameterSingle contextSingle:
                    return contextSingle.GetValue() >= this.MinValue;
                case ParameterDouble contextDouble:
                    return contextDouble.GetValue() >= this.MinValue;
                default:
                    return false;
            }
        }
    }
}
