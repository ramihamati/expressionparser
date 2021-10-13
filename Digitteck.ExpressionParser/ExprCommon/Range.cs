using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprCommon
{
    public abstract class Range<T> : IComparer<T>
    {
        public T MinValue { get; private set; }
        public T MaxValue { get; private set; }

        protected Range(T minValue, T maxValue)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }

        public bool isInRange(T Value)
        {
            var a = this.Compare(Value, MinValue);
            var b = this.Compare(Value, MaxValue);

            bool testLeft = this.Compare(Value, MinValue) ==-1;
            bool testRight = this.Compare(Value, MaxValue) == 1;

            return testLeft == false &&  testRight == false;
        }

        public abstract int Compare(T x, T y);
    }
}
