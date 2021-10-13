using Digitteck.ExpressionParser.ExprParameter.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.FunctionWrapper.Attributes
{
    public interface IArgumentValid
    {
        bool IsValid(ParameterObject Context);

        string ErrorMessage { get; }
    }
}
