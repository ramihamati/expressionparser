using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExprCommon.Extensions;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using System.Linq;

namespace Digitteck.ExpressionParser.ExprCommon.Validators
{
    public class WordValidate_UnderscoreAlone : ExpressionWordValidate
    {
        protected override Proceed<string> Proceed(ExprOptions exprOptions)
        {
            Proceed<string> proceed = new Proceed<string>();
            proceed.If(x => x.Contains(exprOptions.AllowedNamePrefix));
            return proceed;
        }

        protected override ValidationResultWord
            ValidateName(string name, int startIndex, int endIndex, string expression, ExprOptions exprOptions)
        {
            bool test1 = name.ContainsAny(exprOptions.AllowedLetters);
            bool test2 = name.ContainsAny(exprOptions.AllowedNumbers);
            if (!test1 && !test2)
                return new ValidationResultWord
                    (ArgumentError.ECC_IncorectUnderscore, name, startIndex, endIndex,
                    string.Format("Invalid underscore at index {0}", startIndex));

            return ValidationResultWord.OK;
        }
    }
}
