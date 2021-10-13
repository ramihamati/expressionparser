using Digitteck.ExpressionParser.ExprCommon.Validators;
using Digitteck.ExpressionParser.ExprValidateAndCorrect;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Correctors;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Validators;

namespace Digitteck.ExpressionParser.ExprCommon
{
    public static class EvaluatorFactory
    {
        public static IEvaluator ExpressionEvaluator(string[] fnNames,
            string[] parameterNames)
        {
            //fixed add position. To not change (corrector or validator)
            IEvaluator ExpressionEvaluator = EvaluatorBuilder.Create()
                       .AddCorrector(new ExprCorrector_CleanEmptySpaces())
                       .AddCorrector(new ExprCharCorrector_ReplaceAlternativeBrackets())
                       .AddCorrector(new ExprCorrector_MultiplyBeforeOpenBracket(fnNames))
                       .AddCorrector(new WordCorrect_MultSignAfterClosedBracket())
                       .AddCorrector(new ExprCorrector_MultiplyAfterClosingBracket())
                       .AddCorrector(new ExprCorrector_RemoveEnclosingParanthesis())
                       .AddCorrector(new ExprCorrector_RemoveRedundantBrackets())
                       .AddCorrector(new ExprCorrector_RedundantMinusPlusSign())
                       .AddCorrector(new ExprCorrector_RedundantPlusMinusSign())
                       .AddCorrector(new ExprCorrector_RedundantPlusSign())
                       .AddCorrector(new ExprCorrector_PositiveSignCorrection())
                       .AddCorrector(new WordCorrect_TrailingZeros())
                       
                       .AddValidator(new ExprValidate_EmptyExpression())
                       .AddValidator(new ExprValidate_SpecialCases())
                       .AddValidator(new CharValidation_NotAllowed())
                       .AddValidator(new ExprValidate_NoStartWithMultOrDiv())
                       .AddValidator(new ExprValidate_OpenClosedBracketsCountMatch())
                       .AddValidator(new ExprValidate_BracketPositioning())
                       .AddValidator(new CharValidation_CanStartWith())
                       .AddValidator(new CharValidation_CanEndWith())
                       .AddValidator(new CharValidation_OpeningBracket())
                       .AddValidator(new CharValidation_ClosingBracked())
                       .AddValidator(new CharValidation_Operators())
                       .AddValidator(new WordValidate_Punctuation())
                       .AddValidator(new WordValidate_UnderscoreAlone())
                       .AddValidator(new WordValidate_FnOpenBracket(fnNames, parameterNames))
                       .AddValidator(new WordValidate_FnOrParameterName(fnNames, parameterNames))
                       .AddValidator(new ExprValidate_ArgumentSplit(fnNames))
                       .AddValidator(new CharValidation_ArgumentSplit())
                       //.AddCorrector(new ExprCorrector_EncodeFunction(fnNames))
                       .GetEvaluator();

            return ExpressionEvaluator;
        }

    }
}
