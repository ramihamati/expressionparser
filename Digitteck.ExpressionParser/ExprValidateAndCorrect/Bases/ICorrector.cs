using Digitteck.ExpressionParser.ExprCommon;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases
{
    public interface ICorrector
    {
        string Correct(string expression, ExprOptions exprOptions);
    }
}
