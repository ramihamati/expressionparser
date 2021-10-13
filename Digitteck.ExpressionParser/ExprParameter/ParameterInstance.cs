using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExprCommon.Binding;
using Digitteck.ExpressionParser.ExprParameter.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprParameter
{
    public class ParameterInstance : ParameterObjectBase<object>
    {
        public override object GetValue() => Value;

        public ParameterInstance(string name, object value) : base(name)
        {
            this.Value = value;
        }
    }
}
