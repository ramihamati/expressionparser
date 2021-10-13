using Digitteck.ExpressionParser.ExprModel;
using Digitteck.ExpressionParser.ExpressionComponentTree.Base;
using Digitteck.ExpressionParser.ExprParameter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprParameter.PrimaryOperations
{
    public class Add : IOperation
    {
        public EvaluateComponentResult<ExprParameterComponent> 
            Result(ExprParameterComponent First, ExprParameterComponent Second)
        {
            return
                Utils.EvalAndOp(First, Second, null, (_first, _second) =>
                {
                    return _first.Op_Add(_second);
                });
        }
    }
}
