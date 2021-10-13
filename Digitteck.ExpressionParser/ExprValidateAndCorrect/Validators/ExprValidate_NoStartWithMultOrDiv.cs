using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;

namespace Digitteck.ExpressionParser.ExprCommon.Validators
{
    /// <summary>
    /// *(21) or %(21) or /(21) will be false
    /// </summary>
    public class ExprValidate_NoStartWithMultOrDiv : ExpressionValidate
    {
        public override ValidationResult Validate(string expression, ExprOptions exprOptions)
        {
            if (expression[0] == '*' || expression[0] == '/' || expression[0] == '%' || expression[0] == '^')
                return new ValidationResult(ArgumentError.EC_NoStartWithMultDivMod, 
                    "Cannot start with * / % ^");
            return ValidationResult.OK;
        }
    }
}
