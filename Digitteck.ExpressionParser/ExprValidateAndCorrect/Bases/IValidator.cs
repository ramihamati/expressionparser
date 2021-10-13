

using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases
{
    public interface IValidator
    {
        ValidationResult Validate(string expression, ExprOptions exprOptions);
    }
}
