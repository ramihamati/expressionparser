using Digitteck.ExpressionParser.ExprModel;
using Digitteck.ExpressionParser.ExpressionComponentTree.Base;
using System;
using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExprParameter.Common;

namespace Digitteck.ExpressionParser.ExprParameter.PrimaryOperations
{
    public class Power : IOperation
    {
        public EvaluateComponentResult<ExprParameterComponent> 
            Result(ExprParameterComponent First, ExprParameterComponent Second)
        {
            return Utils.EvalAndOp(First, Second, null, (_first, _second) => _first.Op_Pow(_second));
        }
    }
}
