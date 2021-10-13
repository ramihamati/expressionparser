using Digitteck.ExpressionParser.ExpressionComponentTree.Base;

namespace Digitteck.ExpressionParser.ExprModel
{
    public class ExprModelInvalidComponent : IExprModel
    {
        public EvaluateComponentResult<ExprComponent> GetResult() => null;

        public string Context { get; }

        public string CorrectedContext { get; }

        public ExprInvalidComponent ExprInvalidComponent { get; private set; }

        public ExprModelInvalidComponent(
            ExprInvalidComponent exprInvalidComponent,
            string context,
            string correctedContext)
        {
            this.ExprInvalidComponent = exprInvalidComponent;
            this.Context = context;
            this.CorrectedContext = correctedContext;
        }
    }
}
