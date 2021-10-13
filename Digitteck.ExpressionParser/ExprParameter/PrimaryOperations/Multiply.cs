using Digitteck.ExpressionParser.ExprModel;
using Digitteck.ExpressionParser.ExpressionComponentTree.Base;
using System;
using Digitteck.ExpressionParser.ExprParameter.Common;

namespace Digitteck.ExpressionParser.ExprParameter.PrimaryOperations
{
    public class Multiply :IOperation
    {
        public EvaluateComponentResult<ExprParameterComponent> 
            Result(ExprParameterComponent First, ExprParameterComponent Second)
        {
            return
                Utils.EvalAndOp(First, Second, null, (_first, _second) => _first.Op_Mult(_second));
        }
    }
}
