
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;
using System.Collections.Generic;
using System.Linq;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Validators
{
    /// <summary>
    /// checks if a name exists in as function or parameter
    /// </summary>
    /// <returns>EWD_NoPunctuationsInNames/
    /// EWD_MultiplePunctuationSignsNotAllowed/
    /// EWD_NumberCannotStartWithPunctuation,
    /// EWD_NumberCannotEndWithPunctuation</returns>
    public class WordValidate_Punctuation : ExpressionWordValidate
    {
       
        public WordValidate_Punctuation()
        {
 
        }
        /*  string expression1 = "abc.+21";
            string expression2 = "0.2+21+abc";
            string expression3 = "0..2+21";
            string expression4 = "21 + a..b";
            string expression5 = "abc+2.1.4";
            string expression6 = "a.b.c+32";
            string expression7 = "a+.2";
            string expression8 = "21.+abc";
         */
        protected override ValidationResultWord ValidateName
            (string name, int startIndex, int endIndex, string expression, ExprOptions exprOptions)
        {
            if (name.Contains(exprOptions.DecimalSeparator))
            {
                //names and numbers can have a point. Below not valid. For names it will access properties
                //if (!exprOptions.HasNumericFormat(name))
                //    return new ValidationResultWord
                //        (ArgumentError.EWD_InvalidPunctuation, name, startIndex, endIndex);
                //now name/word has a numeric format with minimum 1 punctuation sign
                if (name.Contains(".."))
                    return new ValidationResultWord
                        (ArgumentError.EWD_MultiplePunctuationSignsNotAllowed, 
                          name, startIndex, endIndex,
                          string.Format("Double dot at index {0} ", startIndex));

                if (name.Count(x => x == exprOptions.DecimalSeparator) > 1 && exprOptions.ContainsNumbersAndPoint(name))
                    return new ValidationResultWord
                        (ArgumentError.EWD_MultiplePunctuationSignsNotAllowed, name, startIndex,endIndex,
                        string.Format("Multiple dots in a numeric value (\'{0}\') detected ", name));
                //if punctuation is at first index
                if (name[0] == exprOptions.DecimalSeparator)
                    return new ValidationResultWord
                        (ArgumentError.EWD_ExprCannotStartWithPunctuation, name, startIndex, endIndex,
                            "Expression cannot start with a dot");
                if (name.Last() == exprOptions.DecimalSeparator)
                    return new ValidationResultWord
                        (ArgumentError.EWD_ExprCannotEndWithPunctuation, name, startIndex, endIndex,
                            "Expression cannot end with a dot");
                return ValidationResultWord.OK;
            }
            return ValidationResultWord.OK;
        }
    }
}
