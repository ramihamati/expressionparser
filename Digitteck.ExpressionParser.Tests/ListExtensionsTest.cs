using Digitteck.ExpressionParser.ExprCommon.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParserTests
{
    [TestClass]
    public class ListExtensionsTest
    {
        [TestMethod]
        public void TakeIf_Test()
        {
            string expression = "abcdef";

            string result = string.Concat(expression.TakeIf((ch, idx) => ch != 'b'));

            Assert.AreEqual("acdef", result);
        }
    }
}
