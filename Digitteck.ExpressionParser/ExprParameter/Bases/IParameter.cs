using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprParameter
{
    public interface IParameter<T> : IParameterObject where T: IParameter<T>
    {
        T Op_Add(T other);
        T Op_Minus(T other);
        T Op_Mult(T other);
        T Op_Div(T other);
        T Op_Mod(T other);
        T Op_Pow(T other);
    }
}
