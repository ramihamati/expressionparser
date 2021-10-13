using Digitteck.ExpressionParser.ExprCommon;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases
{
    public abstract class ExpressionCorrect : ICorrector
    {
        public ExpressionCorrect()
        {
        }

        public abstract string Correct(string expression, ExprOptions exprOptions);
    }
}
