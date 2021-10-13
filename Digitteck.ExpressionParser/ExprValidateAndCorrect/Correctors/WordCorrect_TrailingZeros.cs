using Digitteck.ExpressionParser.MExpression.Extensions;
using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExprCommon.Extensions;
using System;
using System.Linq;
using System.Text;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Correctors
{
    /// <summary>
    /// Corrects 0000.00 to 0
    /// Corrects 0000.1 to 0.1
    /// Corrects 1.2000 to 1.2
    /// Corrects .2 to 0.2
    /// Corrects .0 to 0
    /// </summary>
    public sealed class WordCorrect_TrailingZeros : ExpressionWordCorrect
    {
        public WordCorrect_TrailingZeros()
        {

        }
        protected override Proceed<string> Proceed(ExprOptions exprOptions)
        {
            ///if word is a name then it will be validated eitherway
            ///proceed only for numbers. Even if the number has multiple dots, this will not
            ///be further validated
            Proceed<string> proceed = new Proceed<string>();
            proceed.If(x => exprOptions.HasNumericFormat(x));
            return proceed;
        }

        private string FirstIsPunctuation(string expression)
        {
            return '0' + expression;
        }
        private string LastIsPunctuation(string expression)
        {
            string @return;

            if (expression.Length == 1)
                @return = "0";
            else
            {
                @return = string.Concat(expression.Take(expression.Length - 1).ToList());
            }
            return @return;
        }
        private string LeftTrailing(string expression, ExprOptions exprOptions)
        {
            bool _startedCollecting = false;
            StringBuilder _finalNameBuilder = new StringBuilder();
            for (int i = 0; i < expression.Length; i++)
            {
                if (_startedCollecting)
                    _finalNameBuilder.Append(expression[i]);
                else
                {
                    if (expression[i] != '0')
                    {
                        _startedCollecting = true;
                        _finalNameBuilder.Append(expression[i]);
                    }
                    //if there is a next character then check if it's a point. If yes then add current 0
                    else if ((i + 1) < expression.Length)
                    {
                        if (expression[i + 1] == exprOptions.DecimalSeparator)
                        {
                            _finalNameBuilder.Append(expression[i]);
                            _startedCollecting = true;
                        }
                    }
                }
            }
            return _finalNameBuilder.ToString();
        }
        private string RightTrailing(string expression, ExprOptions exprOptions)
        {
            bool startedCollecting = false;
            StringBuilder finalNameBuilder = new StringBuilder();
            for (int i = expression.Length - 1; i >= 0; i--)
            {
                if (startedCollecting)
                    finalNameBuilder.Append(expression[i]);
                else
                {
                    //0001.000 from right to left we skip punctuation if no number is detected
                    //10000.00
                    if (expression[i] != '0' && expression[i] != exprOptions.DecimalSeparator)
                    {
                        startedCollecting = true;
                        finalNameBuilder.Append(expression[i]);
                    }
                    //reached . in 1000.00
                    else if (expression[i] == exprOptions.DecimalSeparator)
                    {
                        startedCollecting = true;
                    }
                }
            }
            return finalNameBuilder.Reverse().ToString();
        }

        protected override NewWord CorrectWord
            (string word, int startIndex, int endIndex, string context, ExprOptions exprOptions)
        {
            //does not handle this:
            if (word.ContainsOnly(exprOptions.DecimalSeparator))
                return new NewWord { Value = word, HasNewWord = false };
            //does not handle this:
            if (word.Count(x => x == exprOptions.DecimalSeparator) > 1)
                return new NewWord { Value = word, HasNewWord = false };

            string finalName = word;
            //handle starting with dot
            if (finalName[0] == exprOptions.DecimalSeparator)
            {
                finalName = FirstIsPunctuation(finalName);
            }
            //handle ending with dot
            if (finalName.Last() == exprOptions.DecimalSeparator)
            {
                finalName = LastIsPunctuation(finalName);
            }
            if (finalName.Length > 0)
            {
                //handle leftside zero trailing
                //test for punctuation here also because it can be removed above
                if (finalName[0] == '0' && finalName.Contains(exprOptions.DecimalSeparator))
                {
                    finalName = LeftTrailing(finalName, exprOptions);
                }
                //handle right side trailing
                if (finalName.Last() == '0' && finalName.Contains(exprOptions.DecimalSeparator))
                {
                    finalName = RightTrailing(finalName, exprOptions);
                }
                //a rare case 0000. => 0000=> then no iteration
                if (finalName.All(x => x == '0'))
                    finalName = "0";
            }

            //if expression was .0 and we removed both
            if (finalName.Length == 0)
                return new NewWord { Value = "0", HasNewWord = true };
            else
                return new NewWord { Value = finalName, HasNewWord = true };
        }
    }
}
