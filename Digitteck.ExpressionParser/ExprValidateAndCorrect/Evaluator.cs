using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using System.Collections.Generic;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect
{
    public interface IEvaluator
    {
        EvaluationResult Evaluate(string context, ExprOptions exprOptions);
    }

    public class EvaluatorBuilder
    {
        private class Union
        {
            public IValidator Validator { get; set; }
            public ICorrector Corrector { get; set; }

            public bool IsValidator { get; set; }

            public Union(IValidator validator)
            {
                this.IsValidator = true;
                this.Validator = validator;
                this.Corrector = null;
            }
            public Union(ICorrector corrector)
            {
                this.IsValidator = false;
                this.Validator = null;
                this.Corrector = corrector;
            }
        }

        private class Evaluator : IEvaluator
        {
            private List<Union> CorrectorsOrValidators { get; set; }

            public Evaluator(List<Union> correctorsOrValidators)
            {
                this.CorrectorsOrValidators = correctorsOrValidators;
            }

            public EvaluationResult Evaluate(string context, ExprOptions exprOptions)
            {
                string correctedContext = context;

                foreach (Union element in this.CorrectorsOrValidators)
                {
                   if (element.IsValidator)
                    {
                        ValidationResult error 
                            = element.Validator.Validate(correctedContext, exprOptions);
                        if (error.ArgumentError != ArgumentError.OK)
                            return new EvaluationResult(context, correctedContext, error);
                    }
                    else {
                        correctedContext = element.Corrector.Correct(correctedContext, exprOptions);
                    }
                }
               
                return new EvaluationResult(context, correctedContext, ValidationResult.OK);
            }
        }

        private List<Union> CorrectorsOrValidators { get; set; }

        private EvaluatorBuilder()
        {
            this.CorrectorsOrValidators = new List<Union>();
        }
        public static EvaluatorBuilder Create()
        {
            return new EvaluatorBuilder();
        }
        public EvaluatorBuilder AddCorrector(ICorrector corrector)
        {
            this.CorrectorsOrValidators.Add(new Union(corrector));
            return this;
        }

        public EvaluatorBuilder AddValidator(IValidator validator)
        {
            this.CorrectorsOrValidators.Add(new Union(validator));
            return this;
        }

        public IEvaluator GetEvaluator() => new Evaluator(this.CorrectorsOrValidators);
    }
}
