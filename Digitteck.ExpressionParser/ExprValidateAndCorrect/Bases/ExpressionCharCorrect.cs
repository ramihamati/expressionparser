
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases
{
    public abstract class ExpressionCharCorrect : ICorrector
    {
        private char[] charArray;

        /// Defines when the correctors takes place
        public ExpressionCharCorrect()
        {
        }

        public string Correct(string expression, ExprOptions exprOptions)
        {
            ///Conversion to char array in order to replace characters at the index where they are corrected
            charArray = expression.ToCharArray();

            for (int i = 0; i < expression.Length; i++)
            {
                char currentCharacter = expression[i];
                NewChar newChar;
                if (i == 0)
                {
                    if (expression.Length > 1)
                        newChar = CorrectFirst(i, currentCharacter, true, expression[1], exprOptions);
                    else
                        newChar = CorrectFirst(i, currentCharacter, false, default(char), exprOptions);
                }
                else if (i == expression.Length - 1 && expression.Length > 1)
                {
                    newChar = Correctlast(i, expression[i - 1], currentCharacter, exprOptions);
                }
                else
                {
                    newChar = CorrectMiddle(i, expression[i - 1], currentCharacter, expression[i + 1], exprOptions);
                }

                if (newChar.HasNewChar)
                    charArray[i] = newChar.Value;
            }
            return string.Concat<char>(charArray);
        }
        /// <summary>
        /// Method to be implemented by child. Will be called to correct the character if the character is at index = 0
        /// </summary>
        protected abstract NewChar CorrectFirst(
            int index,
            char character,
            bool hasNext,
            char nextCharacter,
            ExprOptions mExpressionOpts);
        /// <summary>
        /// Method to be implemented by child. Will be called to correct the character if the character is not at index 0 or last index
        /// </summary>
        protected abstract NewChar CorrectMiddle(
            int index,
            char prevCharacter,
            char character,
            char nextCharacter,
            ExprOptions mExpressionOpts
            );
        /// <summary>
        /// Method to be implemented by child. Will be called to validate the character if the character is at last index
        /// </summary>
        protected abstract NewChar Correctlast(
            int index,
            char prevCharacter,
            char character,
            ExprOptions mExpressionOpts
            );
    }
}
