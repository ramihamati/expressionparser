using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprCommon.Warning
{
    public class Warning
    {
        string Message { get; set; }

        public Warning(string message)
        {
            this.Message = message;
        }
    }
}
