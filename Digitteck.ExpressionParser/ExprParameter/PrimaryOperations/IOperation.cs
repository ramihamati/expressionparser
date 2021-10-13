using Digitteck.ExpressionParser.ExprModel;
using Digitteck.ExpressionParser.ExpressionComponentTree.Base;

namespace Digitteck.ExpressionParser.ExprParameter.PrimaryOperations
{
    public interface IOperation
    {
        EvaluateComponentResult<ExprParameterComponent> 
            Result(ExprParameterComponent First, ExprParameterComponent Second);
    }
}
