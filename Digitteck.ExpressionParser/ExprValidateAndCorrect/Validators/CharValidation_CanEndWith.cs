using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Validators
{
    /// <summary>
    /// Can end with a closed bracket or a name character
    /// </summary>
    public class CharValidation_CanEndWith : ExpressionCharValidate
    {
        protected override ValidationResultChar 
            ValidateFirst(int index, char character, bool hasNext, char nextCharacter, ExprOptions exprOptions)
        {
            return ValidationResultChar.OK;
        }

        protected override ValidationResultChar ValidateLast
            (int index, char prevCharacter, char character, ExprOptions mExpressionOpts)
        {
            if (!mExpressionOpts.AllNamingCharacters.Contains(character) 
                && character != mExpressionOpts.ClosedBracket)
                return new ValidationResultChar
                    (ArgumentError.ECC_InvalidEndCharacter, 
                        character, 
                        index,
                        string.Format("Invalid character \'{0}\' at end of expression", character));
            return ValidationResultChar.OK;
        }

        protected override ValidationResultChar ValidateMiddle
            (int index, char prevCharacter, char character, char nextCharacter, ExprOptions mExpressionOpts)
        {
            return ValidationResultChar.OK;
        }
    }
}
