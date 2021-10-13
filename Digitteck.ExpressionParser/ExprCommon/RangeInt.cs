using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprCommon
{
    public class RangeInt : Range<int>
    {
        protected RangeInt(int minValue, int maxValue) : base(minValue, maxValue)
        {

        }

        public static RangeInt Create(int minValue, int maxValue) => new RangeInt(minValue, maxValue);

        public override int Compare(int x, int y)
        {
            return x.CompareTo(y);
        }
    }
}
