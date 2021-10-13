using System;
using System.Collections.Generic;

namespace Digitteck.ExpressionParser.FunctionWrapper.Extensions
{
    public static class ListExtensions
    {
        public static IEnumerable<T> CheckFor<T>(this IEnumerable<T> Data, Action<IEnumerable<T>> Action)
        {
            Action(Data);
            return Data;
        }
    }
}
