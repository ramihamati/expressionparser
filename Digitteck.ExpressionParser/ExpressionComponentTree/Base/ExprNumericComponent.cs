using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.FunctionWrapper;

namespace Digitteck.ExpressionParser.ExpressionComponentTree.Base
{
    public class ExprNumericComponent : ExprComponent
    {
        private ExprNumericComponent(string expression,
                                 ExprOptions exprOptions)
            : base(expression, exprOptions)
        {
            this.Context = expression;
        }

        public static ExprComponent Create(string expression, ExprOptions exprOptions)
        {
            if (exprOptions.HasNumericFormat(expression))
            {
                return new ExprNumericComponent(expression, exprOptions);
            }
            else
            {
                return new ExprInvalidComponent(expression, exprOptions, string.Format("Expression \'{0}\' does not have a function format", expression));
            }
        }
    }
}
