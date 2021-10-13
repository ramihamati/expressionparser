using Digitteck.ExpressionParser.ExprCommon.Extensions;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Correctors
{
    public class ExprCorrector_PositiveSignCorrection : ExpressionCorrect
    {
        public ExprCorrector_PositiveSignCorrection() : base()
        {
        }
 
        /// Correct +expr to expr
        /// Correct a+(+a) to a+(a)
        /// a++b or a+/b will be triggered as error by another fn
        
        public override string Correct(string expression, ExprOptions exprOptions)
        {
            List<int> indices = expression.AllIndicesOf('+');
            List<int> RemoveAt = new List<int>();
            foreach (int index in indices)
            {
                /// other checks will force that after + we have a name or (
                if (index == 0)
                {
                    RemoveAt.Add(0);
                    continue;
                }
                ///other functions will detect the plus at the end an error
                ///but here we don't want an exception
                if (index == expression.Count() - 1)
                    continue;
                //if (mExpressionOpts.AllNamingCharacters.Contains(expression[index - 1]))
                if (exprOptions.OpenBracket == expression[index - 1])
                {
                    //if the character before the plus is not a namech : a+(+a)
                    //(a+/a) -> this will be detected as an error by another function
                    RemoveAt.Add(index);
                }
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < expression.Count(); i++)
            {
                if (RemoveAt.Contains(i)) continue;
                sb.Append(expression[i]);
            }
            return sb.ToString();
        }
    }
}
