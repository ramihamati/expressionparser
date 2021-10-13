using Microsoft.VisualStudio.TestTools.UnitTesting;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Helper;
using System.Text.RegularExpressions;

namespace Digitteck.ExpressionParserTests
{
    [TestClass]
    public class StringDestructureTest
    {
        [TestMethod]
        public void StringDestructure_Test()
        {
            Regex regex = new Regex("[0-9]");

            StringDestructure sd = 
                StringDestructure.By("abc123def456", (ch) => regex.Match(ch.ToString()).Success);
            sd.OnConditionMet((str, startIdx, endIdx) => str);
            sd.OnConditionNotMet((str, startIdx, endIdx) => str.ToUpper());
            string result = sd.Recompose();

            Assert.AreEqual("ABC123DEF456", result);

            StringDestructure sd2 =
               StringDestructure.By("(a)2", (ch) => regex.Match(ch.ToString()).Success);
            sd2.OnConditionMet((str, startIdx, endIdx) => "*"+str);
            string result2 = sd2.Recompose();
            Assert.AreEqual("(a)*2", result2);

            StringDestructure sd3 =
                StringDestructure.By("abc123def456", (ch) => regex.Match(ch.ToString()).Success);
            string startIndices_ocm = "";
            string endIndices_ocm = "";
            string startIndices_ocnm = "";
            string endIndices_ocnm = "";

            sd3.OnConditionMet((str, startIdx, endIdx) => 
            {
                startIndices_ocm += startIdx.ToString();
                endIndices_ocm += endIdx.ToString();
                return str;
            });
            sd3.OnConditionNotMet((str, startIdx, endIdx) => {
                startIndices_ocnm += startIdx.ToString();
                endIndices_ocnm += endIdx.ToString();
                return str;
            });
            sd3.Recompose();
            Assert.AreEqual("39", startIndices_ocm);
            Assert.AreEqual("511", endIndices_ocm);
            Assert.AreEqual("06", startIndices_ocnm);
            Assert.AreEqual("28", endIndices_ocnm);
        }
    }
}
