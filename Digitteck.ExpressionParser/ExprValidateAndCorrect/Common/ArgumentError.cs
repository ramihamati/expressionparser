using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Common
{
    /// <summary>
    /// ECC - Expression Char Check
    /// EC - Expression Check (As whole)
    /// EWC - Expression Word Check
    /// </summary>
    public enum ArgumentError
    {
        OK ,
        EC_OpenClosedBracketsCountMatch_NoPass,
        EC_ClosedBracketHasNoOpenBracketMatch,
        EC_NoStartWithMultDivMod,
        EC_EmptyExpression,
        EC_SingleDotExpression,
        ECC_IncorectUnderscore,
        ECC_CharIsAllowed_NoPass,
        ECC_IncorectValueAfterOpenBracket,
        ECC_IncorectValueBeforeOpenBracket,
        ECC_OpenBracketAtLastPosition,
        ECC_InvalidStartCharacter,
        ECC_InvalidEndCharacter,
        ECC_ClosedBracketAtFirstPosition,
        ECC_IncorectValueAfterClosedBracket,
        ECC_IncorectValueBeforeClosedBracket,
        ECC_IncorectValueAfterOperator,
        ECC_IncorectValueBeforeOperator,
        ECC_UnexpectedComma,
        ECC_InvalidCommaAtExpressionBegining,
        ECC_InvalidCommaAtExpressionEnd,
        ECC_InvalidValueBeforeComma,
        ECC_InvalidValueAfterComma,

        EWD_NameNotFunctionOrParameter,
        EWD_FunctionWithNoBracket,
        EWD_ParameterIsNotAFunction,
        EWD_NameIsNotFunction,
        //EWD_InvalidPunctuation,
        EWD_MultiplePunctuationSignsNotAllowed,
        EWD_ExprCannotStartWithPunctuation,
        EWD_ExprCannotEndWithPunctuation,
        EWO_InternalError_101, // No Open Bracket At Index,
        EWD_InternalError_102 // Expression with open bracket has 1 in length. Should not happen
    }
}
