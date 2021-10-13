using Digitteck.ExpressionParser.ExprCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Digitteck.ExpressionParser.ExprValidateAndCorrect;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class EvaluatorTest
    {
        string[] fnNames = new string[] { "sqrt", "tan", "cos" };
        string[] paramNames = new string[] { "volume", "area", "perimeter", "Rooms", "a", "b", "c", "d", "abc" };
        string[] paramNamesDuplicateName = new string[] { "volume", "area", "tan" };
        ExprOptions exprOptions = new ExprOptions();
        IEvaluator evaluator;
        IEvaluator duplicateNamingEvaluator;

        public EvaluatorTest()
        {
            evaluator = EvaluatorFactory.ExpressionEvaluator(fnNames, paramNames);
            duplicateNamingEvaluator = EvaluatorFactory.ExpressionEvaluator(fnNames, paramNamesDuplicateName);
        }
        [TestMethod]
        public void EvaluatorShouldEvaluate()
        {
            Func<string, string, ArgumentError, Tuple<string, string, ArgumentError>> createT
                = (str1, str2, ae) => Tuple.Create<string, string, ArgumentError>(str1, str2, ae);

            List<Tuple<string, string, ArgumentError>> data = new List<Tuple<string, string, ArgumentError>>
            {
                createT("volume + area + perimeter","volume+area+perimeter",ArgumentError.OK),
                createT("[a+b*{c+d*(a)}]","a+b*(c+d*a)",ArgumentError.OK),
                createT("(a+b)*2/(a-b*(3+c))+a^2+sqrt(2)","(a+b)*2/(a-b*(3+c))+a^2+sqrt(2)",ArgumentError.OK),
                createT("[a+b{c+d(a)}]","a+b*(c+d*a)",ArgumentError.OK),
                createT("sqrt(a+cos(b))","sqrt(a+cos(b))",ArgumentError.OK),
                createT("a(b+1)+sqrt(a+1)","a*(b+1)+sqrt(a+1)",ArgumentError.OK),
                createT("(a+b)(a+b)","(a+b)*(a+b)",ArgumentError.OK),
                createT("+b-d","b-d",ArgumentError.OK),
                createT("(a+cos(b))","a+cos(b)",ArgumentError.OK),
                createT("a*(10000.0001) + 100000.00000+(+abc+abc)+(+-abc)","a*10000.0001+100000+(abc+abc)+(-abc)",ArgumentError.OK),
                createT("a+b+","a+b+",ArgumentError.ECC_InvalidEndCharacter),
                createT("+a+b","a+b",ArgumentError.OK),
                createT("(a+b+()","(a+b+",ArgumentError.EC_OpenClosedBracketsCountMatch_NoPass),
                createT("((((a+b))))","a+b",ArgumentError.OK),
                createT("(a+(a)/-sqrt(2.4)*2^2)","a+a/-sqrt(2.4)*2^2",ArgumentError.ECC_IncorectValueAfterOperator),
                createT("a+sqrt(2,(2))","a+sqrt(2,(2))", ArgumentError.OK),
                createT("a,sqrt(2,(2))","a,sqrt(2,(2))", ArgumentError.ECC_UnexpectedComma),
                createT("sqrt(2,(2,3))","sqrt(2,(2,3))", ArgumentError.ECC_UnexpectedComma),
                //createT("sqrt(2,sqrt(2,3))","sqrt(2,sqrt(2,3))", ArgumentError.ECC_UnexpectedComma),
                createT("Rooms.Lengh", "Rooms.Lengh", ArgumentError.OK),
                //createT("","",ArgumentError.OK),
            };
            foreach (var item in data)
            {
                EvaluationResult result = evaluator.Evaluate(item.Item1, this.exprOptions);
                Assert.AreEqual(item.Item3, result.ValidationResult.ArgumentError);
                Assert.AreEqual(item.Item2, result.CorrectedContext);
            }
        }
    }
}
