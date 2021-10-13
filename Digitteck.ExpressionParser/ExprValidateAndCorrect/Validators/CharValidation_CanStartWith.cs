using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Validators
{
    /// <summary>
    /// Can start with an open bracket or a name
    /// </summary>
    public class CharValidation_CanStartWith : ExpressionCharValidate
    {
        protected override ValidationResultChar 
            ValidateFirst(int index, char character, bool hasNext, char nextCharacter, ExprOptions exprOptions)
        {
            if (!exprOptions.AllNamingCharacters.Contains(character) 
                && character != exprOptions.OpenBracket
                && character != exprOptions.OperatorMinusSign)
                return new ValidationResultChar
                    (ArgumentError.ECC_InvalidStartCharacter, 
                     character, 
                     index,
                     string.Format("Invalid character \'{0}\' at the beginning of expression", character));

            return ValidationResultChar.OK;
        }

        protected override ValidationResultChar ValidateLast
            (int index, char prevCharacter, char character, ExprOptions exprOptions)
        {
            return ValidationResultChar.OK;
        }

        protected override ValidationResultChar ValidateMiddle
            (int index, char prevCharacter, char character, char nextCharacter, ExprOptions exprOptions)
        {
            return ValidationResultChar.OK;
        }
    }
}
