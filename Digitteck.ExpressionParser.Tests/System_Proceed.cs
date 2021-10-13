using Digitteck.ExpressionParser.ExprCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digitteck.ExpressionParserTests
{
    [TestClass]
    public class System_Proceed
    {
        [TestMethod]
        public void System_Proceed_Test()
        {
            Proceed<int> proceed = new Proceed<int>();

            proceed.If((value) => value % 2 == 0);
            proceed.If(value => value > 5);

            List<int> values = Enumerable.Range(0, 10).ToList();

            string test(List<int> _values)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var nr in _values)
                {
                    if (proceed.ShouldProceed(nr))
                        sb.Append(nr);
                }
                return sb.ToString();
            }

            string test2(List<int> _values)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var nr in _values)
                {
                    if (proceed.ShouldNotProceed(nr))
                        continue;
                    sb.Append(nr);
                }
                return sb.ToString();
            }
            Assert.AreEqual("68", test(values));
            Assert.AreEqual("68", test2(values));

        }
        [TestMethod]
        public void System_Proceed_Test_WithAny()
        {
            Proceed<char> proceedChar = new Proceed<char>();

            proceedChar.WithAny(new List<char> { 'a', 'b', 'c' });

            var expression1 = "abcdefgh";

            StringBuilder sb = new StringBuilder();
            foreach (var ch in expression1)
            {
                var notProceed = proceedChar.ShouldNotProceed(ch);
                if (notProceed)
                    continue;
                sb.Append(ch);
            }
            Assert.AreEqual("abc", sb.ToString());
        }
        [TestMethod]
        public void System_Proceed_Test_Determiners()
        {
            Proceed<char> proceedChar = new Proceed<char>();

            proceedChar.If((ch) => ch == 'a' || ch == 'c');
            var expression1 = "abcdefgh";

            StringBuilder sb = new StringBuilder();
            foreach (var ch in expression1)
            {
                var notProceed = proceedChar.ShouldNotProceed(ch);
                if (notProceed)
                    continue;
                sb.Append(ch);
            }
            Assert.AreEqual("ac", sb.ToString());
        }
        [TestMethod]
        public void System_Proceed_Test_WithWithAnyDeterminers()
        {
            Proceed<int> proceedChar = new Proceed<int>();
            //any of these
            proceedChar.With(1);
            proceedChar.WithAny(new List<int> { 2, 3, 5, 7, 9 });
            //condition
            proceedChar.If((nr) => nr > 5);

            var values = Enumerable.Range(0,20);

            StringBuilder sb = new StringBuilder();
            foreach (int nr in values)
            {
                var notProceed = proceedChar.ShouldNotProceed(nr);
                if (notProceed)
                    continue;
                sb.Append(nr.ToString());
            }
            Assert.AreEqual("79", sb.ToString());
        }
    }
}
