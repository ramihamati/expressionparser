using Digitteck.ExpressionParser.ExprCommon.Extensions;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Correctors
{
    /// <summary>
    /// Corrects (a+b)2 with (a+b)*2. Multiply sign after closing bracket
    /// Corrects (a+b)(a+b) with (a+b)*(a+b)
    /// </summary>
    public class ExprCorrector_MultiplyAfterClosingBracket : ExpressionCorrect
    {
        public ExprCorrector_MultiplyAfterClosingBracket() : base()
        {

        }
        public override string Correct(string expression, ExprOptions exprOptions)
        {
            int[] indices = expression.AllIndicesOf(')').ToArray();
            List<int> MultAddIndices = new List<int>();
            foreach (int index in indices)
            {
                //if index value equals last position in expression continue
                if (index == expression.Count() - 1) continue;

                if (exprOptions.AllNamingCharacters.Contains(expression[index + 1])
                    || exprOptions.OpenBracket == expression[index + 1])
                {
                    MultAddIndices.Add(index);
                }
            }
            StringBuilder sb = new StringBuilder();
            int current = 0;
            foreach (char ch in expression)
            {
                sb.Append(ch);
                if (MultAddIndices.Contains(current))
                    sb.Append('*');
                current++;
            }
            return sb.ToString();
        }
    }
}
