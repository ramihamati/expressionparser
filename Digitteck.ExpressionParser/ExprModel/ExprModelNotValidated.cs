using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExpressionComponentTree.Base;
using Digitteck.ExpressionParser.FunctionWrapper;
using Digitteck.ExpressionParser.FunctionWrapper.Runtime;
using Digitteck.ExpressionParser.ExprParameter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Digitteck.ExpressionParser.ExprModel
{
    public class ExprModelNotValidated : IExprModel
    {
        public EvaluateComponentResult<ExprComponent> GetResult() => null;

        public string Context { get; }

        public string CorrectedContext { get; }

        public ValidationResult ValidationResult { get; private set; }

        public ExprModelNotValidated(
            EvaluationResult evaluationResult)
        {
            this.ValidationResult = evaluationResult.ValidationResult;
            this.Context = evaluationResult.Context;
            this.CorrectedContext = evaluationResult.CorrectedContext;
        }
    }
}
