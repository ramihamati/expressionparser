using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Correctors
{
    /// Replace (a+fn(b)) with (a+fn(b).to easily identify functions further
    /// Should not replace (a) - (b) with a) - (b
    public class ExprCorrector_RemoveEnclosingParanthesis : ExpressionCorrect
    {
        public ExprCorrector_RemoveEnclosingParanthesis()
        {
        }
        public override string Correct(string expression, ExprOptions exprOptions)
        {
            if (expression[0] == exprOptions.OpenBracket
                && expression[expression.Length - 1] == exprOptions.ClosedBracket)
            {
                int openCount = 1;
                for (int i = 1; i <= expression.Length - 2; i++)
                {
                    if (expression[i] == exprOptions.OpenBracket)
                        openCount++;
                    if (expression[i] == exprOptions.ClosedBracket)
                        openCount--;
                    //if bracket closed before reaching the end return
                    if (openCount == 0)
                        return expression;
                }
                ///If between second and priorToLast all brackets closed then the expression is:
                ///(a+b+(a)+0)
                if (openCount == 1)
                {
                    expression = expression.Substring(1, expression.Length - 2);
                    ///in this case we should test again for (((a+b+(a)))
                    return Correct(expression, exprOptions);
                }
                ///else the expression is ((a+b)+(c+d))
            }
            return expression;
        }
    }
}
