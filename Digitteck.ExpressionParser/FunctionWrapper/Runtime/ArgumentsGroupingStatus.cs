using Digitteck.ExpressionParser.ExprParameter.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.FunctionWrapper.Runtime
{
    public class ArgumentsGroupingStatus
    {
        public List<ParameterObject> ParameterObjects { get; set; }

        public string Error { get; set; }

        public bool IsValid { get; set; }
    }
}
