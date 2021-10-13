using Digitteck.ExpressionParser.ExprParameter.Bases;
using Digitteck.ExpressionParser.FunctionWrapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.FunctionWrapper.Base
{
    public class ArgumentValidators
    {
        private List<IArgumentValid> _validators;

        public ArgumentValidators()
        {
            _validators = new List<IArgumentValid>();
        }

        public ArgumentValidators(params IArgumentValid[] argumentValidators)
        {
            _validators = new List<IArgumentValid>();
            foreach (IArgumentValid validator in argumentValidators)
                _validators.Add(validator);
        }

        public void Add(IArgumentValid validator)
        {
            this._validators.Add(validator);
        }

        /// <summary>
        /// Tests an object against lists of validators
        /// </summary>
        public ValidatorsResult CheckDataValidity(ParameterObject data)
        {
            foreach (IArgumentValid validate in _validators)
            {
                if (!validate.IsValid(data))
                    return new ValidatorsResult(false, string.Format("Parameter Name = {0} Value ={1} ValidationError={2}"
                                    , data.Name, data.GetValue(), validate.ErrorMessage));
            }

            return ValidatorsResult.OK;
        }
    }
}
