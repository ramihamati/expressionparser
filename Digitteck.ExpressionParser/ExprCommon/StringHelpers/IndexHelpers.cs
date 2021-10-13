using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprCommon.StringHelpers
{
    public static class IndexHelpers
    {
        /// -1 = no closing bracket
        /// -2 = internal error - not open bracket at indx
        public static int FindIndexOfMatchingClosedBracket(string context, int openBracketPosition, ExprOptions exprOptions)
        {
            if (context[openBracketPosition] != exprOptions.OpenBracket)
                return -2;
            if (context.Length <= 1)
                return -1;

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

        /// expression level is the position where no xpressions are found. 
        /// for expression sqrt(2,(2)) given openBracketPosition at 4 and endBracketPosition at 10 the list is 5,6
        public static List<int> FindIndicesAtExpressionLevel(string context, int openBracketPosition, int endBracketPosition, ExprOptions exprOptions)
        {
            List<Int32> indices = new List<int>();
            //assume we start after the bracket started. for the above example at index 5
            int openBracketCount = 1;
            char OpenBracket = exprOptions.OpenBracket;
            char ClosedBracket = exprOptions.ClosedBracket;

            for (int i = openBracketPosition + 1; i < endBracketPosition; i++)
            {
                char current = context[i];
                if (current == OpenBracket) { openBracketCount++; continue; }
                if (current == ClosedBracket) { openBracketCount--; continue; }
                if (openBracketCount == 1)
                    indices.Add(i);
            }
            return indices;
        }
    }
}
