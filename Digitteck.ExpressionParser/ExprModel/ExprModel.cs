using Digitteck.ExpressionParser.ExprCommon.Extensions;
using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExpressionComponentTree.Base;
using Digitteck.ExpressionParser.FunctionWrapper;
using Digitteck.ExpressionParser.FunctionWrapper.Runtime;
using Digitteck.ExpressionParser.ExprParameter;
using Digitteck.ExpressionParser.ExprParameter.PrimaryOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using Digitteck.ExpressionParser.ExprParameter.Common;
using Digitteck.ExpressionParser.ExprParameter.Bases;

namespace Digitteck.ExpressionParser.ExprModel
{
    public class ExprModel : IExprModel
    {
        private FunctionProvider FunctionProvider { get; set; }

        private ParameterCollection ParameterCollection { get; set; }

        public ExprComponent ExprComponent { get; private set; }

        public string Context { get; private set; }

        public string CorrectedContext { get; private set; }

        public ExprOptions ExprOptions { get; private set; }

        Dictionary<OperatorType, IOperation> opStrategies = new Dictionary<OperatorType, IOperation>()
            {
                { OperatorType.Plus, new Add() },
                { OperatorType.Minus, new Subtract() },
                { OperatorType.Divide, new Divide()},
                { OperatorType.Multiply, new Multiply()},
                { OperatorType.Mod, new Mod()},
                { OperatorType.Power, new Power()},
            };

        Dictionary<Type, Func<ExprComponent, EvaluateComponentResult<ExprComponent>>> componentHandler =
        new Dictionary<Type, Func<ExprComponent, EvaluateComponentResult<ExprComponent>>>();

     
        public EvaluateComponentResult<ExprComponent> GetResult()
        {
            return HandleComponent(this.ExprComponent);
        }

        public ExprModel(FunctionProvider functionProvider,
                         ParameterCollection parameterCollection,
                         ExprComponent exprComponent,
                         ExprOptions exprOptions)
        {
            this.FunctionProvider = functionProvider;
            this.ParameterCollection = parameterCollection;

            this.ExprComponent = exprComponent;
            this.Context = exprComponent.Context;
            this.CorrectedContext = exprComponent.Context;
            this.ExprOptions = exprOptions;

            componentHandler.Add(typeof(ExprComplexComponent), (_component) =>
                 HandleComplexComponent((ExprComplexComponent)_component));

            componentHandler.Add(typeof(ExprFunctionComponent), (_component) =>
                    HandleFunctionComponent((ExprFunctionComponent)_component));

            componentHandler.Add(typeof(ExprNumericComponent), (_component) =>
                    HandleNumericComponent((ExprNumericComponent)_component));

            componentHandler.Add(typeof(ExprParameterComponent), (_component) =>
                    HandleParameterComponent((ExprParameterComponent)_component));
        }

        public EvaluateComponentResult<ExprComponent>
            HandleComponentResult(EvaluateComponentResult<ExprComponent> ComponentResult)
        {
            if (ComponentResult.IsValid == false)
                return ComponentResult;
            return HandleComponent(ComponentResult.ExprComponent);
        }

        public EvaluateComponentResult<ExprComponent> HandleComponent(ExprComponent component)
        {
            Type componentType = component.GetType();

            return componentHandler[componentType](component);
        }

        public EvaluateComponentResult<ExprComponent> HandleComplexComponent(ExprComplexComponent component)
        {
            EvaluateComponentResult<ExprComponent> result = EvaluateChildren(component.Children);

            return HandleComponentResult(result);
        }

        public EvaluateComponentResult<ExprComponent> EvaluateChildren(List<ExprComponent> components)
        {
            if (components.Count == 1)
            {
                return EvaluateChildrenWithCountOne(components);
            }
            else if (components.Count < 1)
            {
                return EvaluateChildrenWithNoCount(components);
            }
            else
            {
                int i = GetNextOperatorIndex(components);

                if (i == 0)
                {
                    return EvaluateOperatorAtIndexZero(components);
                }
                else if (i > 0)
                {
                    return EvaluateOperatorAtIndex(components, i);
                }
                //if no op is found then there is only one child and it's managed at first branch
                else
                    throw new Exception("Unknown exception");
            }
        }

        public EvaluateComponentResult<ExprComponent> EvaluateChildrenWithCountOne(List<ExprComponent> components)
        {
            return HandleComponent(components[0]);
        }

        public EvaluateComponentResult<ExprComponent> EvaluateChildrenWithNoCount(List<ExprComponent> components)
        {
            return new EvaluateComponentResult<ExprComponent>
            {
                IsValid = false,
                ExprComponent = null,
                ErrorMessage = "No component passed"
            };
        }

        public EvaluateComponentResult<ExprComponent> EvaluateOperatorAtIndexZero(List<ExprComponent> components)
        {
            ExprOperatorComponent op = (ExprOperatorComponent)components[0];

            if (op.OperatorType == OperatorType.Minus)
            {
                List<ExprComponent> partialEval = components;

                ExprParameterComponent first = ExprParameterComponent.Create(components[1].ExprOptions,
                                                    ParametersManager.TryGetNumberComponent("0",this.ExprOptions).Value)
                                                        as ExprParameterComponent;
                partialEval.Insert(0, first);

                return EvaluateOperatorAtIndex(partialEval, 1);
            }
            else
            {
                return new EvaluateComponentResult<ExprComponent>
                {
                    IsValid = false,
                    ErrorMessage = "Unreachable : Unhandled operator at beginning of expression",
                    ExprComponent = op
                };
            }
        }

        public EvaluateComponentResult<ExprComponent> EvaluateOperatorAtIndex(List<ExprComponent> components, int i)
        {
            EvaluateComponentResult<ExprComponent> firstEval = HandleComponent(components[i - 1]);
            EvaluateComponentResult<ExprComponent> secondEval = HandleComponent(components[i + 1]);

            if (!firstEval.IsValid)
                return firstEval;

            if (!secondEval.IsValid)
                return secondEval;

            if (!(firstEval.ExprComponent is ExprParameterComponent))
                return new EvaluateComponentResult<ExprComponent>
                {
                    IsValid = false,
                    ErrorMessage = string.Format("Component {0} is not parameter", firstEval.ExprComponent.Context),
                    ExprComponent = firstEval.ExprComponent
                };

            if (!(secondEval.ExprComponent is ExprParameterComponent))
                return new EvaluateComponentResult<ExprComponent>
                {
                    IsValid = false,
                    ErrorMessage = string.Format("Component {0} is not parameter", secondEval.ExprComponent.Context),
                    ExprComponent = firstEval.ExprComponent
                };

            EvaluateComponentResult<ExprComponent> result =
                    HandleOperatorComponent(
                        firstEval.ExprComponent as ExprParameterComponent,
                        secondEval.ExprComponent as ExprParameterComponent,
                        (ExprOperatorComponent)components[i]);

            if (!result.IsValid)
                return result;

            List<ExprComponent> newList
                = components.AddInstead(i - 1, i + 1, result.ExprComponent).ToList();

            return EvaluateChildren(newList);
        }

        public EvaluateComponentResult<ExprComponent> HandleParameterComponent(ExprParameterComponent component)
        {
            return new EvaluateComponentResult<ExprComponent>
            {
                ExprComponent = component,
                IsValid = true,
                ErrorMessage = ""
            };
        }

        public EvaluateComponentResult<ExprComponent> HandleFunctionComponent(ExprFunctionComponent component)
        {
            List<ExprComponent> arguments = new List<ExprComponent>();

            foreach (ExprComplexComponent arg in component.Arguments)
            {
                EvaluateComponentResult<ExprComponent> argEvaluation = HandleComponent(arg);
                if (!argEvaluation.IsValid)
                    return argEvaluation;

                arguments.Add(argEvaluation.ExprComponent);
            }
            //all arguments must be parametric

            foreach (ExprComponent arg in arguments)
            {
                if (!(arg is ExprParameterComponent))
                    return new EvaluateComponentResult<ExprComponent>
                    {
                        IsValid = false,
                        ExprComponent = arg,
                        ErrorMessage = string.Format("Argument \'{0}\' was not calculated to be is not parametric", arg.Context)
                    };
            }

            FunctionBase fn = this.FunctionProvider.GetByName(component.FunctionBase.Name).Value;

            ///Guard for Invalid map of property on object
            List<ParameterObject> objArguments = new List<ParameterObject>();

            foreach (ExprParameterComponent item in arguments.Cast<ExprParameterComponent>())
            {
                EvaluateComponentResult<ExprParameterComponent>
                    parametricComponentEval = Utils.GetSelfOrPropertyIfMapDefined(item);

                if (!parametricComponentEval.IsValid)
                    return new EvaluateComponentResult<ExprComponent>
                    {
                        ErrorMessage = parametricComponentEval.ErrorMessage,
                        ExprComponent = parametricComponentEval.ExprComponent,
                        IsValid = false
                    };
                objArguments.Add(parametricComponentEval.ExprComponent.ParameterBase);
            }

            RuntimeEvaluateStatus result = fn.Invoke(objArguments.ToArray());

            if (result.RuntimeArgumentStatus != RuntimeArgumentStatus.OK)
            {
                return new EvaluateComponentResult<ExprComponent>
                {
                    IsValid = false,
                    ExprComponent = component,
                    ErrorMessage = string.Format(result.Errors)
                };
            }

            return new EvaluateComponentResult<ExprComponent>
            {
                ExprComponent = ExprParameterComponent.Create(component.ExprOptions, result.Result),
                ErrorMessage = "",
                IsValid = true
            };
        }

        public EvaluateComponentResult<ExprComponent> HandleNumericComponent(ExprNumericComponent component)
        {
            Maybe<ParameterObject> parameterBase = ParametersManager.TryGetNumberComponent(component.Context, this.ExprOptions);

            if (!parameterBase.HasValue)
                return new EvaluateComponentResult<ExprComponent>
                {
                    ErrorMessage = string.Format("Invalid numeric format = \'{0}\'", component.Context),
                    IsValid = false,
                    ExprComponent = component
                };

            ExprComponent parameterComponent = ExprParameterComponent.Create(component.ExprOptions, parameterBase.Value);

            return new EvaluateComponentResult<ExprComponent>
            {
                IsValid = true,
                ExprComponent = parameterComponent,
                ErrorMessage = ""
            };
        }

        public EvaluateComponentResult<ExprComponent> HandleOperatorComponent
                    (ExprParameterComponent firstValue,
                     ExprParameterComponent secondValue,
                     ExprOperatorComponent operatorComponent)
        {
            EvaluateComponentResult<ExprComponent> firstEval = HandleComponent(firstValue);
            EvaluateComponentResult<ExprComponent> secondEval = HandleComponent(secondValue);

            if (!firstEval.IsValid) return firstEval;
            if (!secondEval.IsValid) return secondEval;

            if (!(firstEval.ExprComponent is ExprParameterComponent))
                return new EvaluateComponentResult<ExprComponent>
                {
                    ExprComponent = firstEval.ExprComponent,
                    IsValid = false,
                    ErrorMessage = "Argument is not a parameter or object"
                };

            if (!(secondEval.ExprComponent is ExprParameterComponent))
                return new EvaluateComponentResult<ExprComponent>
                {
                    ExprComponent = secondEval.ExprComponent,
                    IsValid = false,
                    ErrorMessage = "Argument is not a parameter or object"
                };

            ExprParameterComponent firstParameterComponent = firstEval.ExprComponent as ExprParameterComponent;

            ExprParameterComponent secondParameterComponent = secondEval.ExprComponent as ExprParameterComponent;

            if (firstParameterComponent.IsEnumerable)
                return new EvaluateComponentResult<ExprComponent>
                {
                    ExprComponent = firstEval.ExprComponent,
                    ErrorMessage = string.Format("Cannot apply operation \'{0}\' to enumerable \'{1}\'", operatorComponent.Context,firstParameterComponent.Context),
                    IsValid = false
                };

            if (secondParameterComponent.IsEnumerable)
                return new EvaluateComponentResult<ExprComponent>
                {
                    ExprComponent = firstEval.ExprComponent,
                    ErrorMessage = string.Format("Cannot apply operation to enumerable \'{0}\'", secondParameterComponent.Context),
                    IsValid = false
                };

            IOperation operation = opStrategies[operatorComponent.OperatorType];

            EvaluateComponentResult<ExprParameterComponent> result
                = operation.Result(firstParameterComponent, secondParameterComponent);

            if (!result.IsValid) return result;

            return new EvaluateComponentResult<ExprComponent>
            {
                IsValid = true,
                ErrorMessage = "",
                ExprComponent = result.ExprComponent
            };
        }

        private int GetNextIndexOfMultiplicative(List<ExprComponent> component)
        {
            for (int i = 0; i < component.Count; i++)
            {
                if (component[i] is ExprOperatorComponent)
                {
                    OperatorType opType = ((ExprOperatorComponent)component[i]).OperatorType;
                    if (opType == OperatorType.Multiply
                        || opType == OperatorType.Divide
                        || opType == OperatorType.Mod
                        || opType == OperatorType.Power)
                        return i;
                }
            }
            return -1;
        }

        private int GetNextIndexOfAdditive(List<ExprComponent> component)
        {
            for (int i = 0; i < component.Count; i++)
            {
                if (component[i] is ExprOperatorComponent)
                {
                    OperatorType opType = ((ExprOperatorComponent)component[i]).OperatorType;
                    if (opType == OperatorType.Plus
                        || opType == OperatorType.Minus)
                        return i;
                }
            }
            return -1;
        }

        private int GetNextOperatorIndex(List<ExprComponent> components)
        {
            int i = GetNextIndexOfMultiplicative(components);

            if (i == -1)
                i = GetNextIndexOfAdditive(components);

            return i;
        }
    }
}
