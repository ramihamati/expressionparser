using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprCommon.Binding
{
 
    /// <summary>
    /// Will store a list of every property/field info set in a binding string.
    /// GetDataInfo(typeof(Person), "Car.Brand.Model")
    ///     => 1. DataInfo { PropertyOrFieldInfo = Car, DataInfoType = PropertyInfo }
    ///     => 2. DataInfo { PropertyOrFieldInfo = Brand, DataInfoType = FieldInfo }
    ///     => etc.
    /// </summary>
    public class MapDataInfo
    {
        public IList<DataInfo> Map { get; private set; }

        private MapDataInfo(string bindPath)

        {
            this.BindPath = bindPath;

            Map = new List<DataInfo>();
        }

        public string BindPath { get; private set; }
        /// <summary>
        /// Use the mapping to retrieve a far-away property
        /// </summary>
        public GetResponseDataInfo<object> GetValue(object Context)
        {
            return GetValue<object>(Context);
        }
        /// <summary>
        /// use the mapping to retrieve a far-away property and cast it to TValue
        /// </summary>
        public GetResponseDataInfo<TValue> GetValue<TValue>(object Context)
        {
            object lastContext = Context;
            for (short i = 0; i < Map.Count(); i++)
            {
                DataInfo dataInfo = Map[i];
                var t = dataInfo.PropertyOrFieldInfo.GetType();
                lastContext = dataInfo.GetValue(lastContext);
                //is the context is null and it's not the end of the path
                //accessing next property will throw an exception. 
                if (lastContext == null && i != Map.Count() - 1)
                {
                    return new GetResponseDataInfo<TValue>
                    {
                        EMapResponse = EDataPathQueryResponse.NullReached,
                        Value = default(TValue),
                        DataInfo = dataInfo
                    };
                }
            }
            return new GetResponseDataInfo<TValue>
            {
                Value = (TValue)lastContext,
                EMapResponse = EDataPathQueryResponse.OK,
                DataInfo = Map.Last()
            };
        }

       
        public static Maybe<MapDataInfo> GetDataInfo(Type type, string binding)
        {
            string[] _params = binding.Split('.');
            PropertyInfo propertyInfo = null, LastPropertyInfo = null;
            FieldInfo fieldInfo = null, LastFieldInfo = null;
            Type LastType = type;

            MapDataInfo mapDataInfo = new MapDataInfo(binding);

            for (var i = 0; i < _params.Count(); i++)
            {
                string _param = _params[i];

                propertyInfo = LastType.GetProperty
                    (_param, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                fieldInfo = LastType.GetField
                    (_param, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

                if (propertyInfo != null)
                {
                    LastPropertyInfo = propertyInfo;
                    LastType = propertyInfo.PropertyType;
                    LastFieldInfo = null;
                    mapDataInfo.Map.Add(new DataInfo
                    {
                        DataInfoType = EDataInfoType.PropertyInfo,
                        PropertyOrFieldInfo = propertyInfo
                    });
                }
                else if (fieldInfo != null)
                {
                    LastPropertyInfo = null;
                    LastFieldInfo = fieldInfo;
                    LastType = fieldInfo.FieldType;
                    mapDataInfo.Map.Add(new DataInfo
                    {
                        PropertyOrFieldInfo = fieldInfo,
                        DataInfoType = EDataInfoType.FieldInfo
                    });
                }
                else
                {
                    return Maybe<MapDataInfo>.Create(null, false);
                }
            }
            return Maybe<MapDataInfo>.Create(mapDataInfo, true);
        }
    }
}
