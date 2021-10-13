using Digitteck.ExpressionParser.ExprCommon.Extensions;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Correctors
{
    /// "a+(b)" => "a+b"
    /// "a*(1)" => "a*1"
    /// "a*(.2)" => a*.2
    /// "a*(10000.0001) => a*10000.0001
    /// "a*(__abc) => a*__abc"
    /// "a*(__124) => a*__124

    public class ExprCorrector_RemoveRedundantBrackets : ExpressionCorrect
    {
        public ExprCorrector_RemoveRedundantBrackets()
        {
        }

        private static int FindClosingMatchBracket(string context, int openBracketPosition, ExprOptions exprOptions)
        {
            if (context[openBracketPosition] != exprOptions.OpenBracket)
                return -2;

            int openBracketCount = 1;
            for (int i = openBracketPosition + 1; i < context.Length; i++)
            {
                if (context[i] == exprOptions.OpenBracket)
                    openBracketCount++;
                if (context[i] == exprOptions.ClosedBracket)
                    openBracketCount--;
                if (openBracketCount == 0)
                    return i;
            }
            return -1;
        }

        string RemoveBracketsAroundNames(string expression, ExprOptions exprOptions)
        {
            List<int> OpenBracketRedundantIndices = new List<int>();
            List<int> ClosedBracketRedundantIndices = new List<int>();
            List<int> openBracketIndices = expression.AllIndicesOf('(');

            if (openBracketIndices.Count == 0)
                return expression;

            openBracketIndices.ForEach((openBracketIndex) => FindClosingBracketIndices(openBracketIndex));

            void FindClosingBracketIndices(int openBracketIndex)
            {
                //find closing bracket only if between open and closed there is name or number
                // +(aas1221.00) not +(abc+a). Only if there is an operator before
                //if (openBracketIndex == 0) return;
                if (openBracketIndex > 0 && openBracketIndex < expression.Length)
                {
                    if (exprOptions.AllowedOperators.Contains(expression[openBracketIndex - 1]))
                    {
                        bool hasOnlyNamingOrNumberCharacters = true;
                        int closingBracketIndex = -1;
                        //we reached at +(...

                        for (int i = openBracketIndex + 1; i < expression.Length; i++)
                        {
                            if (expression[i] == exprOptions.ClosedBracket)
                            {
                                closingBracketIndex = i;
                                break;
                            }
                            if (!exprOptions.AllCharactersInNamesAndNumbers.Contains(expression[i]))
                            {
                                hasOnlyNamingOrNumberCharacters = false;
                                break;
                            }
                        }

                        if (hasOnlyNamingOrNumberCharacters)
                        {
                            OpenBracketRedundantIndices.Add(openBracketIndex);
                            ClosedBracketRedundantIndices.Add(closingBracketIndex);
                        }
                    }
                }
            }

            if (OpenBracketRedundantIndices.Count() == 0)
                return expression;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < expression.Count(); i++)
            {
                if (!(OpenBracketRedundantIndices.Contains(i) || ClosedBracketRedundantIndices.Contains(i)))
                {
                    sb.Append(expression[i]);
                }
            }
            return sb.ToString();
        }

        string RemoveDuplicateBrackets(string expression, ExprOptions exprOptions)
        {
            int openBracketsIndex1 = expression.IndexOf("((");

            if (openBracketsIndex1 == -1)
                return expression;
            int closingMatchIndex1 = FindClosingMatchBracket(expression, openBracketsIndex1, exprOptions);

            int closingMatchIndex2 = FindClosingMatchBracket(expression, openBracketsIndex1 + 1, exprOptions);

            if (closingMatchIndex1 > 0 && closingMatchIndex2 > 0)
            {
                if (closingMatchIndex2 == closingMatchIndex1 - 1)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < expression.Length; i++)
                        if (i != closingMatchIndex2 && i != openBracketsIndex1)
                            sb.Append(expression[i]);
                    return sb.ToString();
                }
            }
            return expression;
        }

        public override string Correct(string expression, ExprOptions exprOptions)
        {

            string newExpr = RemoveDuplicateBrackets(expression, exprOptions);

            if (newExpr == expression)
                return RemoveBracketsAroundNames(newExpr, exprOptions);
            else
                return Correct(newExpr, exprOptions);
        }
    }
}
