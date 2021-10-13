using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using System.Linq;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Validators
{ 
    /// <summary>
   /// allowed function(...) not parameter(...)
   /// </summary>
    public class WordValidate_FnOpenBracket : ExpressionWordValidate
    {
        private string[] FnNames;
        private string[] ParameterNames;

        protected override Proceed<string> Proceed(ExprOptions exprOptions)
        {
            Proceed<string> proceed = new Proceed<string>();
            proceed.If((str) => !exprOptions.HasNumericFormat(str));
            return proceed;
        }

        public WordValidate_FnOpenBracket(string[] fnNames, string[] parameterName)
        {
            this.FnNames = fnNames;
            this.ParameterNames = parameterName;
        }
        /// A function must be followed by an open bracket
        /// A parameter cannot be followed by an open bracket
        protected override ValidationResultWord ValidateName
            (string name, int startIndex, int endIndex, string expression, ExprOptions exprOptions)
        {
            //if we have a name followed by an open bracket and the name is not a function
            if (expression.Length - 1 > endIndex + 1)
            {
                if (!FnNames.Contains(name) && !ParameterNames.Contains(name)
                    && expression[endIndex + 1] == exprOptions.OpenBracket)
                {
                    return new ValidationResultWord
                        (ArgumentError.EWD_NameIsNotFunction, name, startIndex, endIndex,
                        string.Format("Provided word \'{0}\' at index {1} is not a function", name, startIndex));
                }
            }
            if (FnNames.Contains<string>(name))
            {
                //if last index
                if (endIndex == expression.Length - 1)
                    return new ValidationResultWord
                        (ArgumentError.EWD_FunctionWithNoBracket, name, startIndex, endIndex,
                         string.Format("Provided function \'{0}\' at index {1} has no brackets", name, startIndex));
                if (!(expression[endIndex + 1] == exprOptions.OpenBracket))
                    return new ValidationResultWord
                        (ArgumentError.EWD_FunctionWithNoBracket, name, startIndex, endIndex,
                        string.Format("Provided function \'{0}\' at index {1} has no brackets", name, startIndex));
                return ValidationResultWord.OK;
            }

            if (ParameterNames.Contains<string>(name))
            {
                if (endIndex != expression.Length - 1)
                    if (expression[endIndex + 1] == exprOptions.OpenBracket)
                        return new ValidationResultWord
                            (ArgumentError.EWD_ParameterIsNotAFunction, name, startIndex, endIndex,
                            string.Format("Provided word \'{0}\' at index {1} is a parameter not a function", name, startIndex));
            }
            return ValidationResultWord.OK;
        }
    }
}
