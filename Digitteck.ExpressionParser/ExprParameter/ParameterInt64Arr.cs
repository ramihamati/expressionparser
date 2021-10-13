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
    public class ParameterInt64Arr : ParameterObjectBase<Int64[]>, IParameterArray<ParameterInt64>
    {
        public new Int64[] GetValue() => Value;

        public ParameterInt64[] GetObjects()
        {
            List<ParameterInt64> data = new List<ParameterInt64>();

            foreach (var item in Value)
            {
                data.Add(new ParameterInt64(ParametersManager.NextName(), item));
            }
            return data.ToArray();
        }

        public ParameterInt64Arr(string name, Int64[] value) : base(name)
        {
            this.Value = value;
        }
        ////this ctor is accessed in FunctionBase using Activator
        public ParameterInt64Arr(string name, ParameterObject[] value) : base(name)
        {
            this.Value = value.Select(x => x.GetValue()).Select(x=>Convert.ToInt64(x)).ToArray();
        }
    }
}
