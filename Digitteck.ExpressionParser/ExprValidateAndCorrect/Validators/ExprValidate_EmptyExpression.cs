using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;

namespace Digitteck.ExpressionParser.ExprCommon.Validators
{
    /// <summary>
    /// Checks if there is an expression at all
    /// </summary>
    public class ExprValidate_EmptyExpression : ExpressionValidate
    {
        public override ValidationResult Validate(string expression, ExprOptions exprOptionss)
        {
            if (expression.Length == 0)
                return new ValidationResult(ArgumentError.EC_EmptyExpression, "Expression is empty");

            return ValidationResult.OK;
        }
    }
}
