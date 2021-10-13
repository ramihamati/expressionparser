using Digitteck.ExpressionParser.ExprParameter;
using Digitteck.ExpressionParser.ExprParameter.Bases;
using Digitteck.ExpressionParser.FunctionWrapper.Attributes;
using Digitteck.ExpressionParser.FunctionWrapper.Base;
using Digitteck.ExpressionParser.FunctionWrapper.Convertible;
using Digitteck.ExpressionParser.FunctionWrapper.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.FunctionWrapper
{
    public class FunctionBase
    {
        public string Name { get; private set; }

        public MethodInfo MethodInfo { get; private set; }

        public Type OwnerType { get; private set; }

        public FunctionDefinitionType FunctionDefinitionType { get; private set; }

        public ArgumentInfo[] Parameters { get; private set; }

        public FunctionBase[] Overloads { get; private set; }

        public int ParametersCount;

        private ConverterManager ConverterManager { get; set; }

        public static FunctionBase Create(MethodInfo baseMethod, MethodInfo[] overloadMethods, ConverterManager converterManager)
        {
            foreach (MethodInfo minfo in overloadMethods)
            {
                if (minfo.Name != baseMethod.Name)
                    throw new Exception(string.Format("Overload method name \'{0}\' is not compatible with base name \'{1}\'", minfo.Name, baseMethod.Name));
            }

            return new FunctionBase(baseMethod, overloadMethods, converterManager);
        }

        private FunctionBase(MethodInfo baseMinfo, ConverterManager converterManager)
        {
            this.Name = baseMinfo.Name;
            this.MethodInfo = baseMinfo;
            this.OwnerType = baseMinfo.DeclaringType;
            this.ConverterManager = converterManager;
            this.FunctionDefinitionType = baseMinfo.GetCustomAttribute<DeclaredAttribute>().FunctionDefinitionType;
            this.ParametersCount = baseMinfo.GetParameters().Count();

            this.Parameters = baseMinfo
                               .GetParameters()
                               .Select(paramInfo => ArgumentInfo.Create(paramInfo, this.ParametersCount))
                               .ToArray();

        }

        private FunctionBase(MethodInfo baseMinfo, MethodInfo[] baseOverloads, ConverterManager converterManager)
            : this(baseMinfo, converterManager)
        {
            List<FunctionBase> overloadFn = new List<FunctionBase>();

            foreach (MethodInfo mInfo in baseOverloads)
            {
                overloadFn.Add(new FunctionBase(mInfo, converterManager));
            }
            this.Overloads = overloadFn.ToArray();
        }

      
        private RuntimeEvaluateStatus InvokeDirect(ParameterObject[] inputData, int transformationCount)
        {
            RuntimeData runtimeData = RuntimeData.Create(this, this.ConverterManager);

            List<ParameterObject> transformed = new List<ParameterObject>();

            if (transformationCount == 0)
            {
                transformed = inputData.ToList();
            }
            else
            {
                transformed = this.ConverterManager.ConvertTo(inputData.ToList(), this.Parameters.ToList());
            }

            ArgumentsGroupingStatus conversionGroupStatus = runtimeData.ConvertParamsToArrayIfAny(transformed.ToArray());

            if (!conversionGroupStatus.IsValid)
            {
                return new RuntimeEvaluateStatus
                {
                    RuntimeArgumentStatus = RuntimeArgumentStatus.UnableToCast,
                    Errors = conversionGroupStatus.Error,
                    Result = null
                };
            }
            List<ParameterObject> grouped = conversionGroupStatus.ParameterObjects;

            ValidatorsResult validatorsResult = runtimeData.ValidateInput(grouped, this.Parameters.ToList());

            if (!validatorsResult.IsValid)
                return new RuntimeEvaluateStatus
                {
                    RuntimeArgumentStatus = RuntimeArgumentStatus.ValuesNotValidated,
                    Errors = validatorsResult.ErrorMessages,
                    Result = null
                };
            //check data validity
            ParameterObject result = null;
            try
            {
                result = this.MethodInfo.Invoke(OwnerType, grouped.ToArray()) as ParameterObject;
            }
            catch (Exception ex)
            {
                return new RuntimeEvaluateStatus
                {
                    RuntimeArgumentStatus = RuntimeArgumentStatus.RuntimeError,
                    Result = null,
                    Errors = ex.Message
                };
            }
            //Check is a result is null
            if (result == null)
            {
                return new RuntimeEvaluateStatus
                {
                    RuntimeArgumentStatus = RuntimeArgumentStatus.NullResult,
                    Result = null,
                    Errors = string.Format("Null result when evaluating {0}", this.MethodInfo.Name)
                };
            }
            //Check is a result is NaN - only if it's not an array
            if (!(ParametersManager.IsParameterArray(result)))
            {
                if (double.IsNaN(Convert.ToDouble(result.GetValue())))
                    return new RuntimeEvaluateStatus
                    {
                        RuntimeArgumentStatus = RuntimeArgumentStatus.NaN,
                        Result = result,
                        Errors = "Result is NaN at function " + this.Name
                    };
            }

            return new RuntimeEvaluateStatus
            {
                RuntimeArgumentStatus = RuntimeArgumentStatus.OK,
                Errors = "",
                Result = result
            };

        }

        public RuntimeEvaluateStatus Invoke(ParameterObject[] inputData)
        {
            RuntimeData runtimeData = RuntimeData.Create(this, this.ConverterManager);

            MatchFunction status1 = runtimeData.FindSuitedFunction(inputData);

            if (status1.TransformationCount == -1)
                return new RuntimeEvaluateStatus
                {
                    RuntimeArgumentStatus = RuntimeArgumentStatus.NoMathingSignatureFound,
                    Result = null,
                    Errors = string.Format("No overload was found for {0} that accepts the provided arguments", this.Name)
                };

            return status1.FunctionBase.InvokeDirect(inputData, status1.TransformationCount);
        }
    }
}
