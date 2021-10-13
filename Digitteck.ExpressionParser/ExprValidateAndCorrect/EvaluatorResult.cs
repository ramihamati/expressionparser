using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;

namespace Digitteck.ExpressionParser.ExprCommon
{
    public class EvaluationResult
    {
        public string Context { get; private set; }
        public string CorrectedContext { get; private set; }
        //Can be ValidationResult / ValidationResultChar / ValidationResultWord
        public ValidationResult ValidationResult { get; private set; }

        public EvaluationResult(string context, string correctedContext, ValidationResult validationResult)
        {
            this.Context = context;
            this.CorrectedContext = correctedContext;
            this.ValidationResult = validationResult;
        }
    }
}
