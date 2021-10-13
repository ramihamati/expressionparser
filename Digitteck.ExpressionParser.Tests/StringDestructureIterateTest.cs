
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Helper;
using Digitteck.ExpressionParser.ExprCommon;
using System.Linq;
using System.Text.RegularExpressions;

namespace Digitteck.ExpressionParserTests
{
    [TestClass]
    public class StringDestructureIterateTest
    {
        [TestMethod]
        public void StringDestructureIterate_Test()
        {
            Regex regex = new Regex("[0-9]");

            string startIndices_ocm = "";
            string endIndices_ocm = "";
            string startIndices_ocnm = "";
            string endIndices_ocnm = "";
            string partialsMeetCondition = "";
            string partialsDoNotMeetCondition = "";

            StringDestructureIterate sd =
                StringDestructureIterate.By("abc123def456", (ch) => regex.Match(ch.ToString()).Success);

            sd.ForEachIfConditionMet((str, startIdx, endIdx) =>
            {
                startIndices_ocm += startIdx.ToString();
                endIndices_ocm += endIdx.ToString();
                partialsMeetCondition += str;
            });

            sd.ForEachIfConditionNotMet((str, startIdx, endIdx) =>
            {
                startIndices_ocnm += startIdx.ToString();
                endIndices_ocnm += endIdx.ToString();
                partialsDoNotMeetCondition += str;
            });
            sd.Eval();

            Assert.AreEqual("39", startIndices_ocm);
            Assert.AreEqual("511", endIndices_ocm);
            Assert.AreEqual("06", startIndices_ocnm);
            Assert.AreEqual("28", endIndices_ocnm);
            Assert.AreEqual("abcdef", partialsDoNotMeetCondition);
            Assert.AreEqual("123456", partialsMeetCondition);
        }
        [TestMethod]
        public void StringDestructureIterate_Test2()
        {
            Regex regex = new Regex("[0-9]");
            ExprOptions exprOptions = new ExprOptions();

            string startIndices_ocm = "";
            string endIndices_ocm = "";
            string startIndices_ocnm = "";
            string endIndices_ocnm = "";
            string partialsMeetCondition = "";
            string partialsDoNotMeetCondition = "";

            StringDestructureIterate sd =
                StringDestructureIterate.By("a+sqrt(volume+tan(c)/atan(area))", 
                    (ch) =>exprOptions.AllCharactersInNamesAndNumbers.Contains(ch));

            sd.ForEachIfConditionMet((str, startIdx, endIdx) =>
            {
                startIndices_ocm += startIdx.ToString();
                endIndices_ocm += endIdx.ToString();
                partialsMeetCondition += str;
            });

            sd.ForEachIfConditionNotMet((str, startIdx, endIdx) =>
            {
                startIndices_ocnm += startIdx.ToString();
                endIndices_ocnm += endIdx.ToString();
                partialsDoNotMeetCondition += str;
            });
            sd.Eval();

            Assert.AreEqual("asqrtvolumetancatanarea", partialsMeetCondition);
            Assert.AreEqual("+(+()/())", partialsDoNotMeetCondition);
            Assert.AreEqual("02714182126", startIndices_ocm);
            Assert.AreEqual("051216182429", endIndices_ocm);
            Assert.AreEqual("161317192530", startIndices_ocnm);
            Assert.AreEqual("161317202531", endIndices_ocnm);
        }
    }
}
