using Digitteck.ExpressionParser.ExprCommon.Extensions;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Digitteck.ExpressionParser.ExprCommon.Validators
{
    /// <summary>
    /// Returns true if there is an equal no of open and closed brackets
    /// </summary>
    public class ExprValidate_SpecialCases : ExpressionValidate
    {
        public ExprValidate_SpecialCases()
        {
        }

        public override ValidationResult Validate(string expression, ExprOptions exprOptions)
        {
            if (expression.ContainsOnly(exprOptions.DecimalSeparator))
                return new ValidationResult(ArgumentError.EC_SingleDotExpression,
                    "Expression cannot be a single dot");

            return ValidationResult.OK;
        }
    }
}
