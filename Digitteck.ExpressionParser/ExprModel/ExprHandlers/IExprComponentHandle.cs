using Digitteck.ExpressionParser.ExpressionComponentTree.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprModel.ExprHandlers
{
    public interface IExprComponentHandle<T> where T : ExprComponent
    {
        T Component { get; }

        EvaluateComponentResult<ExprComponent> Evaluate();
    }
}
