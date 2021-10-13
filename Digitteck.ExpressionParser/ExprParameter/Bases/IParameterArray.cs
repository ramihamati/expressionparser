using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprParameter.Bases
{
    public interface IParameterArray<T> where T : ParameterObject
    {
        //bool IsContentType(Type t);
        T[] GetObjects();
    }
}
