using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.FunctionWrapper.Runtime
{
    public enum RuntimeArgumentStatus
    {
        OK,
        //InvalidNoOfArguments,
        //ArgumentsMismatch,
        ValuesNotValidated,
        //NullArgumentPassed,
        NoMathingSignatureFound,
        NaN,
        RuntimeError,
        NullResult,
        UnableToCast
    }
}
