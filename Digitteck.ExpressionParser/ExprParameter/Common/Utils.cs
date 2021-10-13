using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExpressionComponentTree.Base;
using Digitteck.ExpressionParser.ExprModel;
using Digitteck.ExpressionParser.ExprParameter.Bases;
using System;

namespace Digitteck.ExpressionParser.ExprParameter.Common
{
    public static class Utils
    {
        /// <summary>
        /// This function will test components for ParameterBase is ParameterObject
        /// If yes then GetValue() will return a Maybe<ParameterBase>
        /// This object will contain the GetValue() of the final numerical value
        /// </summary>
        public static EvaluateComponentResult<ExprParameterComponent> EvalAndOp
                (ExprParameterComponent first,
                 ExprParameterComponent second,
                 Func<ParameterObject, ParameterObject, EvaluateComponentResult<ExprParameterComponent>> postCheck,
                 Func<Parameter, Parameter, Parameter> operation)
        {
            //if the parameter it's an object then aquire a new ExprParameterComponent
            EvaluateComponentResult<ExprParameterComponent> res1 = Utils.GetSelfOrPropertyIfMapDefined(first);
            EvaluateComponentResult<ExprParameterComponent> res2 = Utils.GetSelfOrPropertyIfMapDefined(second);

            if (!res1.IsValid) return res1;
            if (!res2.IsValid) return res2;

            ExprParameterComponent _first = res1.ExprComponent;
            ExprParameterComponent _second = res2.ExprComponent;

            try
            {
                if (postCheck != null)
                {
                    var postCheckResult = postCheck(_first.ParameterBase, _second.ParameterBase);

                    if (postCheckResult != null)
                        return postCheckResult;
                }

                Parameter result = operation((Parameter)_first.ParameterBase, (Parameter)_second.ParameterBase);

                ExprComponent resultExpr = ExprParameterComponent.Create(first.ExprOptions, result);

                return new EvaluateComponentResult<ExprParameterComponent>
                {
                    ErrorMessage = "",
                    ExprComponent = (ExprParameterComponent)resultExpr,
                    IsValid = true
                };
            }
            catch (Exception ex)
            {
                return new EvaluateComponentResult<ExprParameterComponent>
                {
                    ErrorMessage = string.Format(ex.Message),
                    ExprComponent = null,
                    IsValid = false
                };
            }
        }
        /// <summary>
        /// This function will get a property from an object
        /// The ParameterObject
        /// </summary>
        public static EvaluateComponentResult<ExprParameterComponent> GetSelfOrPropertyIfMapDefined(ExprParameterComponent Component)
        {
            ParameterObject obj = Component.ParameterBase;

            if (string.IsNullOrEmpty(Component.MapToProperty))
            {
                return new EvaluateComponentResult<ExprParameterComponent>
                {
                    IsValid = true,
                    ErrorMessage = "Object " + obj.Name + " has no accessors defined",
                    ExprComponent = Component
                };
            }
            else
            {
                Maybe<ParameterObject> _base = obj.GetValue(Component.MapToProperty, Component.ExprOptions);

                if (!_base.HasValue)
                    return new EvaluateComponentResult<ExprParameterComponent>
                    {
                        IsValid = false,
                        ExprComponent = Component,
                        ErrorMessage = string.Format("Unable to get property \'{0}\', or property is not numeric, from {1}", Component.MapToProperty, obj.Name)
                    };
                else
                {
                    Maybe<ParameterObject> maybeParam = ParametersManager.TryGetComponent(_base.Value.GetValue(), Component.ExprOptions);

                    if (!maybeParam.HasValue)
                        return new EvaluateComponentResult<ExprParameterComponent>
                        {
                            IsValid = false,
                            ExprComponent = Component,
                            ErrorMessage = string.Format("Unable to get numerical property \'{0}\' from {1}", Component.MapToProperty, obj.Name)
                        };

                    return new EvaluateComponentResult<ExprParameterComponent>
                    {
                        IsValid = true,
                        ExprComponent = (ExprParameterComponent)
                                         ExprParameterComponent.Create(Component.ExprOptions, maybeParam.Value),
                        ErrorMessage = ""
                    };
                }
            }
        }
    }
}
