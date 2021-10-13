using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprCommon.TypeHelper
{
    public static class TypeHelper
    {
        public static bool MatchesTypeOrAncestorBaseType(Type actual, Type required)
        {
            if (actual.BaseType is null)
                return false;
            if (actual.BaseType.Equals(required))
                return true;

            return MatchesTypeOrAncestorBaseType(actual.BaseType, required);
        }
    }
}
