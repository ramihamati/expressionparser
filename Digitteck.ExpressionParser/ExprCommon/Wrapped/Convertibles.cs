using Digitteck.ExpressionParser.ExprParameter;
using Digitteck.ExpressionParser.FunctionWrapper.Attributes;

namespace Digitteck.ExpressionParser.ExprCommon.Wrapped
{
    public static class Convertibles
    {
        [Convertible]
        public static ParameterDouble Convert(ParameterInt32 value)
        {
            return new ParameterDouble(value.Name, value.GetValue());
        }

        [Convertible]
        public static ParameterDouble Convert(ParameterSingle value)
        {
            return new ParameterDouble(value.Name, value.GetValue());
        }

        [Convertible]
        public static ParameterDoubleArr Convert(ParameterInt32Arr value)
        {
            return new ParameterDoubleArr(value.Name, value.GetObjects());
        }
    }
}
