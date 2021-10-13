using Digitteck.ExpressionParser.ExprParameter;
using Digitteck.ExpressionParser.ExprParameter.Bases;
using Digitteck.ExpressionParser.FunctionWrapper;
using Digitteck.ExpressionParser.FunctionWrapper.Attributes;
using Digitteck.ExpressionParser.FunctionWrapper.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Digitteck.ExpressionParser.ExprCommon.WrappedFunctions
{

    public static class Functions
    {
        [Declared]
        public static ParameterDouble Sqrt([MinValue(0)] ParameterDouble value)
        {
            return new ParameterDouble(ParametersManager.NextName(), Math.Sqrt(value.GetValue()));
        }

       
        [Declared(FunctionDefinitionType = FunctionDefinitionType.Base)]
        public static ParameterDouble Max(ParameterDoubleArr data)
        {
            double result = data.GetValue().ToList().Aggregate((a, b) => Math.Max(a, b));

            return new ParameterDouble(ParametersManager.NextName(), result);
        }

        [Declared]
        public static ParameterInt32Arr List([Params] ParameterInt32Arr data)
        {
            return new ParameterInt32Arr(ParametersManager.NextName(), data.GetValue());
        }

        [Declared(FunctionDefinitionType = FunctionDefinitionType.Overload)]
        public static ParameterDoubleArr List([Params] ParameterDoubleArr data)
        {
            return new ParameterDoubleArr(ParametersManager.NextName(), data.GetValue());
        }


        //[Declared(FunctionDefinitionType = FunctionDefinitionType.Overload)]
        //public static double Sqrt([ArgMaxValue(MaxValue = 111)] short value)
        //{
        //    return Math.Sqrt(value);
        //}

        //[Declared(FunctionDefinitionType = FunctionDefinitionType.Overload)]
        //public static double Sqrt([ArgMaxValue(MaxValue = 120)] double value)
        //{
        //    return Math.Sqrt(value);
        //}

        //[Declared(FunctionDefinitionType = FunctionDefinitionType.Overload)]
        //public static double Sqrt([ArgMaxValue(MaxValue = 150)] float value)
        //{
        //    return Math.Sqrt(value);
        //}

        //[Declared(FunctionDefinitionType = FunctionDefinitionType.Overload)]
        //public static double Sqrt([ArgMaxValue(MaxValue = 100)] long value)
        //{
        //    return Math.Sqrt(value);
        //}
    }
}
