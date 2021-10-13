using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExprCommon.Binding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprParameter.Bases
{
    public abstract class ParameterObject : IParameterObject
    {
        public string Name { get; protected set; }

        public abstract object GetValue();

        public ParameterObject(string name)
        {
            this.Name = name;
        }

        public Maybe<ParameterObject> GetValue(string mapToProperty, ExprOptions exprOptions)
        {
            Maybe<MapDataInfo> mapDataInfo
                = MapDataInfo.GetDataInfo(this.GetValue().GetType(), mapToProperty);

            if (!mapDataInfo.HasValue)
                return Maybe<ParameterObject>.Create(null, false);

            GetResponseDataInfo<object> response = mapDataInfo.Value.GetValue(this.GetValue());

            if (response.EMapResponse == EDataPathQueryResponse.NullReached)
                return Maybe<ParameterObject>.Create(null, false);

            return ParametersManager.TryGetComponent(response.Value, exprOptions);
        }


    }
}
