using Digitteck.ExpressionParser.FunctionWrapper.Base;
using System;

namespace Digitteck.ExpressionParser.FunctionWrapper.Attributes
{
    public class DeclaredAttribute : Attribute
    {
        public FunctionDefinitionType FunctionDefinitionType { get;set;}

        public DeclaredAttribute()
        {
            this.FunctionDefinitionType = FunctionDefinitionType.Base;
        }
    }
}
