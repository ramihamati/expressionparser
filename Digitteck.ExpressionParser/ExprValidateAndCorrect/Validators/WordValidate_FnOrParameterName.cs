using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using System.Linq;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Validators
{
    /// <summary>
    /// checks if a name exists in as function or parameter
    /// </summary>
    public class WordValidate_FnOrParameterName : ExpressionWordValidate
    {
        private string[] FnNames;
        private string[] ParameterNames;

        protected override Proceed<string> Proceed(ExprOptions exprOptions)
        {
            Proceed<string> proceed = new Proceed<string>();
            proceed.If((str) => !exprOptions.ContainsNumbersAndPoint(str));
            return proceed;
        }

        public WordValidate_FnOrParameterName(string[] fnNames, string[] parameterNames)
        {
            this.FnNames = fnNames;
            this.ParameterNames = parameterNames;
        }

        protected override ValidationResultWord ValidateName
            (string name, int startIndex, int endIndex, string expression, ExprOptions exprOptions)
        {
            //a name may come as Rooms.Length.OtherProperty. Only Rooms should be checked
            string finalName;
            //if contains a punctuation but does not have only numbers. It can have numbers and points
            //and this will be unvalidated
            if (name.Contains(exprOptions.DecimalSeparator)
                && !exprOptions.ContainsNumbersAndPoint(name))
            {
                finalName = name.Split(exprOptions.DecimalSeparator).First();
            }
            else
            {
                finalName = name;
            }
            if (!(FnNames.Contains<string>(finalName)) && !(ParameterNames.Contains<string>(finalName)))
                return new ValidationResultWord
                    (ArgumentError.EWD_NameNotFunctionOrParameter, name, startIndex, endIndex,
                        string.Format("Provided word \'{0}\' at index {1} is not a function or a parameter name", name, startIndex));
            return ValidationResultWord.OK;
        }
    }
}
