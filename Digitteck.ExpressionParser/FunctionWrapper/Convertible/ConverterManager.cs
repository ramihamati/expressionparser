using Digitteck.ExpressionParser.ExprCommon.TypeHelper;
using Digitteck.ExpressionParser.ExprCommon.Warning;
using Digitteck.ExpressionParser.ExprParameter.Bases;
using Digitteck.ExpressionParser.FunctionWrapper.Attributes;
using Digitteck.ExpressionParser.FunctionWrapper.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.FunctionWrapper.Convertible
{
    public class ConverterManager
    {
        public List<Warning> Warnings { get; private set; }

        public List<Convertible> Converters { get; private set; }

        private ConverterManager(MethodInfo[] converterMethods)
        {
            this.Warnings = new List<Warning>();
            this.Converters = new List<Convertible>();

            if (converterMethods.Count() == 0)
                this.Warnings.Add(new Warning("No converted method detected"));

            foreach (MethodInfo minfo in converterMethods)
            {
                this.Converters.Add(Convertible.Create(minfo.GetParameters().First().ParameterType, minfo.ReturnType, minfo));
            }
        }

        public static ConverterManager Create(params Type[] ownerClasses)
        {
            if (ownerClasses == null)
                throw new Exception("To make a ConverterManager argument cannot be null");

            if (ownerClasses.Length == 0)
                throw new Exception("To make a ConverterManager provide at least one class type");

            foreach (Type type in ownerClasses)
            {
                if (!type.IsClass)
                    throw new Exception(string.Format("Provided type \'{0}\' is not a class", type.Name));
            }

            MethodInfo[] converterMethods =
                ownerClasses.SelectMany(item => item.GetMethods(BindingFlags.Static | BindingFlags.Public))
                            .Where(mInfo => mInfo.GetCustomAttribute<ConvertibleAttribute>() != null)
                            .ToArray();

            return new ConverterManager(converterMethods);
        }

        public List<ParameterObject> ConvertTo(List<ParameterObject> From, List<ArgumentInfo> To)
        {
            if (From.Count() != To.Count() && !(To.Any(x=>x.IsParams)))
                throw new Exception("Internal Error : Provided no of arguments do not match required no of converted data");

            List<ParameterObject> matchingData = new List<ParameterObject>();

            for (int i = 0; i < From.Count(); i++)
            {
                if (From[i].GetType().Equals(To[i].ArgumentType))
                    matchingData.Add(From[i]);

                else if (To[i].IsParams)
                {
                    Type referentType = To[i].ArrayUnitType;

                    if (i != To.Count() - 1)
                        throw new Exception("Params object must be the last item in the function definition");

                    for (int j = i; j < From.Count(); j++)
                    {
                        Type fromType = From[j].GetType();
                        Type toType = referentType;
                        if (fromType.Equals(toType))
                        {
                            matchingData.Add(From[j]);
                        }
                        else
                        {
                            Convertible converter = this.Converters.Where(conv => conv.From.Equals(fromType) && conv.To.Equals(toType)).FirstOrDefault();
                            if (converter == null)
                                throw new Exception(string.Format("Internal Erro : No Converter found from type {0} to type {1}", fromType.ToString(), toType.ToString()));
                            matchingData.Add(converter.Convert(From[j]));
                        }
                    }
                    break;
                }

                else
                {
                    Type fromType = From[i].GetType();
                    Type toType = To[i].ArgumentType;

                    Convertible converter =
                        this.Converters.Where(conv => conv.From.Equals(fromType) && conv.To.Equals(toType))
                        .FirstOrDefault();
                    if (converter == null)
                        throw new Exception(string.Format("Internal Erro : No Converter found from type {0} to type {1}", fromType.ToString(), toType.ToString()));

                    matchingData.Add(converter.Convert(From[i]));
                }
            }

            return matchingData;
        }

        public int CalculateTransformations(List<Type> parametersProvided, List<ArgumentInfo> fnArguments)
        {
            foreach (Type type in parametersProvided)
            {
                if (!(TypeHelper.MatchesTypeOrAncestorBaseType(type, typeof(ParameterObject))))
                    throw new Exception(string.Format("Internal Error : Type provided is {0} but must be {1} or inheriting from it", type.ToString(), nameof(ParameterObject)));
            }

            int mutations = 0;

            if (fnArguments.Count() > parametersProvided.Count())
                return -1; //No transformation can be made

            for (int i = 0; i < fnArguments.Count(); i++)
            {
                Type providedType = parametersProvided[i];
                ArgumentInfo requiredArgumentInfo = fnArguments[i];

                if (requiredArgumentInfo.IsParams)
                {
                    //check if rest of parameters ar of the desired type
                    Type desiredArrayType = requiredArgumentInfo.ArrayUnitType;
                    for (int j = i; j < parametersProvided.Count(); j++)
                    {
                        if (!parametersProvided[j].Equals(desiredArrayType))
                        {
                            bool HasConverter = this.Converters.Where(x => x.From.Equals(parametersProvided[j]) && x.To.Equals(desiredArrayType)).Count() != 0;

                            if (HasConverter)
                                mutations++;
                            else
                                return -1;
                        }
                    }
                }
                else
                {
                    if (!providedType.Equals(requiredArgumentInfo.ArgumentType))
                    {
                        bool HasConverter = this.Converters.Where(x => x.From.Equals(providedType) && x.To.Equals(requiredArgumentInfo.ArgumentType)).Count() != 0;

                        if (HasConverter)
                            mutations++;
                        else
                            return -1;
                    }
                }
            }
            return mutations;
        }
    }
}
