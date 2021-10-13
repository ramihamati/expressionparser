using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprParameter.Bases
{
    public abstract class ParameterObjectBase<T> : ParameterObject
    {
        protected T Value { get; set; }

        public override object GetValue() => Value;

        public ParameterObjectBase(string name) : base(name)
        {
        }
    }
}
