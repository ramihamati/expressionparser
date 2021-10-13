using Digitteck.ExpressionParser.ExprParameter.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.FunctionWrapper.Runtime
{
    public class RuntimeEvaluateStatus
    {
        public RuntimeArgumentStatus RuntimeArgumentStatus { get; set; }

        public string Errors { get; set; }

        public ParameterObject Result { get; set; }
    }
}
