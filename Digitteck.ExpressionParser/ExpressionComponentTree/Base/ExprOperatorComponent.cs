using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.FunctionWrapper;

namespace Digitteck.ExpressionParser.ExpressionComponentTree.Base
{
    public class ExprOperatorComponent : ExprComponent
    {
        public OperatorType OperatorType { get; set; }

        private ExprOperatorComponent(string expressionString,
                                  ExprOptions exprOptions,
                                  OperatorType operatorType)
            : base(expressionString, exprOptions)
        {
            this.OperatorType = operatorType;
        }

        public static ExprComponent Create(string expression, ExprOptions exprOptions, OperatorType operatorType)
        {
            if (exprOptions.HasOperatorFormat(expression))
            {
                return new ExprOperatorComponent(expression, exprOptions, operatorType);
            }
            else
            {
                return new ExprInvalidComponent(expression, exprOptions, string.Format("Invalid operator = {0}", expression));
            }
        }

        public static ExprComponent Create(char charOp, ExprOptions exprOptions)
        {
            if (exprOptions.HasOperatorFormat(charOp.ToString()))
            {
                switch (charOp)
                {
                    //then add operators
                    case '+':
                        return new ExprOperatorComponent("+", exprOptions, OperatorType.Plus);
                    case '-':
                        return new ExprOperatorComponent("-", exprOptions, OperatorType.Minus);
                    case '*':
                        return new ExprOperatorComponent("*", exprOptions, OperatorType.Multiply);
                    case '/':
                        return new ExprOperatorComponent("/", exprOptions, OperatorType.Divide);
                    case '%':
                        return new ExprOperatorComponent("%", exprOptions, OperatorType.Mod);
                    case '^':
                        return new  ExprOperatorComponent("^", exprOptions, OperatorType.Power);
                    default:
                        return new ExprInvalidComponent(charOp.ToString(), exprOptions, string.Format("{0} is not a valid operator", charOp));
                }
            }
            else
            {
                return new ExprInvalidComponent(charOp.ToString(), exprOptions, string.Format("{0} is not a valid operator", charOp));
            }
        }
    }
}
