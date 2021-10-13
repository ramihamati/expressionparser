using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Validators
{
    /// <summary>
    /// can be (1,2,3) (1,(a),c)
    /// , near name / number
    /// , after closingbracekt
    /// , before closingbracket
    /// </summary>
    public class CharValidation_ArgumentSplit : ExpressionCharValidate
    {

        protected override Proceed<char> Proceed(ExprOptions exprOptions)
        {
            Proceed<char> proceed = new Proceed<char>();

            proceed.With(exprOptions.GroupSeparator);

            return proceed;
        }

        bool BeforeASplitter(char character, char previousCharacter,
                ExprOptions exprOptions)
        {
            if (!exprOptions.AllNamingCharacters.Contains(previousCharacter)
                && !(previousCharacter == exprOptions.ClosedBracket))
                return false;
            return true;
        }

        bool AfterASplitter(char character, char nextCharacter, ExprOptions exprOptions)
        {
            if (!exprOptions.AllNamingCharacters.Contains(nextCharacter)
                && nextCharacter != exprOptions.OpenBracket)
                return false;
            return true;
        }
        protected override ValidationResultChar ValidateFirst(int index, char character, bool hasNext, char nextCharacter, ExprOptions exprOptions)
        {
            return new ValidationResultChar
                (ArgumentError.ECC_InvalidCommaAtExpressionBegining, 
                    character, 
                    0,
                    string.Format("Invalid Comma at index 0"));
        }

        protected override ValidationResultChar ValidateLast(int index, char prevCharacter, char character, ExprOptions exprOptions)
        {
            return new ValidationResultChar
                        (ArgumentError.ECC_InvalidCommaAtExpressionEnd, 
                            character, 
                            index,
                            string.Format("Invalid comma at index {0}", index));
        }

        protected override ValidationResultChar ValidateMiddle(int index, char prevCharacter, char character, char nextCharacter, ExprOptions exprOptions)
        {
            bool isValidBefore = BeforeASplitter(character, prevCharacter, exprOptions);
            bool isValidAfter = AfterASplitter(character, nextCharacter, exprOptions);

            if (!isValidBefore)
                return new ValidationResultChar
                    (ArgumentError.ECC_InvalidValueBeforeComma, 
                        prevCharacter, 
                        index - 1,
                        string.Format("Invalid value \'{0}\' before comma at index {1}", prevCharacter, index-1));
            if (!isValidAfter)
                return new ValidationResultChar
                    (ArgumentError.ECC_InvalidValueAfterComma, 
                        nextCharacter, 
                        index + 1,
                        string.Format("Invalid value \'{0}\' after comma at index {1}", nextCharacter, index+1));

            return ValidationResultChar.OK;
        }
    }
}
