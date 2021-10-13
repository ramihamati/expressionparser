using Digitteck.ExpressionParser.ExpressionComponentTree.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprModel.ExprHandlers
{
    public class HandlerFunction : IExprComponentHandle<ExprFunctionComponent>
    {
        public ExprFunctionComponent Component { get; }

        private HandlerFunction(ExprFunctionComponent component)
        {
            this.Component = component;
        }

        public EvaluateComponentResult<ExprComponent> Evaluate()
        {
            throw new NotImplementedException();
        }
    }
}
