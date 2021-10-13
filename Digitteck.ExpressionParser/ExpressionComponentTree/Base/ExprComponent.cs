

using Digitteck.ExpressionParser.ExprCommon;

namespace Digitteck.ExpressionParser.ExpressionComponentTree.Base
{
    public abstract class ExprComponent
    {
        public string Context { get; protected set; }
        //public string EncodedContext { get; private set; }

        public ExprOptions ExprOptions { get; set; }

        public ExprComponent(string ExpressionString, ExprOptions exprOptions)
        {
            this.ExprOptions = exprOptions;

            this.Context = ExpressionString;
        }
    }
}
