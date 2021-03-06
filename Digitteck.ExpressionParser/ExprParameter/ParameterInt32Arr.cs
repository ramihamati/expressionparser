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
    public class ParameterInt32Arr : ParameterObjectBase<Int32[]>, IParameterArray<ParameterInt32>
    {
        public new Int32[] GetValue() => Value;

        public ParameterInt32[] GetObjects()
        {
            List<ParameterInt32> data = new List<ParameterInt32>();

            foreach (var item in Value)
            {
                data.Add(new ParameterInt32(ParametersManager.NextName(), item));
            }

            return data.ToArray();
        }

        public ParameterInt32Arr(string name, Int32[] value) : base(name)
        {
            this.Value = value;
        }
        ////this ctor is accessed in FunctionBase using Activator and Runtime
        public ParameterInt32Arr(string name, ParameterObject[] value) : base(name)
        {
            this.Value = value.Select(x => x.GetValue()).Select(x=>Convert.ToInt32(x)).ToArray();
        }
    }
}
