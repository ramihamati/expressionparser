using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.FunctionWrapper;
using Digitteck.ExpressionParser.ExprParameter;
using Digitteck.ExpressionParser.ExprParameter.Bases;
using System.Collections;

namespace Digitteck.ExpressionParser.ExpressionComponentTree.Base
{
    public class ExprParameterComponent : ExprComponent
    {
        public ParameterObject ParameterBase { get; private set; }

        public string MapToProperty { get; set; }

        public bool IsEnumerable { get; protected set; }

        protected ExprParameterComponent(ExprOptions exprOptions,
                                         ParameterObject parameterBase,
                                         string mapToProperty = "")
            : base(parameterBase.Name, exprOptions)
        {
            this.ParameterBase = parameterBase;
            this.MapToProperty = mapToProperty;
        }

        public static ExprComponent Create
            (ExprOptions exprOptions, ParameterObject parameterBase, string mapToProperty = "")
        {
            return new ExprParameterComponent(exprOptions, parameterBase, mapToProperty)
            {
                IsEnumerable = parameterBase.GetValue() is IEnumerable
            };
        }
    }
}
