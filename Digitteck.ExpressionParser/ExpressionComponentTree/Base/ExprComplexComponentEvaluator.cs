using Digitteck.ExpressionParser.ExprCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Digitteck.ExpressionParser.ExprCommon.Extensions;
using Digitteck.ExpressionParser.FunctionWrapper;
using Digitteck.ExpressionParser.ExprParameter;
using Digitteck.ExpressionParser.ExprParameter.Bases;

namespace Digitteck.ExpressionParser.ExpressionComponentTree.Base
{
    public class ExprComplexComponentEvaluator
    {

        ExprOptions ExprOptions { get; set; }
        FunctionProvider FunctionProvider { get; set; }
        ParameterCollection ParameterCollection { get; set; }

        private ExprComplexComponentEvaluator() { }

        public static ExprComplexComponentEvaluator Create(ExprOptions exprOptions,
                                                           FunctionProvider functionProvider,
                                                           ParameterCollection parameterCollection)
        {
            return new ExprComplexComponentEvaluator
            {
                ExprOptions = exprOptions,
                FunctionProvider = functionProvider,
                ParameterCollection = parameterCollection
            };
        }

        private List<ExprComponent> returnInvalid(ExprInvalidComponent component)
        {
            return new List<ExprComponent> { component };
        }

        public List<ExprComponent> Decompose(string context)
        {
            context = RemoveEnclosingBrackets(context);
            char openBracket = this.ExprOptions.OpenBracket;
            char closingBracket = this.ExprOptions.ClosedBracket;

            List<ExprComponent> children = new List<ExprComponent>();

            StringBuilder wordBuilder = new StringBuilder();

            for (int i = 0; i < context.Length; i++)
            {
                if (context[i] == ' ') continue;
                //if there is no word(meaning no name of a function) but a bracket follows. Decomposing it
                if (context[i] == this.ExprOptions.OpenBracket && wordBuilder.Length == 0)
                {
                    //if there is an expresion inside brackets but no function calls
                    int _openBracket = i;

                    int _closeBracket = FindClosingMatchBracket(context, i);

                    ExprComponent result = AddSimpleExpression(context, _openBracket, _closeBracket);

                    if (result is ExprInvalidComponent)
                        return returnInvalid(result as ExprInvalidComponent);
                    children.Add(result);

                    i = _closeBracket;
                }
                //if there is a word(meaning a name of a function) and a bracket follows. Decomposing it
                else if (context[i] == this.ExprOptions.OpenBracket && wordBuilder.Length > 0)
                {
                    //if there is an expression inside brackets and there is a function
                    int _openBracket = i;
                    int _closedBracket = FindClosingMatchBracket(context, _openBracket);

                    ExprComponent result
                        = AddExprFunctionComponent(context, _openBracket, _closedBracket, wordBuilder.ToString());

                    if (result is ExprInvalidComponent) return returnInvalid(result as ExprInvalidComponent);
                    children.Add(result);

                    wordBuilder.Clear();
                    i = _closedBracket;
                }

                else if (this.ExprOptions.AllowedOperators.Contains(context[i]))
                {
                    //if we encountered operators, but no brackets, means we have a word only
                    if (wordBuilder.Length > 0)
                    {
                        //add existing word if there is one
                        ExprComponent componentLeft = AddExprNumeric_ParameterComponent(wordBuilder.ToString());
                        if (componentLeft is ExprInvalidComponent)
                            return returnInvalid(componentLeft as ExprInvalidComponent);
                        else
                            children.Add(componentLeft);
                        wordBuilder.Clear();
                    }
                    ExprComponent component = AddExprOperatorComponent(context[i]);
                    if (component is ExprInvalidComponent) return returnInvalid(component as ExprInvalidComponent);

                    children.Add(component);
                }
                //if this is the last index. We will add it to wordBuilder
                else if (i == context.Length - 1)
                {
                    //last item
                    wordBuilder.Append(context[i]);
                    ExprComponent component = AddExprNumeric_ParameterComponent(wordBuilder.ToString());
                    if (component is ExprInvalidComponent) return returnInvalid(component as ExprInvalidComponent);

                    children.Add(component);
                    wordBuilder.Clear();
                }
                else
                {
                    wordBuilder.Append(context[i]);
                }
            }

            return children;

        }
        /// Adds an expression that may have children
        private ExprComponent AddSimpleExpression(string context, int openBracketStart, int closeBracketIndex)
        {
            //this error should never occur
            if (closeBracketIndex == -2)
            {
                return new ExprInvalidComponent(context, this.ExprOptions, string.Format("Char at index={0} is not open bracket", openBracketStart));
            }
            //this error should never occur
            if (closeBracketIndex == -1)
            {
                return new ExprInvalidComponent(context, this.ExprOptions, string.Format("Could not find a closing bracket for {0}", context));
            }
            string extract = context.SliceIncludeLastIndex(openBracketStart, closeBracketIndex);

            ExprComponent pComponent =
                ExprComplexComponent.Create(extract, this.ExprOptions, this.FunctionProvider, this.ParameterCollection);

            return pComponent;
        }

        /// Adds an expression that is argument to a function
        private ExprComponent AddExprFunctionComponent
            (string context, int openBracketIndex, int closedBracketIndex, string fnName)
        {
            //this error should never occur
            if (closedBracketIndex == -2)
            {
                return new ExprInvalidComponent(fnName, this.ExprOptions,
                            string.Format("Char at index={0} is not open bracket", openBracketIndex));
            }
            //this error should never occur
            if (closedBracketIndex == -1)
            {
                return new ExprInvalidComponent(fnName, this.ExprOptions,
                            string.Format("Could not find a closing bracket for {0}", fnName));
            }
            //Try get function
            Maybe<FunctionBase> functionBase = this.FunctionProvider.GetByName(fnName);
            if (!functionBase.HasValue)
                return new ExprInvalidComponent(fnName, this.ExprOptions,
                            string.Format("{0} is not a function name", fnName));
            //brackets excluded
            string fnContent = context.SliceIncludeLastIndex(openBracketIndex, closedBracketIndex);
            //get entire function body like fn(a)
            string functionEntire = fnName + this.ExprOptions.OpenBracket + fnContent + this.ExprOptions.ClosedBracket;

            ExprComponent component = null;

            if (this.ExprOptions.HasFunctionFormat(functionEntire))
            {
                component =
                    ExprFunctionComponent.Create(fnContent, this.ExprOptions, functionBase.Value, (_value) =>
                    {
                        return ExprComplexComponent.Create(_value, this.ExprOptions, this.FunctionProvider, this.ParameterCollection);
                    });
            }
            else
            {
                component =
                    new ExprInvalidComponent(functionEntire, this.ExprOptions,
                        string.Format("Expression \'{0}\'does not have a function format", functionEntire));
            }

            return component;
        }

        private ExprComponent AddExprOperatorComponent(char character)
        {
            return ExprOperatorComponent.Create(character, this.ExprOptions);
        }
        /// A component is a parameter or a value
        private ExprComponent AddNumericComponent(string expression)
        {
            return ExprNumericComponent.Create(expression, this.ExprOptions);
        }

        private ExprComponent AddObjectComponent(string expression)
        {
            string paramName = "";
            string mapToProperty = "";

            List<string> parts = expression.Split(this.ExprOptions.DecimalSeparator).ToList();
            paramName = parts.First();
            parts.RemoveAt(0);

            mapToProperty = string.Join("" + this.ExprOptions.DecimalSeparator, parts);

            Maybe<ParameterObject> maybeParam = this.ParameterCollection.TryGetParameter(paramName);

            if (!maybeParam.HasValue)
            {
                return new ExprInvalidComponent(expression, this.ExprOptions,
                        string.Format("Paramter {0} was not found in collection", expression));
            }

            if (!(maybeParam.Value is ParameterInstance))
            {
                return new ExprInvalidComponent(expression, this.ExprOptions,
                        string.Format("Paramter {0} is not an object", expression));
            }
            return ExprParameterComponent.Create(this.ExprOptions, (ParameterInstance)maybeParam.Value, mapToProperty);
        }

        private ExprComponent AddParametricComponent(string expression)
        {
            Maybe<ParameterObject> maybeParam1 = ParameterCollection.TryGetParameter(expression);

            if (!maybeParam1.HasValue)
            {
                return new ExprInvalidComponent(expression, this.ExprOptions,
                        string.Format("Paramter {0} was not found in collection", expression));
            }

            return ExprParameterComponent.Create(this.ExprOptions, maybeParam1.Value);
        }

        private ExprComponent AddExprNumeric_ParameterComponent(string expression)
        {
            if (this.ExprOptions.HasNumericFormat(expression))
            {
                return AddNumericComponent(expression);
            }
            else if (expression.Contains(this.ExprOptions.DecimalSeparator))
            {
                return AddObjectComponent(expression);
            }
            else
            {
                return AddParametricComponent(expression);
            }
        }
        /// Removes paranthesis from the beginning the end if they hold the whole expression
        private string RemoveEnclosingBrackets(string expression)
        {
            if (expression[0] == this.ExprOptions.OpenBracket
                && expression[expression.Length - 1] == this.ExprOptions.ClosedBracket)
            {
                int openCount = 1;
                for (int i = 1; i <= expression.Length - 2; i++)
                {
                    if (expression[i] == this.ExprOptions.OpenBracket)
                        openCount++;
                    if (expression[i] == this.ExprOptions.ClosedBracket)
                        openCount--;
                    //if bracket closed before reaching the end return
                    if (openCount == 0)
                        return expression;
                }
                ///If between second and priorToLast all brackets closed then the expression is:
                ///(a+b+(a)+0)
                if (openCount == 1)
                {
                    return expression = expression.Substring(1, expression.Length - 2);
                }
            }
            return expression;
        }

        private int FindClosingMatchBracket(string context, int openBracketPosition)
        {
            if (context[openBracketPosition] != this.ExprOptions.OpenBracket)
                return -2;

            int openBracketCount = 1;
            for (int i = openBracketPosition + 1; i < context.Length; i++)
            {
                if (context[i] == this.ExprOptions.OpenBracket)
                    openBracketCount++;
                if (context[i] == this.ExprOptions.ClosedBracket)
                    openBracketCount--;
                if (openBracketCount == 0)
                    return i;
            }
            return -1;
        }
    }
}
