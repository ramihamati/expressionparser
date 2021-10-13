using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Validators
{
    /// <summary>
    /// 1. Before a closed bracket we can have a name or a closed bracket
    /// 2. After a closed bracket we can have an operator, a closed bracket, nothing at end
    /// 3. this solves the issue of empty brackets
    /// ok - (a+b+c) 
    /// not - (a+b+) | (a+b+() | (a)b | (a)(
    /// 4. After a closing bracket we can have , fpr example function((a+b),2)
    /// </summary>
    public class CharValidation_ClosingBracked : ExpressionCharValidate
    {
        protected override Proceed<char> Proceed(ExprOptions exprOptions)
        {
            Proceed<char> proceed = new Proceed<char>();
            proceed.With(exprOptions.ClosedBracket);
            return proceed;
        }
        protected override ValidationResultChar ValidateFirst
            (int index, char character, bool hasNext, char nextCharacter, ExprOptions exprOptions)
        {
            if (hasNext)
            {
                if (character == exprOptions.ClosedBracket)
                    return new ValidationResultChar
                                    (ArgumentError.ECC_ClosedBracketAtFirstPosition,
                                     character, 
                                     index,
                                     "Cannot have closing bracket at the beggining of expression");
            }
            
            return ValidationResultChar.OK;
        }

        protected override ValidationResultChar ValidateLast
            (int index, char prevCharacter, char character, ExprOptions exprOptions)
        {
            //before
            if (InvalidPrevCharacter(character, prevCharacter, exprOptions))
                return new ValidationResultChar
                        (ArgumentError.ECC_IncorectValueBeforeClosedBracket, 
                            prevCharacter, 
                            index - 1,
                            string.Format("Invalid value \'{0}\' before closing bracket at index {1}", prevCharacter, index -1));
            return ValidationResultChar.OK;
        }

        protected override ValidationResultChar ValidateMiddle
            (int index, char prevCharacter, char character, char nextCharacter, ExprOptions exprOptions)
        {
            if (character == exprOptions.ClosedBracket)
            {
                //after
                if (InvalidNextCharacter(character, nextCharacter, exprOptions))
                    return new ValidationResultChar
                        (ArgumentError.ECC_IncorectValueAfterClosedBracket, 
                         nextCharacter, 
                         index + 1,
                         string.Format("Invalid value \'{0}\' after closing bracket at index {1}", nextCharacter, index + 1));
                //before
                if (InvalidPrevCharacter(character, prevCharacter, exprOptions))
                    return new ValidationResultChar
                        (ArgumentError.ECC_IncorectValueBeforeClosedBracket, 
                         character, 
                         index - 1,
                         string.Format("Invalid value \'{0}\' before closing bracket at index {1}", prevCharacter, index - 1));
            }
            return ValidationResultChar.OK;
        }
        /// <summary>
        /// True if invalid
        /// Previous character should be a closed bracket or a name(not an operator)
        /// </summary>
        private bool InvalidPrevCharacter(char character, char prevCharacter, ExprOptions exprOptions)
        {
            if (!exprOptions.AllNamingCharacters.Contains(prevCharacter)
                && exprOptions.ClosedBracket != prevCharacter)
                return true;
            return false;
        }
        /// <summary>
        /// True if invalid
        /// Next character should be a closed bracket or a name or an operator
        /// </summary>
        private bool InvalidNextCharacter(char character, char nextCharacter, ExprOptions exprOptions)
        {
            if (!exprOptions.AllowedOperators.Contains(nextCharacter)
                 && nextCharacter != exprOptions.ClosedBracket
                 && nextCharacter != exprOptions.GroupSeparator)
                return true;
            return false;
        }
    }
}
