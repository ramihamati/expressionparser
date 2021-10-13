using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprCommon
{
    public class Maybe<T>
    {
        public T Value { get; private set; }

        public bool HasValue { get; private set; }

        private Maybe()
        {
        }

        public static Maybe<T> Create(T value, bool hasValue)
        {
            return new Maybe<T> { Value = value, HasValue = hasValue };
        }
    }
}
