using Digitteck.ExpressionParser.ExpressionComponentTree.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprModel
{
    public class EvaluateComponentResult<T> where T: ExprComponent
    {
        public T ExprComponent { get; set; }

        public bool IsValid { get; set; }

        public string ErrorMessage { get; set; }

        public static implicit operator EvaluateComponentResult<T>(EvaluateComponentResult<ExprParameterComponent> data)
        {
            return new EvaluateComponentResult<T>
            {
                ErrorMessage = data.ErrorMessage,
                ExprComponent =  data.ExprComponent as T,
                IsValid = data.IsValid
            };
        }
    }
}
