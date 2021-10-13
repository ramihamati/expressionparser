using Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprCommon;
using System;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Correctors
{
    /// <summary>
    /// replace [ { with ( and }, ] with )
    /// </summary>
    public class ExprCharCorrector_ReplaceAlternativeBrackets : ExpressionCharCorrect
    {

        public ExprCharCorrector_ReplaceAlternativeBrackets() : base()
        {
        }

        Func<char, ExprOptions, NewChar> correctChar = (character, options) => 
        {
            if (options.AlternativeOpenBrackets.Contains(character))
                return new NewChar { HasNewChar = true, Value = options.OpenBracket };

            if (options.AlternativeClosedBrackets.Contains(character))
                return new NewChar { HasNewChar = true, Value = options.ClosedBracket };

            return new NewChar { HasNewChar = false };
        };

        protected override NewChar CorrectFirst(int index, char character, bool hasNext, char nextCharacter, ExprOptions exprOptions)
        {
            return correctChar(character, exprOptions);
        }

        protected override NewChar Correctlast(int index, char prevCharacter, char character, ExprOptions exprOptions)
        {
            return correctChar(character, exprOptions);
        }

        protected override NewChar CorrectMiddle(int index, char prevCharacter, char character, char nextCharacter, ExprOptions exprOptions)
        {
            return correctChar(character, exprOptions);
        }
    }
}
