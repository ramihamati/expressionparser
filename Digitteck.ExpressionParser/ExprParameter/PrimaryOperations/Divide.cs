using Digitteck.ExpressionParser.ExprModel;
using Digitteck.ExpressionParser.ExpressionComponentTree.Base;
using System;
using Digitteck.ExpressionParser.ExprParameter.Common;

namespace Digitteck.ExpressionParser.ExprParameter.PrimaryOperations
{
    public class Divide : IOperation
    {
        public EvaluateComponentResult<ExprParameterComponent>
            Result(ExprParameterComponent First, ExprParameterComponent Second)
        {
            return Utils.EvalAndOp(First, Second,
                (_first, _second) =>
                {
                    if (Convert.ToDouble(Second.ParameterBase.GetValue()) == 0)
                        return new EvaluateComponentResult<ExprParameterComponent>
                        {
                            IsValid = false,
                            ExprComponent = null,
                            ErrorMessage = string.Format("Attempt to divide by 0 \'{0}\\{1}\'", First.ParameterBase.GetValue(), Second.ParameterBase.GetValue())
                        };

                    return null;
                },
                (_first, _second) => _first.Op_Div(_second));

        }
    }
}
