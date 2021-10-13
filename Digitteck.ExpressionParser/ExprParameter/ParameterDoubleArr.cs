using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExprCommon.Binding;
using Digitteck.ExpressionParser.ExprParameter.Bases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprParameter
{
    public class ParameterDoubleArr : ParameterObjectBase<double[]>, IParameterArray<ParameterDouble>
    {
        public new double[] GetValue() => Value;

        public ParameterDouble[] GetObjects()
        {
            List<ParameterDouble> data = new List<ParameterDouble>();

            foreach (var item in Value)
            {
                data.Add(new ParameterDouble(ParametersManager.NextName(), item));
            }
            return data.ToArray();
        }

        public ParameterDoubleArr(string name, double[] value) : base(name)
        {
            this.Value = value;
        }
        //TODO : Refactor this way of accessing function
        ////this ctor is accessed in FunctionBase using Activator
        public ParameterDoubleArr(string name, ParameterObject[] value) : base(name)
        {
            //TODO : Remake this because it may be prone to exceptions
            this.Value = value.Select(x => x.GetValue()).Select(x=>Convert.ToDouble(x)).ToArray();
        }
    }
}
