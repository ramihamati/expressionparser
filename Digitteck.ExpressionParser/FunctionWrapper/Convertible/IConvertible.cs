using Digitteck.ExpressionParser.ExprParameter.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.FunctionWrapper.Convertible
{
    public interface IConvertible
    {
        Type From { get; }

        Type To { get; }

        ParameterObject Convert(ParameterObject Object);

        bool CanTransform(Type From, Type To);
    }
}
