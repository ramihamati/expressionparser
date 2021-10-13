using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;
using System.Linq;

namespace Digitteck.ExpressionParser.ExprCommon.Validators
{
    /// <summary>
    /// Returns true if there is an equal no of open and closed brackets
    /// </summary>
    public class ExprValidate_OpenClosedBracketsCountMatch : ExpressionValidate
    {
        public override ValidationResult Validate(string expression, ExprOptions exprOptions)
        {

            int openCount = expression.Count(x => x == exprOptions.OpenBracket);
            int closedCount = expression.Count(x => x == exprOptions.ClosedBracket);

            if (openCount != closedCount)
                return new ValidationResult(ArgumentError.EC_OpenClosedBracketsCountMatch_NoPass,
                    "Unequal number of open brackets and closed brackets");
            return ValidationResult.OK;
        }
    }
}
