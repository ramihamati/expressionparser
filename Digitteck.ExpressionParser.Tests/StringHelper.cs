using Microsoft.VisualStudio.TestTools.UnitTesting;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Correctors;
using Digitteck.ExpressionParser.ExprCommon;
using System.Collections.Generic;
using Digitteck.ExpressionParser.ExprCommon.StringHelpers;

namespace Helpers
{
    [TestClass]
    public class StringHelper
    {
        ExprOptions mExpressionOpts;

        public StringHelper()
        {
            mExpressionOpts = new ExprOptions();
        }

        [TestMethod]
        public void FindIndicesAtExpressionLevel_Test()
        {
            string expr = "sqrt(2,(2))";
            int openBracketPos = 4;
            int endBracketPos = 10;
            List<int> indices = IndexHelpers.FindIndicesAtExpressionLevel(expr, openBracketPos, endBracketPos, mExpressionOpts);

            CollectionAssert.AreEqual(new List<int> { 5, 6 }, indices);
        }
    }
}
