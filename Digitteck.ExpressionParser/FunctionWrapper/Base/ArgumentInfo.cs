using Digitteck.ExpressionParser.ExprParameter;
using Digitteck.ExpressionParser.ExprParameter.Bases;
using Digitteck.ExpressionParser.FunctionWrapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.FunctionWrapper.Base
{
    public class ArgumentInfo
    {
        /// Argument Type defining data type
        public Type ArgumentType { get; set; }

        /// Extracting limits set to this argument value
        public ArgumentValidators ArgumentValidators { get; set; }

        public bool IsParams { get; set; }

        public string Name { get; set; }

        public int Position { get; set; }

        public bool IsArray { get; set; }

        //if the parameter is ParameterDoubleArr - it has as value double[] but it refers to ParameterDouble as it's ArrayUnitType

        public Type ArrayUnitType { get; set; }

        private ArgumentInfo()
        {


        }

        public static ArgumentInfo Create(ParameterInfo parameterInfo, int parametersCount)
        {
            IArgumentValid[] attributes = parameterInfo.GetCustomAttributes<Attribute>()
                                               .Where(x => x is IArgumentValid)
                                               .Cast<IArgumentValid>()
                                               .ToArray();

            ArgumentValidators validators = new ArgumentValidators(attributes);

            ParamsAttribute paramsAttribute = parameterInfo.GetCustomAttribute<ParamsAttribute>();

            bool isArray = ParametersManager.IsParameterArray(parameterInfo);

            bool isParams = paramsAttribute != null;

            Type arrayUnitType = null;

            if (isArray)
            {
                arrayUnitType = ParametersManager.GetArrayUnitType(parameterInfo);
            }

            if (isParams)
            {
                if (paramsAttribute != null && !(isArray))
                    throw new Exception("Params attribute must be associated to an enumerable type");

                if (paramsAttribute != null && parameterInfo.Position != parametersCount - 1)
                    throw new Exception("Params attribute must be associated to last argument");
            }

            return new ArgumentInfo
            {
                ArgumentValidators = validators,
                Name = parameterInfo.Name,
                Position = parameterInfo.Position,
                IsArray = isArray,
                IsParams = isParams,
                ArgumentType = parameterInfo.ParameterType,
                ArrayUnitType = arrayUnitType
            };
        }
    }
}
