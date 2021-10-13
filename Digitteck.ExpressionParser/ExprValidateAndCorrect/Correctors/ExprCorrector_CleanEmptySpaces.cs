using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;
using System.Collections.Generic;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Correctors
{
    /// <summary>
    /// replaces "a + b + c" with "a+b+c"
    /// </summary>
    public class ExprCorrector_CleanEmptySpaces : ExpressionCorrect
    {
        public ExprCorrector_CleanEmptySpaces() : base()
        {
        }

        public override string Correct(string expression, ExprOptions exprOptions)
        {
            List<char> newExpressionArray = new List<char>();
            foreach (char ch in expression)
            {
                if (ch != ' ')
                    newExpressionArray.Add(ch);
            }
            return string.Concat(newExpressionArray);
        }
    }
}
