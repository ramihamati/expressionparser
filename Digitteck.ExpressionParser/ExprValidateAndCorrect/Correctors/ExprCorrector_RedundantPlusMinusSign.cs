using Digitteck.ExpressionParser.ExprCommon.Extensions;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;
using System.Collections.Generic;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Correctors
{
    /// +-abc with -abc
    /// +-123 with -123

    public class ExprCorrector_RedundantPlusMinusSign : ExpressionCorrect
    {
        public ExprCorrector_RedundantPlusMinusSign()
        {
        }
        public override string Correct(string expression, ExprOptions exprOptions)
        {
            List<int> indices = expression.AllIndicesOf("+-");

            string newExpression = expression.TakeIf((ch, index) =>
            {
                bool test = indices.Contains(index);
                return !(test);
            }).JoinToString();

            return newExpression;
        }
    }
}
