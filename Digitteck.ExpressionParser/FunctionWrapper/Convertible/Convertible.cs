using Digitteck.ExpressionParser.ExprCommon.TypeHelper;
using Digitteck.ExpressionParser.ExprParameter.Bases;
using Digitteck.ExpressionParser.FunctionWrapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.FunctionWrapper.Convertible
{
    public class Convertible : IConvertible
    {
        public Type From { get; private set; }

        public Type To { get; private set; }

        private MethodInfo MethodInfo { get; set; }

        public ParameterObject Convert(ParameterObject Object)
        {
            return MethodInfo.Invoke(null, new object[1] { Object }) as ParameterObject;
        }

        private Convertible(Type from, Type to, MethodInfo methodInfo)
        {
            this.MethodInfo = methodInfo;
            this.From = from;
            this.To = to;
        }

        public static Convertible Create(Type from, Type to, MethodInfo methodInfo)
        {
            if (methodInfo.GetCustomAttribute<ConvertibleAttribute>() == null)
                throw new Exception(string.Format("Method {0} is not marked with {0} attribute", methodInfo.Name, nameof(ConvertibleAttribute)));

            if (methodInfo.GetParameters().Count() != 1)
                throw new Exception(string.Format("Method {0} must hast 1 argument", methodInfo.Name));

            if (!TypeHelper.MatchesTypeOrAncestorBaseType(methodInfo.ReturnType, typeof(ParameterObject)))
                throw new Exception(string.Format("Return type of method {0} is {1} but a converter requires the type {2}", methodInfo.Name, methodInfo.ReturnType.Name, nameof(ParameterObject)));

            Type argumentType = methodInfo.GetParameters().First().ParameterType;

            if (!TypeHelper.MatchesTypeOrAncestorBaseType(argumentType, typeof(ParameterObject)))
                throw new Exception(string.Format("Return type of method {0} is {1} but a converter requires the type {2}", methodInfo.Name, argumentType.Name, nameof(ParameterObject)));

            return new Convertible(from, to, methodInfo);
        }

        public bool CanTransform(Type From, Type To)
        {
            return this.From.Equals(From) && this.To.Equals(To);
        }
    }
}
