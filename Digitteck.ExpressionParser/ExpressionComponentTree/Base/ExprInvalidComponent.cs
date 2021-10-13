
using Digitteck.ExpressionParser.ExprCommon;

namespace Digitteck.ExpressionParser.ExpressionComponentTree.Base
{
    public class ExprInvalidComponent : ExprComponent
    {
        public string ErrorMessage { get; set; }

        public ExprInvalidComponent(string ExpressionString, ExprOptions exprOptions, string errorMessage="") 
            : base(ExpressionString, exprOptions)
        {
            this.ErrorMessage = errorMessage;
        }
    }
}
