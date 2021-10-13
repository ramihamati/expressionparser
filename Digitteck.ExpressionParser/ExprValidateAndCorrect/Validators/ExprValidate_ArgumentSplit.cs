using Digitteck.ExpressionParser.ExprCommon.Extensions;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using Digitteck.ExpressionParser.ExprCommon.StringHelpers;

namespace Digitteck.ExpressionParser.ExprCommon.Validators
{
    /// <summary>
    /// Returns true if there is an equal no of open and closed brackets
    /// </summary>
    public class ExprValidate_ArgumentSplit : ExpressionValidate
    {
        string[] FunctionNames;

        public ExprValidate_ArgumentSplit(string[] fnNames)
        {
            this.FunctionNames = fnNames;
        }
        //Logic: WHenever we enter a function - a ',' can be only if we are on the same level of the expression
        //This means a+sqrt(2,(2)) is Valid, a+sqrt(2,(2,2)) is not valid.
        //Logic : Whenever we enter a function a , can be only if there all the brackets inside are closed

        private ValidationResult ValidateFunction(string expression, int functionBracketStart, ExprOptions exprOptions)
        {
            int closeBracket = IndexHelpers.FindIndexOfMatchingClosedBracket(expression, functionBracketStart, exprOptions);

            if (closeBracket == -2)
                return new ValidationResult(ArgumentError.EWO_InternalError_101,
                        string.Format("Internal Error 101"));

            if (closeBracket == -1)
                return new ValidationResult(ArgumentError.EWD_InternalError_102,
                        string.Format("Internal Error 102"));
            //where sqrt(,) =>
            int startOfExpression = functionBracketStart + 1;
            int endOfExpression = closeBracket - 1;

            if (startOfExpression == exprOptions.GroupSeparator)
                return new ValidationResult(ArgumentError.ECC_UnexpectedComma,
                                string.Format("Invalid position of , at index {0}", startOfExpression));

            if (endOfExpression == exprOptions.GroupSeparator)
                return new ValidationResult(ArgumentError.ECC_UnexpectedComma,
                                string.Format("Invalid position of , at index {0}", endOfExpression));

            return ValidationResult.OK;
        }
        /// <summary>
        /// Note : For the example "sqrt(2,(2,3)) we cannot test if the , is set withing range of open bracket / closed bracket of the function
        /// Because the inner expression is not allowed to have argument separators
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="exprOptions"></param>
        /// <returns></returns>
        public override ValidationResult Validate(string expression, ExprOptions exprOptions)
        {
            string[] needles = this.FunctionNames.Select(x => x + '(').ToArray();

            List<int> functionStarts = new List<int>();
            ///For each function range will hold min and max values being the index of ) and ( that mark the function expression
            ///For "Max(1,2)" we have ( at position 3 and ) at position 7. Range will store 3 and 7 
            List<RangeInt> ranges = new List<RangeInt>();

            foreach (string needle in needles)
            {
                int[] positions = expression.AllIndicesOf(needle).ToArray();
                foreach (int position in positions)
                {
                    //-1 because we search for 'sqrt(' not 'sqrt'
                    functionStarts.Add(position + needle.Length - 1);
                    var a = expression.Length;

                    int closeBrackedIndex = IndexHelpers.FindIndexOfMatchingClosedBracket(expression, position + needle.Length - 1, exprOptions);

                    ranges.Add(RangeInt.Create(position + needle.Length - 1, closeBrackedIndex));
                }
            }

            foreach (int startPosition in functionStarts)
            {
                ValidationResult validationResult = ValidateFunction(expression, startPosition, exprOptions);

                if (!(validationResult.ArgumentError == ArgumentError.OK))
                    return validationResult;
            }

            List<int> AllIndicesOfArgumentSeparator = expression.AllIndicesOf(exprOptions.GroupSeparator);

            //sqrt(2,(1,2)) for the expression in the left some positions are not permitted, allowed only 5, 6
            List<int> AllPermitedPositions =
                ranges
                .Select(x => IndexHelpers.FindIndicesAtExpressionLevel(expression, x.MinValue, x.MaxValue, exprOptions))
                .SelectMany(x => x)
                .ToList();

            foreach (int separtorIndex in AllIndicesOfArgumentSeparator)
            {
                if (!AllPermitedPositions.Any(value => value == separtorIndex))
                {
                    return new ValidationResult(ArgumentError.ECC_UnexpectedComma, string.Format("Unexpected \',\' at index = {0}", separtorIndex));
                }
            }
            return ValidationResult.OK;
        }
    }
}
