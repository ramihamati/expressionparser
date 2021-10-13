using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprCommon.Binding
{
    public class GetResponseDataInfo<T>
    {
        public EDataPathQueryResponse EMapResponse { get; set; }
        public T Value { get; set; }
        public DataInfo DataInfo { get; set; }
    }
}
