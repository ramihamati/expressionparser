using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprParameter
{
    public interface IParameterObject
    {
        string Name { get; }

        object GetValue();
    }
}
