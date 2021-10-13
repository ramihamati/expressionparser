using Digitteck.ExpressionParser.ExprParameter;
using Digitteck.ExpressionParser.ExprParameter.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.FunctionWrapper.Attributes
{
    public class MaxValueAttribute: System.Attribute, IArgumentValid
    {
        private double MaxValue { get; set; }

        public string ErrorMessage =>
            string.Format("Value must be smaller then {0}", this.MaxValue);

        public MaxValueAttribute(double maxValue)
        {
            this.MaxValue = maxValue;
        }

        public bool IsValid(ParameterObject Context)
        {
            switch (Context)
            {
                case ParameterInt32 contextInt32:
                    return contextInt32.GetValue() <= this.MaxValue;
                case ParameterInt64 contextInt64:
                    return contextInt64.GetValue() <= this.MaxValue;
                case ParameterSingle contextSingle:
                    return contextSingle.GetValue() <= this.MaxValue;
                case ParameterDouble contextDouble:
                    return contextDouble.GetValue() <= this.MaxValue;
                default:
                    return false;
            }
        }
    }
}
