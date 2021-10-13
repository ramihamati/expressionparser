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
    /// Multiply sign before opening bracket
    /// Corrects a(b+1) with a*(b+1) if a is not a function
    /// </summary>
    public class ExprCorrector_MultiplyBeforeOpenBracket : ExpressionCorrect
    {
        string[] FnNames;

        public ExprCorrector_MultiplyBeforeOpenBracket(string[] fnNames)
        {
            this.FnNames = fnNames;
        }

        public override string Correct(string expression, ExprOptions exprOptions)
        {
            int[] indices = expression.AllIndicesOf(x => x == exprOptions.OpenBracket).ToArray();

            List<int> AddMultAt = new List<int>();
            StringBuilder sb = new StringBuilder();
            bool collect = false;
            bool addIndex = false;

            foreach (int index in indices)
            {
                collect = false;
                /// If an open bracket exists at the beginning of the expression then continue
                if (index == 0) continue;
                ///Collect if before the bracket we don't have an operator
                if (exprOptions.AllNamingCharacters.Contains(expression[index - 1]))
                {
                    collect = true;
                }
                if (collect)
                {
                    sb.Clear();
                    /// Stop where the naming ends or at index=0
                    for (int j = index - 1; j >= 0; j--)
                    {
                        /// if next character is not naming character stop collecting
                        if (!exprOptions.AllNamingCharacters.Contains(expression[j]))
                        {
                            collect = false;
                        }
                        else
                        {   /// Current char is a naming char, add to string
                            sb.Append(expression[j]);
                        }

                        if ((collect == false && sb.Length > 0) ||
                            (j == 0 && sb.Length > 0 && collect == true))
                        {
                            addIndex = true;
                        }
                        if (addIndex == true)
                        {
                            addIndex = false;
                            string b = sb.ToString().ToCharArray().Reverse().JoinToString();
                            if (!FnNames.Contains(b))
                            {
                                AddMultAt.Add(index);
                            }
                            sb.Clear();
                            break;
                        }
                        if (collect == false && sb.Length == 0)
                            break;
                    }
                }
            }
            sb.Clear();
            int i = 0;
            foreach (char ch in expression)
            {
                if (AddMultAt.Contains(i))
                    sb.Append('*');
                sb.Append(ch);
                i++;
            }

            return sb.ToString();
        }
    }
}
