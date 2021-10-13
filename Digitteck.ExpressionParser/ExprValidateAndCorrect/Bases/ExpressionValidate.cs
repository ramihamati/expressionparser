using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases
{
    /// <summary>
    /// checks entire expression
    /// </summary>
    public abstract class ExpressionValidate : IValidator
    {
        public abstract ValidationResult Validate(string expression, ExprOptions exprOptions);
    }
}
