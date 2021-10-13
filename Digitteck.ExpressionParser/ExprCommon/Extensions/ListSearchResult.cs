using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprCommon.Extensions
{
    public struct ListSearchResult<T>
    {
        public int Index { get; set; }
        public T Found { get; set; }
    }
}
