using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases
{
    public abstract class ExpressionCharValidate : IValidator
    {
        private Proceed<char> ProceedWithCharacter;

        //limit character under testing
        protected virtual Proceed<char> Proceed(ExprOptions mExpressionOpts)
        {
            return null;
        }

        /// <summary>
        /// Iterate each character of the expressing and call the appropiate function
        /// </summary>
        public ValidationResult Validate(string expression, ExprOptions mExpressionOpts)
        {
            this.ProceedWithCharacter = Proceed(mExpressionOpts);

            ValidationResultChar @return = ValidationResultChar.OK;

            for (int i = 0; i < expression.Length; i++)
            {
                char ch = expression[i];
                ///if proceed is defined - then test only characters passing the test
                if (this.ProceedWithCharacter!= null)
                    if (this.ProceedWithCharacter.ShouldNotProceed(ch))
                        continue;

                if (i == 0)
                {
                    ValidationResultChar result;
                    if (expression.Length > 1)
                        result = ValidateFirst(i, ch, true, expression[1], mExpressionOpts);
                    else
                        result = ValidateFirst(i, ch, false, default(char), mExpressionOpts);

                    if (result.ArgumentError != ArgumentError.OK)
                        return result;
                }
                //last only if not first
                else if (i == expression.Length - 1 && expression.Length > 1)
                {
                    ValidationResultChar result = ValidateLast(i, expression[i - 1], ch, mExpressionOpts);
                    if (result.ArgumentError != ArgumentError.OK)
                        return result;
                }
                else
                {
                    ValidationResultChar result = ValidateMiddle(i, expression[i - 1], ch, expression[i + 1], mExpressionOpts);
                    if (result.ArgumentError != ArgumentError.OK)
                        return result;
                }
            }
            return @return;
        }

        /// <summary>
        /// Method to be implemented by child. Will be called to validate the character if 
        /// the character is at index = 0
        /// </summary>
        protected abstract ValidationResultChar ValidateFirst(
              int index,
              char character,
              bool hasNext,
              char nextCharacter,
              ExprOptions mExpressionOpts);

        /// <summary>
        /// Method to be implemented by child. Will be called to validate the character 
        /// if the character is not at index 0 or last index
        /// </summary>
        protected abstract ValidationResultChar ValidateMiddle(
           int index,
           char prevCharacter,
           char character,
           char nextCharacter,
           ExprOptions mExpressionOpts);

        /// <summary>
        /// Method to be implemented by child. Will be called to validate the character 
        /// if the character is at last index
        /// </summary>
        protected abstract ValidationResultChar ValidateLast(
            int index,
            char prevCharacter,
            char character,
            ExprOptions mExpressionOpts);
    }
}