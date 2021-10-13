using Digitteck.ExpressionParser.ExprParameter.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.FunctionWrapper.Runtime
{
    public class ArgumentMatchingStatus
    {
        public ParameterObject ParameterObject { get; set; }

        public string Error { get; set; }
    }
}
