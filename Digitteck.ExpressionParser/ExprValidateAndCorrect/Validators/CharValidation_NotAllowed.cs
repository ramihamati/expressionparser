using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Validators
{
    /// <summary>
    /// Returns true if the character is allowed
    /// </summary>
    public class CharValidation_NotAllowed : ExpressionCharValidate
    {
        protected override ValidationResultChar ValidateFirst
            (int index, char character, bool hasNext, char nextCharacter, ExprOptions exprOptions)
        {
            if (exprOptions.AllAllowedCharacters.Contains(character))
                return ValidationResultChar.OK;
            else
                return new ValidationResultChar
                    (ArgumentError.ECC_CharIsAllowed_NoPass, 
                     character, 
                     index,
                     string.Format("Character \'{0}\' at the beggining of expression", character));
        }

        protected override ValidationResultChar ValidateLast
            (int index, char prevCharacter, char character, ExprOptions exprOptions)
        {
            if (exprOptions.AllAllowedCharacters.Contains(character))
                return ValidationResultChar.OK;
            else
                return new ValidationResultChar
                    (ArgumentError.ECC_CharIsAllowed_NoPass, 
                     character, 
                     index,
                     string.Format("Character \'{0}\' at expression end not allowed", character));
        }

        protected override ValidationResultChar ValidateMiddle
            (int index, char prevCharacter, char character, char nextCharacter, ExprOptions exprOptions)
        {
            if (exprOptions.AllAllowedCharacters.Contains(character))
                return ValidationResultChar.OK;
            else
                return new ValidationResultChar
                    (ArgumentError.ECC_CharIsAllowed_NoPass, 
                     character, 
                     index,
                     string.Format("Character \'{0}\' at index {1} not allowed", character, index ));
        }
    }
}
