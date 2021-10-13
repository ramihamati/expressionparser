using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExpressionComponentTree.Base;
using Digitteck.ExpressionParser.FunctionWrapper;
using Digitteck.ExpressionParser.ExprParameter;

namespace Digitteck.ExpressionParser.ExpressionComponentTree
{
    public static class CreateComponentTree
    {
        public static ExprComponent Create(string expression, 
                                                  FunctionProvider functionProvider,
                                                  ParameterCollection parameterCollection,
                                                  ExprOptions exprOptions)
            => ExprComplexComponent.Create(expression, exprOptions, functionProvider, parameterCollection);
    }
}
