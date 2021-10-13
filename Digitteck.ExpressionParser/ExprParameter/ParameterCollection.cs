using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExprParameter.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprParameter
{
    public class ParameterCollection
    {
        List<ParameterObject> Parameters;

        public string[] ParametersNames => Parameters.Select(x => x.Name).ToArray();

        public ParameterCollection()
        {
            this.Parameters = new List<ParameterObject>();
        }

        public ParameterCollection(params ParameterObject[] parameterObjects)
        {
            this.Parameters = parameterObjects.ToList();
        }

        public void Add(ParameterObject parameter) => this.Parameters.Add(parameter);

        public void Add(params ParameterObject[] parameters)
        {
            foreach (var parameter in parameters)
            {
                this.Add(parameter);
            }
        }
        public Maybe<ParameterObject> TryGetParameter(string name)
        {
            ParameterObject parameter = this.Parameters.Where(x => x.Name == name).FirstOrDefault();

            if (parameter == null)
                return Maybe<ParameterObject>.Create(null, false);

            return Maybe<ParameterObject>.Create(parameter, true);
        }
    }
}
