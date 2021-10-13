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
    public class ParameterSingleArr : ParameterObjectBase<float[]>, IParameterArray<ParameterSingle>
    {
        public new float[] GetValue() => Value;

        public ParameterSingle[] GetObjects()
        {
            List<ParameterSingle> data = new List<ParameterSingle>();

            foreach (var item in Value)
            {
                data.Add(new ParameterSingle(ParametersManager.NextName(), item));
            }
            return data.ToArray();
        }

        public ParameterSingleArr(string name, float[] value) : base(name)
        {
            this.Value = value;
        }
        ////this ctor is accessed in FunctionBase using Activator
        public ParameterSingleArr(string name, ParameterObject[] value) : base(name)
        {
            this.Value = value.Select(x => x.GetValue()).Select(x=>Convert.ToSingle(x)).ToArray();
        }
    }
}
