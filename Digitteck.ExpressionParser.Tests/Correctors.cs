using Microsoft.VisualStudio.TestTools.UnitTesting;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Correctors;
using Digitteck.ExpressionParser.ExprCommon;

namespace Digitteck.ExpressionParserTests
{
    [TestClass]
    public class Correctors
    {
        ExprOptions mExpressionOpts;

        public Correctors()
        {
            mExpressionOpts = new ExprOptions();
        }

        [TestMethod]
        public void ExprCharCorrector_ReplaceAlternativeBrackets_Test()
        {
            ExprCharCorrector_ReplaceAlternativeBrackets corrector =
                new ExprCharCorrector_ReplaceAlternativeBrackets();

            string expr1 = "[a+b{c+d(a)}]";
            Assert.AreEqual("(a+b(c+d(a)))", corrector.Correct(expr1, mExpressionOpts));
        }
      
        [TestMethod]
        public void ExprCorrector_MultiplyBeforeOpenBracket_Test()
        {
            ExprCorrector_MultiplyBeforeOpenBracket corrector =
                new ExprCorrector_MultiplyBeforeOpenBracket(new string[] { "sqrt" });
            //Corrects a(b+1) with a*(b+1) if a is not a function
            string case1 = "ab(ab+1)+sqrt(ab+1)";
            string expected1 = "ab*(ab+1)+sqrt(ab+1)";

            Assert.AreEqual(expected1, corrector.Correct(case1, mExpressionOpts));
        }
        [TestMethod]
        public void ExprCorrector_MultiplyAfterClosingBracket()
        {
            ExprCorrector_MultiplyAfterClosingBracket corrector =
                new ExprCorrector_MultiplyAfterClosingBracket();
            // Corrects (a+b)2 with (a+b)*2.
            string case1 = "(a+b)2";
            string expected1 = "(a+b)*2";
            string case2 = "(a+b)a";
            string expected2 = "(a+b)*a";
            string case3 = "(a+b)(a+b)";
            string expected3 = "(a+b)*(a+b)";

            Assert.AreEqual(expected1, corrector.Correct(case1, mExpressionOpts));
            Assert.AreEqual(expected2, corrector.Correct(case2, mExpressionOpts));
            Assert.AreEqual(expected3, corrector.Correct(case3, mExpressionOpts));
        }
        [TestMethod]
        public void ExprCorrector_PositiveSignCorrection_Test()
        {/// Correct +expr to expr
         /// Correct a+(+a) to a+(a)
            ExprCorrector_PositiveSignCorrection corrector =
                new ExprCorrector_PositiveSignCorrection();
            string case1 = "+ab-v";
            string expected1 = "ab-v";

            string case2 = "a+(+a)";
            string expected2 = "a+(a)";

            Assert.AreEqual(expected1, corrector.Correct(case1, mExpressionOpts));
            Assert.AreEqual(expected2, corrector.Correct(case2, mExpressionOpts));
        }
        [TestMethod]
        public void ExprCorrector_RemoveEnclosingParanthesis_Test()
        {
            ExprCorrector_RemoveEnclosingParanthesis corrector =
                new ExprCorrector_RemoveEnclosingParanthesis();
            string case1 = "(a+fn(b))";
            string expected1 = "a+fn(b)";

            string case2 = "(a)+(b)";
            string expected2 = "(a)+(b)";

            Assert.AreEqual(expected1, corrector.Correct(case1, mExpressionOpts));
            Assert.AreEqual(expected2, corrector.Correct(case2, mExpressionOpts));
        }
        [TestMethod]
        public void ExprCorrector_RemoveRedundantBrackets_Test()
        {
            ExprCorrector_RemoveRedundantBrackets corrector =
                new ExprCorrector_RemoveRedundantBrackets();

            string case1 = "a+(b)";
            string expected1 = "a+b";

            string case2 = "a*(1)";
            string expected2 = "a*1";

            string case3 = "a*(.2)";
            string case4 = "a*(10000.0001)";
            string case5 = "a*(__abc)";
            string case6 = "a*(__124)";
            string case7 = "a*((((__124))))";

            Assert.AreEqual(expected1, corrector.Correct(case1, mExpressionOpts));
            Assert.AreEqual(expected2, corrector.Correct(case2, mExpressionOpts));
            Assert.AreEqual("a*.2", corrector.Correct(case3, mExpressionOpts));
            Assert.AreEqual("a*10000.0001", corrector.Correct(case4, mExpressionOpts));
            Assert.AreEqual("a*__abc", corrector.Correct(case5, mExpressionOpts));
            Assert.AreEqual("a*__124", corrector.Correct(case6, mExpressionOpts));
            Assert.AreEqual("a*__124", corrector.Correct(case7, mExpressionOpts));
        }
        [TestMethod]
        public void WordCorrect_TrailingZeros_Test()
        {
            WordCorrect_TrailingZeros corrector = new WordCorrect_TrailingZeros();

            /// Corrects 0000.00 to 0
            /// Corrects 0000.1 to 0.1
            /// Corrects 1.2000 to 1.2
            /// Corrects .2 to 0.2
            /// Corrects .0 to 0
            string expression1 = "0000.00";
            string expression2 = "0000.1";
            string expression3 = "1.20000";
            string expression4 = ".2";
            string expression5 = ".0";
            string expression6 = "0.0";
            string expression7 = "100000.000001";
            string expression8 = "100000.00000";
            string expression9 = "00000.000001";
            string expression10 = ".000001";
            string expression11 = "00000.";
            string expression12 = ".00000";
            string expression13 = "100000.";
            string expression14 = "12345";
            string expression15 = "0";
            string expression16 = "4";
            string expression17 = ".";
            Assert.AreEqual("0", corrector.Correct(expression1, mExpressionOpts));
            Assert.AreEqual("0.1", corrector.Correct(expression2, mExpressionOpts));
            Assert.AreEqual("1.2", corrector.Correct(expression3, mExpressionOpts));
            Assert.AreEqual("0.2", corrector.Correct(expression4, mExpressionOpts));
            Assert.AreEqual("0", corrector.Correct(expression5, mExpressionOpts));
            Assert.AreEqual("0", corrector.Correct(expression6, mExpressionOpts));
            Assert.AreEqual("100000.000001", corrector.Correct(expression7, mExpressionOpts));
            Assert.AreEqual("100000", corrector.Correct(expression8, mExpressionOpts));
            Assert.AreEqual("0.000001", corrector.Correct(expression9, mExpressionOpts));
            Assert.AreEqual("0.000001", corrector.Correct(expression10, mExpressionOpts));
            Assert.AreEqual("0", corrector.Correct(expression11, mExpressionOpts));
            Assert.AreEqual("0", corrector.Correct(expression12, mExpressionOpts));
            Assert.AreEqual("100000", corrector.Correct(expression13, mExpressionOpts));
            Assert.AreEqual("12345", corrector.Correct(expression14, mExpressionOpts));
            Assert.AreEqual("0", corrector.Correct(expression15, mExpressionOpts));
            Assert.AreEqual("4", corrector.Correct(expression16, mExpressionOpts));
            Assert.AreEqual(".", corrector.Correct(expression17, mExpressionOpts));
        }
        [TestMethod]
        public void ExprCorrector_RedundantPlusSign_Test()
        {
            ExprCorrector_RedundantPlusSign corrector = new ExprCorrector_RedundantPlusSign();

            string expression1 = "(abc+abc)";
            string expression2 = "(+abc+abc)";
            string expression3 = "(+123+abc)";
            string expression4 = "(abc+abc+(+0.22))";
            string expression5 = "(abc+abc+(+.22))";
            string expression6 = "(abc+abc+(+1.))";

            Assert.AreEqual(expression1, corrector.Correct(expression1, mExpressionOpts));
            Assert.AreEqual("(abc+abc)", corrector.Correct(expression2, mExpressionOpts));
            Assert.AreEqual("(123+abc)", corrector.Correct(expression3, mExpressionOpts));
            Assert.AreEqual("(abc+abc+(0.22))", corrector.Correct(expression4, mExpressionOpts));
            Assert.AreEqual("(abc+abc+(.22))", corrector.Correct(expression5, mExpressionOpts));
            Assert.AreEqual("(abc+abc+(1.))", corrector.Correct(expression6, mExpressionOpts));
        }
        [TestMethod]
        public void ExprCorrector_RedundantPlusMinusSign_Test()
        {
            ExprCorrector_RedundantPlusMinusSign corrector = new ExprCorrector_RedundantPlusMinusSign();

            string expression1 = "(abc+abc)";
            string expression2 = "(+-abc+abc)";
            string expression3 = "(+-123+abc)";
            string expression4 = "(abc+abc+(+-0.22))";
            string expression5 = "(abc+abc+(+-.22))";
            string expression6 = "(abc+abc+(+-1.))";

            Assert.AreEqual(expression1, corrector.Correct(expression1, mExpressionOpts));
            Assert.AreEqual("(-abc+abc)", corrector.Correct(expression2, mExpressionOpts));
            Assert.AreEqual("(-123+abc)", corrector.Correct(expression3, mExpressionOpts));
            Assert.AreEqual("(abc+abc+(-0.22))", corrector.Correct(expression4, mExpressionOpts));
            Assert.AreEqual("(abc+abc+(-.22))", corrector.Correct(expression5, mExpressionOpts));
            Assert.AreEqual("(abc+abc+(-1.))", corrector.Correct(expression6, mExpressionOpts));
        }
        [TestMethod]
        public void ExprCorrector_RedundantMinusPlusSign_Test()
        {
            ExprCorrector_RedundantMinusPlusSign corrector = new ExprCorrector_RedundantMinusPlusSign();

            string expression1 = "(abc+abc)";
            string expression2 = "(-+abc+abc)";
            string expression3 = "(-+123+abc)";
            string expression4 = "(abc+abc+(-+0.22))";
            string expression5 = "(abc+abc+(-+.22))";
            string expression6 = "(abc+abc+(-+1.))";

            Assert.AreEqual(expression1, corrector.Correct(expression1, mExpressionOpts));
            Assert.AreEqual("(-abc+abc)", corrector.Correct(expression2, mExpressionOpts));
            Assert.AreEqual("(-123+abc)", corrector.Correct(expression3, mExpressionOpts));
            Assert.AreEqual("(abc+abc+(-0.22))", corrector.Correct(expression4, mExpressionOpts));
            Assert.AreEqual("(abc+abc+(-.22))", corrector.Correct(expression5, mExpressionOpts));
            Assert.AreEqual("(abc+abc+(-1.))", corrector.Correct(expression6, mExpressionOpts));
        }
        [TestMethod]
        public void WordCorrect_MultSignAfterClosedBracket_Test()
        {
            WordCorrect_MultSignAfterClosedBracket corrector =
                new WordCorrect_MultSignAfterClosedBracket();
            string expr1 = "(a)2";// => (a)*2
            string expr2 = "(1.2)2";// => (1.2) * 2
            string expr3 = "(a)b";// => (a) * b
            string expr4 = "(1.2)b";// => (1.2) * 2
            string expr5 = "(.2).2";
            string expr6 = "(.2)b+(b).2+(a(b)1.2)";
            Assert.AreEqual("(a)*2", corrector.Correct(expr1, mExpressionOpts));
            Assert.AreEqual("(1.2)*2", corrector.Correct(expr2, mExpressionOpts));
            Assert.AreEqual("(a)*b", corrector.Correct(expr3, mExpressionOpts));
            Assert.AreEqual("(1.2)*b", corrector.Correct(expr4, mExpressionOpts));
            Assert.AreEqual("(.2)*.2", corrector.Correct(expr5, mExpressionOpts));
            Assert.AreEqual("(.2)*b+(b)*.2+(a(b)*1.2)", corrector.Correct(expr6, mExpressionOpts));
        }
        [TestMethod]
        public void ExprCorrector_CleanEmptySpaces_Test()
        {
            ExprCorrector_CleanEmptySpaces corrector = new ExprCorrector_CleanEmptySpaces();
            string expr1 = "a + b + c";
            string expr2 = "(a + c+(1) ";
            Assert.AreEqual("a+b+c", corrector.Correct(expr1, mExpressionOpts));
            Assert.AreEqual("(a+c+(1)", corrector.Correct(expr2, mExpressionOpts));
        }
    }
}
