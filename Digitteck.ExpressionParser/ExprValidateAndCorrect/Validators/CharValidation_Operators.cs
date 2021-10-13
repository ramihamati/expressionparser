using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Validators
{
    /// <summary>
    /// Cannot have (+( this is allowed in brackets because (+a is valid and a+( is valid
    /// Cannot have )+)
    /// Cannot have ++ or +-*/%) or (/*& or (* or )1) 
    /// Before an operator can have name/number/open bracket if -/closedbracket
    /// After an operator can have name/number
    /// </summary>
    public class CharValidation_Operators : ExpressionCharValidate
    {

        protected override Proceed<char> Proceed(ExprOptions exprOptions)
        {
            Proceed<char> proceed = new Proceed<char>();

            proceed.WithAny(exprOptions.AllowedOperators);

            return proceed;
        }

        bool BeforeAnOperator(char character, char previousCharacter,
                ExprOptions exprOptions)
        {
            if (!exprOptions.AllNamingCharacters.Contains(previousCharacter)
                && !(previousCharacter == exprOptions.OpenBracket && character == exprOptions.OperatorMinusSign)
                && !(previousCharacter == exprOptions.ClosedBracket))
                return false;
            return true;
        }

        bool AfterAnOperator(char character, char nextCharacter, ExprOptions exprOptions)
        {
            if (!exprOptions.AllNamingCharacters.Contains(nextCharacter)
                && nextCharacter != exprOptions.OpenBracket)
                return false;
            return true;
        }
        protected override ValidationResultChar ValidateFirst(int index, char character, bool hasNext, char nextCharacter, ExprOptions exprOptions)
        {
            bool isValid = true;
            if (hasNext)
            {
                isValid = AfterAnOperator(character, nextCharacter, exprOptions);
            }
            //index + 1 is ok even for expression with 1 item because hasNext enforces to have more then 1
            return (isValid == true) ? ValidationResultChar.OK
                    : new ValidationResultChar(ArgumentError.ECC_IncorectValueAfterOperator, 
                            nextCharacter, 
                            index + 1,
                            string.Format(string.Format("Invalid value \'{0}\' after an operator at index {1}", nextCharacter, index +1)));
        }

        protected override ValidationResultChar ValidateLast(int index, char prevCharacter, char character, ExprOptions exprOptions)
        {
            bool isValid = BeforeAnOperator(character, prevCharacter, exprOptions);

            return (isValid == true) ?
                ValidationResultChar.OK : 
                new ValidationResultChar(
                        ArgumentError.ECC_IncorectValueBeforeOperator, 
                        prevCharacter, 
                        index - 1,
                        string.Format("Invalid value \'{0}\' before an operator at index {1}", prevCharacter, index - 1));
        }

        protected override ValidationResultChar ValidateMiddle(int index, char prevCharacter, char character, char nextCharacter, ExprOptions exprOptions)
        {
            bool isValidBefore = BeforeAnOperator(character, prevCharacter, exprOptions);
            bool isValidAfter = AfterAnOperator(character, nextCharacter, exprOptions);

            if (!isValidBefore)
                return new ValidationResultChar
                    (ArgumentError.ECC_IncorectValueBeforeOperator, 
                     prevCharacter, 
                     index - 1,
                     string.Format("Invalid value \'{0}\' before an operator at index {1}", prevCharacter,index - 1));
            if (!isValidAfter)
                return new ValidationResultChar
                    (ArgumentError.ECC_IncorectValueAfterOperator, nextCharacter, index + 1,
                    string.Format("Invalid value \'{0}\' after an operator at index {1}", nextCharacter, index + 1));

            return ValidationResultChar.OK;
        }
    }
}
