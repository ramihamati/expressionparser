using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExprCommon.TypeHelper;
using Digitteck.ExpressionParser.ExprCommon.Warning;
using Digitteck.ExpressionParser.ExprParameter.Bases;
using Digitteck.ExpressionParser.FunctionWrapper.Attributes;
using Digitteck.ExpressionParser.FunctionWrapper.Base;
using Digitteck.ExpressionParser.FunctionWrapper.Convertible;
using Digitteck.ExpressionParser.FunctionWrapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.FunctionWrapper
{
    /*
           ConverterManager converterManager1 = ConverterManager.Create(typeof(Convertible));

            FunctionProvider fp = FunctionProvider.Create(converterManager1,typeof(Functions));
            ParameterDouble a = new ParameterDouble("a", 2);
            ParameterDouble bb = new ParameterDouble("bb", 2);
            FunctionBase sqrt = fp.GetByName("List").Value;
            var result1 = sqrt.Invoke(new ParameterObject[] { a, bb });
         */
    public class FunctionProvider
    {
        public FunctionBase[] FunctionsBase { get; private set; }

        public ConverterManager ConverterManager { get; private set; }

        public string[] FunctionNames { get; set; }

        public List<Warning> Warnings { get; private set; }

        public static FunctionProvider Create(ConverterManager converterManager, params Type[] ownerClasses)
        {
            if (ownerClasses == null)
                throw new Exception("To make a FunctionProvider argument cannot be null");

            if (ownerClasses.Length == 0)
                throw new Exception("To make a FunctionProvider provide at least one class type");

            foreach (Type type in ownerClasses)
            {
                if (!type.IsClass)
                    throw new Exception(string.Format("Provided type \'{0}\' is not a class", type.Name));
            }

            MethodInfo[] methods = 
                ownerClasses.SelectMany(item => item.GetMethods(BindingFlags.Static | BindingFlags.Public))
                                    .Where(x => x.GetCustomAttribute<DeclaredAttribute>() != null)
                                    .CheckFor<MethodInfo>(GuardForDuplicateBaseTypes)
                                    .CheckFor<MethodInfo>(GuardForOverloadWithoutBase)
                                    .CheckFor<MethodInfo>(GuardForWrongArguments)
                                    .Where(mInfo => mInfo.GetCustomAttribute<DeclaredAttribute>() != null)
                                    .ToArray();
         
            MethodInfo[] bases = methods.Where(x => x.GetCustomAttribute<DeclaredAttribute>().FunctionDefinitionType == FunctionDefinitionType.Base).ToArray();
            MethodInfo[] overloads = methods.Where(x => x.GetCustomAttribute<DeclaredAttribute>().FunctionDefinitionType == FunctionDefinitionType.Overload).ToArray();

            return new FunctionProvider(bases, overloads, converterManager);
        }

        private static void GuardForDuplicateBaseTypes(IEnumerable<MethodInfo> methodInfos)

        {
            List<string> fnBaseNames = new List<string>();

            foreach (var methodInfo in methodInfos)
            {
                DeclaredAttribute attribute = methodInfo.GetCustomAttribute<DeclaredAttribute>();

                if (attribute.FunctionDefinitionType == FunctionDefinitionType.Base
                    && fnBaseNames.Contains(methodInfo.Name))
                    throw new Exception(string.Format("Function \"{0}\" is defined more then once", methodInfo.Name));
                else if (attribute.FunctionDefinitionType == FunctionDefinitionType.Base)
                    fnBaseNames.Add(methodInfo.Name);
            }
        }

        private static void GuardForOverloadWithoutBase(IEnumerable<MethodInfo> methodInfos)
        {
            //get all bases
            List<string> fnBaseNames = new List<string>();
            foreach (var methodInfo in methodInfos)
            {
                DeclaredAttribute attribute = methodInfo.GetCustomAttribute<DeclaredAttribute>();
                if (attribute.FunctionDefinitionType == FunctionDefinitionType.Base)
                    fnBaseNames.Add(methodInfo.Name);
            }
            //check overloads
            foreach (var methodInfo in methodInfos)
            {
                DeclaredAttribute attribute = methodInfo.GetCustomAttribute<DeclaredAttribute>();
                if (attribute.FunctionDefinitionType == FunctionDefinitionType.Overload
                    && (!fnBaseNames.Contains(methodInfo.Name)))
                    throw new Exception(string.Format("Function \"{0}\" has no base", methodInfo.Name));
            }
        }

        private static void GuardForWrongArguments(IEnumerable<MethodInfo> methodInfos)
        {
            foreach (MethodInfo mi in methodInfos)
            {
                ParameterInfo[] parameters = mi.GetParameters();

                foreach (ParameterInfo parameter in parameters)
                {
                    bool hasParameterObjectBaseType = TypeHelper.MatchesTypeOrAncestorBaseType(parameter.ParameterType, typeof(ParameterObject));

                    if (!hasParameterObjectBaseType)
                    {
                        throw new Exception("Argument must be a derived type of " + nameof(ParameterObject));
                    }
                }
            }
        }

        public FunctionProvider(MethodInfo[] baseMethods, MethodInfo[] overloads ,ConverterManager converterManager)
        {
            this.ConverterManager = converterManager;

            List<FunctionBase> fnBases = new List<FunctionBase>();

            foreach (MethodInfo baseMInfo in baseMethods)
            {
                MethodInfo[] baseOverloads = overloads.Where(fn => fn.Name == baseMInfo.Name).ToArray();
                fnBases.Add(FunctionBase.Create(baseMInfo, baseOverloads, converterManager));

            }

            this.FunctionsBase = fnBases.ToArray();

            FunctionNames = FunctionsBase.Select(x => x.Name).ToArray();
        }

        public Maybe<FunctionBase> GetByName(string functionName)
        {
            FunctionBase functionBase = FunctionsBase.Where(fn => fn.Name == functionName).FirstOrDefault();

            if (functionBase != null)
                return Maybe<FunctionBase>.Create(functionBase, true);
            return Maybe<FunctionBase>.Create(null, false);
        }
    }
}
