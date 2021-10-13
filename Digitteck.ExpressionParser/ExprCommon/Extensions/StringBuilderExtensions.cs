using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.MExpression.Extensions
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder Reverse(this StringBuilder @this)
        {
            StringBuilder builder = new StringBuilder();

            for (var i = @this.Length - 1; i >= 0; i--)
                builder.Append(@this[i]);
            return builder;
        }
    }
}
