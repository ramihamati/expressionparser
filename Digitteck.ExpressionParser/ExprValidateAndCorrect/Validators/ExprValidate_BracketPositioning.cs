
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Validators
{
    /// <summary>
    /// It checks if closed brackets are met before openbrackets are placed
    /// </summary>
    public class ExprValidate_BracketPositioning : ExpressionValidate
    {
        public override ValidationResult Validate(string expression, ExprOptions exprOptions)
        {
            int openBracketCount = 0;
            foreach (char ch in expression)
            {
                if (ch == exprOptions.OpenBracket) openBracketCount++;
                if (ch == exprOptions.ClosedBracket) openBracketCount--;
                if (openBracketCount < 0)
                    return new ValidationResult(ArgumentError.EC_ClosedBracketHasNoOpenBracketMatch,
                        string.Format("Bracket mismatch"));
            }
            return ValidationResult.OK;
        }
    }
}
