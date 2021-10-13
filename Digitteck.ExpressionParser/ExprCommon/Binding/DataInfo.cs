using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprCommon.Binding
{
    public class DataInfo
    {
        public EDataInfoType DataInfoType { get; set; }

        public object PropertyOrFieldInfo { get; set; }

        public object GetValue(Object Context)
        {
            return
                (DataInfoType == EDataInfoType.FieldInfo) ?
                    ((FieldInfo)PropertyOrFieldInfo).GetValue(Context) :
                    ((PropertyInfo)PropertyOrFieldInfo).GetValue(Context);
        }
        public TValue GetValue<TValue>(object Context)
        {
            return (TValue)GetValue(Context);
        }
        public void SetValue(object Context, object Value)
        {
            if (DataInfoType == EDataInfoType.FieldInfo)
                ((FieldInfo)PropertyOrFieldInfo).SetValue(Context, Value);
            else
                ((PropertyInfo)PropertyOrFieldInfo).SetValue(Context, Value);
        }
    }
}
