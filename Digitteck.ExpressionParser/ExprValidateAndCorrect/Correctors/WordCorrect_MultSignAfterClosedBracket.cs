using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Correctors
{
    /// <summary>
    /// (a)2 => (a)*2
    /// (1.2)2 => (1.2) * 2
    /// (a)b => (a) * b
    /// (1.2)2 => (1.2) * 2
    /// </summary>
    public sealed class WordCorrect_MultSignAfterClosedBracket : ExpressionWordCorrect
    {
        public WordCorrect_MultSignAfterClosedBracket()
        {

        }
        //protected override Proceed<string> Proceed(MExpressionOpts mExpressionOpts)
        //{
        //    ///if word is a name then it will be validated eitherway
        //    ///proceed only for numbers. Even if the number has multiple dots, this will not
        //    ///be further validated
        //    Proceed<string> proceed = new Proceed<string>();
        //    proceed.If(x => mExpressionOpts.HasNumericFormat(x));
        //    return proceed;
        //}

        protected override NewWord CorrectWord
            (string word, int startIndex, int endIndex, string context, ExprOptions mExpressionOpts)
        {
            if (startIndex > 0)
            {
                if (context[startIndex - 1] == mExpressionOpts.ClosedBracket)
                {
                    string @return = "*" + word;
                    return new NewWord { HasNewWord = true,Value = @return};
                }
            }
            return new NewWord { HasNewWord = false };
        }
    }
}
