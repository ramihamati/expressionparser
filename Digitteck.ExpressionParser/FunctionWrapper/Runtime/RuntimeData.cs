using Digitteck.ExpressionParser.ExprParameter;
using Digitteck.ExpressionParser.ExprParameter.Bases;
using Digitteck.ExpressionParser.FunctionWrapper.Base;
using Digitteck.ExpressionParser.FunctionWrapper.Convertible;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Digitteck.ExpressionParser.FunctionWrapper.Runtime
{
    public class RuntimeData
    {
        public FunctionBase FunctionBase { get; private set; }

        public ConverterManager ConverterManager { get; private set; }

        public static RuntimeData Create(FunctionBase functionBase, ConverterManager ConverterManager)
        {
            return new RuntimeData(functionBase, ConverterManager);
        }
        private RuntimeData(FunctionBase functionBase, ConverterManager converterManager)
        {
            this.FunctionBase = functionBase;
            this.ConverterManager = converterManager;
        }

        public MatchFunction FindSuitedFunction(ParameterObject[] inputData)
        {
            List<FunctionBase> AllFunctions = FunctionBase.Overloads.Concat(new List<FunctionBase> { FunctionBase }).ToList();

            List<int> transformations = new List<int>();

            foreach (FunctionBase fn in AllFunctions)
            {
                transformations.Add(this.ConverterManager.CalculateTransformations(inputData.Select(x => x.GetType()).ToList(), fn.Parameters.ToList()));
            }

            if (transformations.Count() == 0)
            {
                return new MatchFunction { FunctionBase = null };
            }

            int transformationCount=-1;

            FunctionBase chosenFunction = null;

            for (int index = 0; index < transformations.Count(); index++)
            {
                if (transformationCount == -1 && transformations[index] >= 0)
                {
                    transformationCount = transformations[index];
                    chosenFunction = AllFunctions[index];
                }
                else
                {
                    if (transformations[index] >= 0 && transformations[index] < transformationCount)
                    {
                        transformationCount = transformations[index];
                        chosenFunction = AllFunctions[index];
                    }
                }
            }

            return new MatchFunction { FunctionBase = chosenFunction, TransformationCount = transformationCount };
        }

        public ArgumentsGroupingStatus ConvertParamsToArrayIfAny(ParameterObject[] inputData)
        {
            List<ParameterObject> objList = new List<ParameterObject>();

            for (int i = 0; i < this.FunctionBase.Parameters.Count(); i++)
            {
                ArgumentInfo argInfo = this.FunctionBase.Parameters[i];

                if (argInfo.IsParams)
                {
                    ArgumentMatchingStatus status = CreateArrayFromTypes(i, inputData);

                    if (status.ParameterObject == null)
                        return new ArgumentsGroupingStatus { IsValid = false, Error = status.Error };

                    objList.Add(status.ParameterObject);
                }
                else
                {
                    objList.Add(inputData[i]);
                }
            }
            return new ArgumentsGroupingStatus { IsValid = true, ParameterObjects = objList };
        }

        private ArgumentMatchingStatus CreateArrayFromTypes(int fromPosition, ParameterObject[] inputData)
        {
            ArgumentInfo argRequirementInfo = this.FunctionBase.Parameters[fromPosition];

            List<ParameterObject> restOfArguments = new List<ParameterObject>();

            for (int j = fromPosition; j < inputData.Count(); j++)
            {
                Type inputdata = inputData[j].GetType();

                bool test = argRequirementInfo.ArrayUnitType.Equals(inputdata);

                if (!test)
                {
                    return new ArgumentMatchingStatus
                    {
                        ParameterObject = null,
                        Error = string.Format("Provided parameters at index " + j + " are not of type " + argRequirementInfo.ArrayUnitType.ToString())
                    };
                }

                restOfArguments.Add(inputData[j]);
            }
            //TODO : Refactor reflected
            ParameterObject array =
                Activator.CreateInstance(argRequirementInfo.ArgumentType, new object[] { ParametersManager.NextName(), restOfArguments.ToArray() }) as ParameterObject;

            return new ArgumentMatchingStatus {  ParameterObject = array, Error = ""};
        }


        public ValidatorsResult ValidateInput(List<ParameterObject> parameters, List<ArgumentInfo> arguments)
        {
            for (int i = 0; i < arguments.Count(); i++)
            {
                ArgumentValidators validators = arguments[i].ArgumentValidators;

                ValidatorsResult result =  validators.CheckDataValidity(parameters[i]);

                if (!(result.IsValid))
                    return result;
            }

            return ValidatorsResult.OK;
        }
    }
}
