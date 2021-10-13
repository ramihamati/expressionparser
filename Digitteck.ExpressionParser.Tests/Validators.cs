using Digitteck.ExpressionParser.ExprCommon.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Validators;
using Digitteck.ExpressionParser.ExprCommon;
using System.Collections.Generic;

namespace Digitteck.ExpressionParserTests
{
    [TestClass]
    public class Validators
    {
        ExprOptions exprOptions;
        public Validators()
        {
            exprOptions = new ExprOptions();
        }
        [TestMethod]
        public void CharValidation_CanEndWith_Test()
        {
            /// Can end with a closed bracket or a name character
            CharValidation_CanEndWith validator = new CharValidation_CanEndWith();

            string expression = "a+b+";
            string expression2 = "a+b";
            string expression3 = "(a+b)";

            Assert.AreEqual<ArgumentError>(ArgumentError.ECC_InvalidEndCharacter,
                validator.Validate(expression, exprOptions).ArgumentError);
            Assert.AreEqual<ArgumentError>(ArgumentError.OK,
                    validator.Validate(expression2, exprOptions).ArgumentError);
            Assert.AreEqual<ArgumentError>(ArgumentError.OK,
                    validator.Validate(expression3, exprOptions).ArgumentError);
        }
        [TestMethod]
        public void CharValidation_CanStartWith_Test()
        {
            CharValidation_CanStartWith validator = new CharValidation_CanStartWith();
            string expression1 = "+a+b";
            string expression2 = "a+b";
            string expression3 = "(a+b)";

            Assert.AreEqual<ArgumentError>(ArgumentError.ECC_InvalidStartCharacter,
                    validator.Validate(expression1, exprOptions).ArgumentError);
            Assert.AreEqual<ArgumentError>(ArgumentError.OK,
                    validator.Validate(expression2, exprOptions).ArgumentError);
            Assert.AreEqual<ArgumentError>(ArgumentError.OK,
                    validator.Validate(expression3, exprOptions).ArgumentError);
        }
        [TestMethod]
        public void CharValidation_ClosedBracked_Test()
        {
            CharValidation_ClosingBracked validator = new CharValidation_ClosingBracked();
            ///ok - (a + b + c)
            /// not - (a+b+) | (a+b+() | (a)b | (a)(
            string expr1 = "(a(a))";
            string expr2 = "(a+b+c)";
            string expr3 = "(a+b+)";
            string expr4 = "(a+b+()";
            string expr5 = "(a)b";
            string expr6 = "(a)(";
            string expr7 = "sqrt((1),2)";
            Assert.AreEqual(ArgumentError.OK, validator.Validate(expr1, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK, validator.Validate(expr2, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.ECC_IncorectValueBeforeClosedBracket, validator.Validate(expr3, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.ECC_IncorectValueBeforeClosedBracket, validator.Validate(expr4, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.ECC_IncorectValueAfterClosedBracket, validator.Validate(expr5, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.ECC_IncorectValueAfterClosedBracket, validator.Validate(expr6, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK, validator.Validate(expr7, exprOptions).ArgumentError);
        }
        [TestMethod]
        public void CharValidation_NotAllowed_Test()
        {
            CharValidation_NotAllowed validator = new CharValidation_NotAllowed();

            string allowed = "QWERTYUIOPLKJHGFDSAZXCVBNMqwertyuiop()_asdfghjklzxcvbnm.1234567890-+*/%^,";
            string notAllowed = "[]{}!@#$&<>~`?";
            Assert.AreEqual(ArgumentError.OK, validator.Validate(allowed, exprOptions).ArgumentError);

            foreach (char c in notAllowed)
            {
                Assert.AreEqual(ArgumentError.ECC_CharIsAllowed_NoPass,
                                validator.Validate(string.Format("{0}", c), exprOptions).ArgumentError);
            }
        }
        [TestMethod]
        public void CharValidation_OpenBracket_Test()
        {
            CharValidation_OpeningBracket validator = new CharValidation_OpeningBracket();
            /// 1. Before an open bracket we can have an operator, an open bracket, 
            /// nothing if it's first or naming (fn)
            /// 2. After an open bracket we can have an open bracket, naming, + or minus sign
            string expr1 = "((a+b))";
            string expr2 = "(a+b+c)";
            string expr3 = ")(a+b+)";
            string expr4 = "(a+b+()";
            string expr5 = ".(a)";
            string expr6 = "b(a)"; //ok because it can be a function
            string expr7 = "fn(2,(2+2))";
            Assert.AreEqual(ArgumentError.OK, validator.Validate(expr1, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK, validator.Validate(expr2, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.ECC_IncorectValueBeforeOpenBracket,
                validator.Validate(expr3, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.ECC_IncorectValueAfterOpenBracket,
                validator.Validate(expr4, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.ECC_IncorectValueBeforeOpenBracket,
                validator.Validate(expr5, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK,
                validator.Validate(expr6, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK,
               validator.Validate(expr7, exprOptions).ArgumentError);
        }
        [TestMethod]
        public void CharValidation_Operators_Test()
        {
            CharValidation_Operators validator = new CharValidation_Operators();

            string test1 = "(a+b+c)";

            string test2 = "((A+b+(a)+sqrt(2.4)*2^2)";
            string test3 = "((+b+(a)+sqrt(2.4)*2^2)";
            string test4 = "((A+(a)+-sqrt(2.4)*2^2)";
            string test5 = "((A+(+)+sqrt(2.4)*2^2)";
            string test6 = "(*A+(a)+sqrt(2.4)*2^2)";
            string test7 = "(-A+(a)+sqrt(2.4)*2^2)";
            string test8 = "(1+2+3)";
            Assert.AreEqual(ArgumentError.OK, validator.Validate(test1, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK, validator.Validate(test2, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.ECC_IncorectValueBeforeOperator, validator.Validate(test3, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.ECC_IncorectValueAfterOperator, validator.Validate(test4, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.ECC_IncorectValueBeforeOperator, validator.Validate(test5, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.ECC_IncorectValueBeforeOperator, validator.Validate(test6, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK, validator.Validate(test7, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK, validator.Validate(test8, exprOptions).ArgumentError);
        }
        [TestMethod]
        public void CharValidation_ArgumentSplit_Test()
        {
            CharValidation_ArgumentSplit validator = new CharValidation_ArgumentSplit();

            string expr1 = "1,2,3";
            string expr2 = "(1),2,3";
            string expr3 = "1,2,(3)";
            string expr4 = "1,2,";
            string expr5 = ",1,2,3,4";
            string expr6 = "(,2,3,4)";
            string expr7 = "(1,2,3,)";

            Assert.AreEqual(ArgumentError.OK, validator.Validate(expr1, this.exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK, validator.Validate(expr2, this.exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK, validator.Validate(expr3, this.exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.ECC_InvalidCommaAtExpressionEnd, validator.Validate(expr4, this.exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.ECC_InvalidCommaAtExpressionBegining, validator.Validate(expr5, this.exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.ECC_InvalidValueBeforeComma, validator.Validate(expr6, this.exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.ECC_InvalidValueAfterComma, validator.Validate(expr7, this.exprOptions).ArgumentError);
        }
        [TestMethod]
        public void ExprValidate_ArgumentSplit_Test()
        {
            string[] fnNames = new string[] { "sqrt", "tan" };

            ExprValidate_ArgumentSplit validator = new ExprValidate_ArgumentSplit(fnNames);

            string expression1 = "a+sqrt(2,(2))";
            string expression2 = "a,sqrt(2,(2))";
            string expression3 = "a+sqrt(2,(2,2))";
            string expression4 = "a+sqrt(2,tan(2,2))";
            string expression5 = "a+sqrt(2,tan(2,,2))";//double , in expr in checked in CharValidation_ArgumentSplit
            string expression6 = "a+sqrt(2,,tan(2,2))";//double , in expr in checked in CharValidation_ArgumentSplit

            Assert.AreEqual(ArgumentError.OK, validator.Validate(expression1, this.exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.ECC_UnexpectedComma, validator.Validate(expression2, this.exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.ECC_UnexpectedComma, validator.Validate(expression3, this.exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK, validator.Validate(expression4, this.exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK, validator.Validate(expression5, this.exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK, validator.Validate(expression6, this.exprOptions).ArgumentError);
        }
        [TestMethod]
        public void ExprValidate_BracketPositioning_Test()
        {
            ExprValidate_BracketPositioning validator = new ExprValidate_BracketPositioning();
            //it checks if closed brackets are met before openbrackets are placed
            string expression1 = "(a+b)";
            string expression2 = "(a+b)("; //ok because it doesnot handle open brackets
            string expression3 = ")";
            string expression4 = "(a+b))(";

            Assert.AreEqual(ArgumentError.OK,
                            validator.Validate(expression1, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK,
                            validator.Validate(expression2, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.EC_ClosedBracketHasNoOpenBracketMatch,
                            validator.Validate(expression3, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.EC_ClosedBracketHasNoOpenBracketMatch,
                            validator.Validate(expression4, exprOptions).ArgumentError);
        }
        [TestMethod]
        public void ExprValidate_EmptyExpression_Test()
        {
            ExprValidate_EmptyExpression validator = new ExprValidate_EmptyExpression();

            string expression1 = "1";
            string expression2 = "";
            Assert.AreEqual(ArgumentError.OK,
                            validator.Validate(expression1, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.EC_EmptyExpression,
                            validator.Validate(expression2, exprOptions).ArgumentError);
        }
        [TestMethod]
        public void ExprValidate_NoStartWithMultOrDiv_Test()
        {
            ExprValidate_NoStartWithMultOrDiv validator = new ExprValidate_NoStartWithMultOrDiv();

            string expression1 = "a+b+c";
            string expression2 = "*a+b+c";
            string expression3 = "/(a+b+c)";

            Assert.AreEqual(ArgumentError.OK,
                            validator.Validate(expression1, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.EC_NoStartWithMultDivMod,
                            validator.Validate(expression2, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.EC_NoStartWithMultDivMod,
                            validator.Validate(expression3, exprOptions).ArgumentError);
        }
        [TestMethod]
        public void ExprValidate_OpenClosedBracketsCountMatch_Test()
        {
            ExprValidate_OpenClosedBracketsCountMatch validator =
                new ExprValidate_OpenClosedBracketsCountMatch();
            string expression1 = "a+(b+c+d(a))";
            string expression2 = "a+(b+c+d(a)+a(1+ab(1+c)))";
            string expression3 = "a+(b+c+d(a))(";
            string expression4 = "a+(b+c+d(a)))";
            Assert.AreEqual(ArgumentError.OK, validator.Validate(expression1, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK, validator.Validate(expression2, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.EC_OpenClosedBracketsCountMatch_NoPass,
                validator.Validate(expression3, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.EC_OpenClosedBracketsCountMatch_NoPass,
                validator.Validate(expression4, exprOptions).ArgumentError);
        }
        [TestMethod]
        public void WordValidate_FnOpenBracket_Test()
        {
            List<string> functionNames = new List<string> { "sqrt", "tan", "atan" };
            List<string> parameterNames = new List<string> { "area", "volume" };

            WordValidate_FnOpenBracket validator =
                new WordValidate_FnOpenBracket(functionNames.ToArray(), parameterNames.ToArray());
            string expression1 = "a+sqrt(volume+tan(c)/atan(area))";
            string expression2 = "a+sqrt(tan+tan(c)/atan(area))";
            string expression3 = "a+sqrt(volume+volume(c)/atan(area))";
            //because this below is not a function - a * sign will be added before (2)
            //with corrector multsignbeforeclosedbracket
            string expression4 = "(a+b)*2/(a-b*(3+c))+a^2+sqrt1(2)";

            Assert.AreEqual(ArgumentError.OK, validator.Validate(expression1, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.EWD_FunctionWithNoBracket,
                                validator.Validate(expression2, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.EWD_ParameterIsNotAFunction,
                                validator.Validate(expression3, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.EWD_NameIsNotFunction,
                                validator.Validate(expression4, exprOptions).ArgumentError);
        }
        [TestMethod]
        public void WordValidate_FnOrParameterName_Test()
        {
            List<string> functionNames = new List<string> { "sqrt", "tan", "atan" };
            List<string> parameterNames = new List<string> { "a", "b", "c", "area", "volume", "Rooms" };
            WordValidate_FnOrParameterName validator =
                new WordValidate_FnOrParameterName(functionNames.ToArray(), parameterNames.ToArray());

            string expression1 = "a+sqrt(volume+tan(c)/atan(area))+22";
            string expression2 = "a+_sqrt(volume+tan(c)/atan(area))";
            string expression3 = "a+sqrt(_volume+tan(c)/atan(area))";

            string expression4 = "(a+b)*2/(a-b*(3+c))+a^2+sqrt(2)";
            string expression5 = "Rooms.Length";

            Assert.AreEqual(ArgumentError.OK, validator.Validate(expression1, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.EWD_NameNotFunctionOrParameter,
                            validator.Validate(expression2, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.EWD_NameNotFunctionOrParameter,
                            validator.Validate(expression3, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK,
                              validator.Validate(expression4, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK,
                              validator.Validate(expression5, exprOptions).ArgumentError);
        }
        [TestMethod]
        public void WordValidate_Punctuation_Test()
        {
            /// EWD_NoPunctuationsInNames
            /// EWD_MultiplePunctuationSignsNotAllowed
            /// EWD_NumberCannotStartWithPunctuation
            /// EWD_NumberCannotEndWithPunctuation

            WordValidate_Punctuation validator =
                new WordValidate_Punctuation();
            string expression1 = "abc.+21";
            string expression2 = "0.2+21+abc";
            string expression3 = "0..2+21";
            string expression4 = "21 + a..b";
            string expression5 = "abc+2.1.4";
            string expression6 = "a.b.c+32";
            string expression7 = "a+.2";
            string expression8 = "21.+abc";
            string expression9 = ".";

            Assert.AreEqual(ArgumentError.EWD_ExprCannotEndWithPunctuation,
                    validator.Validate(expression1, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK,
                    validator.Validate(expression2, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.EWD_MultiplePunctuationSignsNotAllowed,
                    validator.Validate(expression3, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.EWD_MultiplePunctuationSignsNotAllowed,
                    validator.Validate(expression4, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.EWD_MultiplePunctuationSignsNotAllowed,
                    validator.Validate(expression5, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK,
                    validator.Validate(expression6, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.EWD_ExprCannotStartWithPunctuation,
                    validator.Validate(expression7, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.EWD_ExprCannotEndWithPunctuation,
                    validator.Validate(expression8, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.EWD_ExprCannotStartWithPunctuation,
                   validator.Validate(expression9, exprOptions).ArgumentError);
        }
        [TestMethod]
        public void WordValidate_UnderscoreAlone_Test()
        {
            WordValidate_UnderscoreAlone validator = new WordValidate_UnderscoreAlone();
            /// Checks for a+_+b; _ or __ alone is not allowed;
            /// ___123 - is allowed
            string expression1 = "_";
            string expression2 = "a+_+_b";
            string expression3 = "a+b+_";
            string expression4 = "_+a+b";
            string expression5 = "a_+b + sqrt(b_)";
            string expression6 = "__b+a__+fn(__a+b__)";

            Assert.AreEqual(ArgumentError.ECC_IncorectUnderscore,
                validator.Validate(expression1, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.ECC_IncorectUnderscore,
                validator.Validate(expression2, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.ECC_IncorectUnderscore,
                validator.Validate(expression3, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.ECC_IncorectUnderscore,
                validator.Validate(expression4, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK,
               validator.Validate(expression5, exprOptions).ArgumentError);
            Assert.AreEqual(ArgumentError.OK,
               validator.Validate(expression6, exprOptions).ArgumentError);
        }
    }
}
