
using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;

namespace Digitteck.ExpressionParser.ExprCommon.Validators
{
    /// <summary>
    /// 1. Before an open bracket we can have an operator, an open bracket, nothing if it's first or naming (fn)
    /// 2. After an open bracket we can have an open bracket, naming, + or minus sign
    /// 3. Before an open bracket we can have , as in function(2,(a+b))
    /// </summary>
    public class CharValidation_OpeningBracket : ExpressionCharValidate
    {
        protected override Proceed<char> Proceed(ExprOptions mExpressionOpts)
        {
            Proceed<char> proceed = new Proceed<char>();
            proceed.With(mExpressionOpts.OpenBracket);
            return proceed;
        }
        /// <summary>
        /// returns true if it's invalid
        /// </summary>
        public bool IsBeforeInvalid(char previousCharacter, char character, ExprOptions exprOptions)
        {
            if (exprOptions.AllowedOperators.Contains(previousCharacter)
                  || exprOptions.OpenBracket == previousCharacter
                  || exprOptions.AllNamingCharacters.Contains(previousCharacter)
                  || exprOptions.GroupSeparator == previousCharacter)
                return false;
            return true;
        }
        /// returns true if is invalid
        public bool IsAfterInvalid(char nextCharacter, char character, ExprOptions exprOptions)
        {
            if (exprOptions.AllNamingCharacters.Contains(nextCharacter)
                || nextCharacter == exprOptions.OpenBracket
                || nextCharacter == exprOptions.OperatorPlusSign
                || nextCharacter == exprOptions.OperatorMinusSign)
                return false;
            return true;
        }

        protected override ValidationResultChar ValidateFirst
            (int index, char character, bool hasNext, char nextCharacter, ExprOptions exprOptions)
        {
            if (hasNext)
            {
                if (IsAfterInvalid(nextCharacter, character, exprOptions))
                    return new ValidationResultChar
                        (ArgumentError.ECC_IncorectValueAfterOpenBracket, 
                            nextCharacter, 
                            index + 1,
                            string.Format("Invalid value \'{0}\' after open bracket at the beginning", nextCharacter));
            }
            
            return ValidationResultChar.OK;
        }

        protected override ValidationResultChar ValidateLast
            (int index, char prevCharacter, char character, ExprOptions exprOptions)
        {
            if (character == exprOptions.OpenBracket)   
                return new ValidationResultChar
                    (ArgumentError.ECC_OpenBracketAtLastPosition, 
                     character, 
                     index,
                     string.Format("Open bracket cannot be at the end of expression"));
            return ValidationResultChar.OK;
        }

        protected override ValidationResultChar ValidateMiddle
            (int index, char prevCharacter, char character, char nextCharacter, ExprOptions exprOptions)
        {
            //before
            if (IsBeforeInvalid(prevCharacter, character, exprOptions))
                return new ValidationResultChar
                    (ArgumentError.ECC_IncorectValueBeforeOpenBracket, character, index,
                    string.Format("Invalid value \'{0}\' before open bracket at the beginning", prevCharacter));

            //after
            if (IsAfterInvalid(nextCharacter, character, exprOptions))
                return new ValidationResultChar
                    (ArgumentError.ECC_IncorectValueAfterOpenBracket, character, index,
                     string.Format("Invalid value \'{0}\' after open bracket at the beginning", nextCharacter));

            return ValidationResultChar.OK;
        }
    }
}
