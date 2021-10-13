using Digitteck.ExpressionParser.ExpressionComponentTree.Base;

namespace Digitteck.ExpressionParser.ExprModel
{
    public interface IExprModel
    {
        EvaluateComponentResult<ExprComponent> GetResult();

        string Context { get; }

        string CorrectedContext { get; }
    }
}
