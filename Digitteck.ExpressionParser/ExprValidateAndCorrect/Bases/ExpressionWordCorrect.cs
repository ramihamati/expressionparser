using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Helper;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases
{
    public abstract class ExpressionWordCorrect : ICorrector
    {

        /// <summary>
        /// Getting only words - includes names and numbers (numbers, letters, underscore and .)
        /// </summary>
        protected abstract NewWord CorrectWord(string word,
                                               int startIndex, 
                                               int endIndex, 
                                               string context,
                                               ExprOptions mExpressionOpts);

        // Will proceed only with words that pass this testing. Is Proceed Is null then this
        /// condition is ignored
        private Proceed<string> ProceedWith;

        protected virtual Proceed<string> Proceed(ExprOptions mExpressionOpts)
        {
            return null;
        }

        public ExpressionWordCorrect()
        {
        }

        public string Correct(string expression, ExprOptions mExpressionOpts)
        {
            this.ProceedWith = Proceed(mExpressionOpts);

            StringDestructure stringDestructure =
                StringDestructure.By(expression,
                    ch => mExpressionOpts.AllCharactersInNamesAndNumbers.Contains(ch));

            stringDestructure.OnConditionMet((str, startIndex, endIndex) =>
            {
                bool _proceed = true;
                if (this.ProceedWith != null)
                {
                    if (this.ProceedWith.ShouldNotProceed(str))
                    {
                        _proceed = false;
                    }
                }
                if (_proceed)
                {
                    NewWord newWord =
                         CorrectWord(str, startIndex, endIndex, expression, mExpressionOpts);
                    if (newWord.HasNewWord)
                        return newWord.Value;
                }
                return str;
            });

            return string.Join("", stringDestructure.Recompose());
        }
    }
}
