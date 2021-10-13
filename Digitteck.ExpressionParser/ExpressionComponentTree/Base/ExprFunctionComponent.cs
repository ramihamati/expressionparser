using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.FunctionWrapper;
using Digitteck.ExpressionParser.ExprParameter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Digitteck.ExpressionParser.ExpressionComponentTree.Base
{
    public class ExprFunctionComponent : ExprComponent
    {
        public FunctionBase FunctionBase { get; set; }

        public List<ExprComplexComponent> Arguments { get; private set; }

        //the reason a list of arguments is determined outside the constructor is that in the 
        //Create function we first check for any invalid components. If any is determined then 
        //that component is returned. Ensuring thus nested-level-expression invalid upwards passing.

        private ExprFunctionComponent
            (string expressionString, FunctionBase functionBase, ExprOptions exprOptions, List<ExprComplexComponent> arguments)
            : base(expressionString, exprOptions)
        {

            this.FunctionBase = functionBase;
            this.Arguments = arguments;
        }

        public static ExprComponent Create(string expression, ExprOptions exprOptions,
                                           FunctionBase functionBase,
                                           Func<string, ExprComponent> CreateComplexComponent)
        {
            int openBracketCount = 0;
            List<string> components = new List<string>();

            StringBuilder sb = new StringBuilder();

            foreach (char ch in expression)
            {
                if (ch == exprOptions.OpenBracket) { openBracketCount++; sb.Append(ch); continue; }
                if (ch == exprOptions.ClosedBracket) { openBracketCount--; sb.Append(ch); continue; }


                if (ch == exprOptions.GroupSeparator && openBracketCount == 0)
                {
                    components.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(ch);
                }
            }
            if (sb.Length > 0)
                components.Add(sb.ToString());

            List<ExprComplexComponent> arguments = new List<ExprComplexComponent>();

            foreach (string part in components)
            {
                ExprComponent component = CreateComplexComponent(part);

                if (component is ExprInvalidComponent)
                    return component;

                arguments.Add((ExprComplexComponent)component);
            }

            return new ExprFunctionComponent(expression, functionBase, exprOptions, arguments);

        }
    }
}
