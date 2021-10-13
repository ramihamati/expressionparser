using Digitteck.ExpressionParser.ExprCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Digitteck.ExpressionParser.ExprCommon.Extensions;
using Digitteck.ExpressionParser.FunctionWrapper;
using Digitteck.ExpressionParser.ExprParameter;

namespace Digitteck.ExpressionParser.ExpressionComponentTree.Base
{
    /* reasoning:
     * The reason children is processed in a static function and passed in the constructor is 
     * that we need first to evaluate if any children has any invalid components. If so the expression 
     * will not be created but instead invalidcomponent is returned
     * This means that for any level of nested expression an Invalid component can be passed upwards
     */

    /// <summary>
    /// A complex components has Children components. Multiple parameters
    /// </summary>
    public class ExprComplexComponent : ExprComponent
    {
        public List<ExprComponent> Children { get; private set; }

        private ExprComplexComponent(string expression,
                                     ExprOptions exprOptions,
                                     List<ExprComponent> children)
            : base(expression, exprOptions)
        {
            this.Children = children;
        }

        public static ExprComponent Create(string context,
                                           ExprOptions exprOptions,
                                           FunctionProvider functionProvider,
                                           ParameterCollection parameterCollection)
        {

            List<ExprComponent> _children =
                ExprComplexComponentEvaluator
                    .Create(exprOptions, functionProvider, parameterCollection)
                    .Decompose(context);

            if (_children.Count == 1)
                if (_children.First() is ExprInvalidComponent)
                    return _children.First();

            ExprComplexComponent exprComplexComponent = new ExprComplexComponent(context, exprOptions, _children);

            return exprComplexComponent;
        }

    }
}
