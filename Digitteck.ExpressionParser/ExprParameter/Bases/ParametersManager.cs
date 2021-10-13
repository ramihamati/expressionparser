using Digitteck.ExpressionParser.ExprCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprParameter.Bases
{
    public static class ParametersManager
    {
        public static string NextName() => namePrefix + (counter++).ToString();

        private static string namePrefix = "__parameter__";

        private static int counter = 0;

        //TODO : Refactor = move this in exprModel
        public static bool DoubleBeforeSingle = true;

        private static Maybe<Tout> TryConvert<Tout>(string expression, Func<string, Tout> converter)
        {
            Tout result = default(Tout);
            bool succeed = false;
            try
            {
                result = converter(expression);
                succeed = true;
            }
            catch { }
            return Maybe<Tout>.Create(result, succeed);
        }

        public static Maybe<ParameterObject> TryGetComponent(object data, ExprOptions exprOptions)
        {
            if (data is double[])
            {
                return Maybe<ParameterObject>.Create
                    (new ParameterDoubleArr(ParametersManager.NextName(), (double[])data), true);
            }
            else if (data is Int32[])
            {
                return Maybe<ParameterObject>.Create
                    (new ParameterInt32Arr(ParametersManager.NextName(), (Int32[])data), true);
            }
            else if (data is Int64[])
            {
                return Maybe<ParameterObject>.Create
                    (new ParameterInt64Arr(ParametersManager.NextName(), (Int64[])data), true);
            }
            else if (data is Single[])
            {
                return Maybe<ParameterObject>.Create
                   (new ParameterSingleArr(ParametersManager.NextName(), (Single[])data), true);
            }
            else if (data is Int32)
            {
                return Maybe<ParameterObject>.Create
                 (new ParameterInt32(ParametersManager.NextName(), (Int32)data), true);
            }
            else if (data is Int64)
            {
                return Maybe<ParameterObject>.Create
                 (new ParameterInt64(ParametersManager.NextName(), (Int64)data), true);
            }
            else if (data is Single)
            {
                if (!DoubleBeforeSingle)
                    return Maybe<ParameterObject>.Create(new ParameterSingle(ParametersManager.NextName(), (Single)data), true);
                else
                    return Maybe<ParameterObject>.Create(new ParameterDouble(ParametersManager.NextName(), (Double)data), true);
            }
            else if (data is Double)
            {
                return Maybe<ParameterObject>.Create
                 (new ParameterDouble(ParametersManager.NextName(), (Double)data), true);
            }
            else return TryGetNumberComponent(data.ToString(), exprOptions);
        }

        public static Maybe<ParameterObject> TryGetNumberComponent(string expression, ExprOptions exprOptions)
        {
            char NumberDecimalSeparator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            string local = expression.Replace(exprOptions.GroupSeparator, NumberDecimalSeparator).Replace(exprOptions.DecimalSeparator, NumberDecimalSeparator);

            Maybe<Int32> tryInt32 = TryConvert<Int32>(local, Convert.ToInt32);


            if (tryInt32.HasValue)
                return Maybe<ParameterObject>.Create(new ParameterInt32(ParametersManager.NextName(), tryInt32.Value), true);

            Maybe<Int64> tryInt64 = TryConvert<Int64>(local, Convert.ToInt64);
            if (tryInt64.HasValue)
                return Maybe<ParameterObject>.Create(new ParameterInt64(ParametersManager.NextName(), tryInt64.Value), true);

            if (!DoubleBeforeSingle)
            {
                Maybe<Single> trySingle = TryConvert<Single>(local, Convert.ToSingle);
                if (trySingle.HasValue)
                    return Maybe<ParameterObject>.Create(new ParameterSingle(ParametersManager.NextName(), trySingle.Value), true);
            }

            Maybe<Double> tryDouble = TryConvert<Double>(local, Convert.ToDouble);
            if (tryDouble.HasValue)
                return Maybe<ParameterObject>.Create(new ParameterDouble(ParametersManager.NextName(), tryDouble.Value), true);

            return Maybe<ParameterObject>.Create(null, false);
        }

        //determines if the underlying type of the parameterinfo is an object implementing IParameterArray<T>
        public static bool IsParameterArray(ParameterInfo parameterInfo)
        {
            //an object thatm implements IParameterArray
            Type parameterType = parameterInfo.ParameterType;
            //TODO : maybe refactor contains.name
            Type IParameterArrayInterface = parameterType.GetInterfaces().Where(x => x.Name.Contains("IParameterArray")).FirstOrDefault();

            if (IParameterArrayInterface == null)
                return false;

            if (IParameterArrayInterface.GenericTypeArguments.Count() != 1)
                return false;

            return true;
        }

        public static bool IsParameterArray(ParameterObject parameterObject)
        {
            //an object thatm implements IParameterArray
            Type parameterType = parameterObject.GetType();
            //TODO : maybe refactor contains.name
            Type IParameterArrayInterface = parameterType.GetInterfaces().Where(x => x.Name.Contains("IParameterArray")).FirstOrDefault();

            if (IParameterArrayInterface == null)
                return false;

            if (IParameterArrayInterface.GenericTypeArguments.Count() != 1)
                return false;

            return true;
        }
        //returns the GenericType passed to interface IParameterArray
        public static Type GetArrayUnitType(ParameterInfo parameterInfo)
        {
         
            //an object thatm implements IParameterArray
            Type parameterType = parameterInfo.ParameterType;
            Type IParameterArrayInterface = parameterType.GetInterfaces().Where(x => x.Name.Contains("IParameterArray")).FirstOrDefault();
            //TODO : maybe refactor here
            return IParameterArrayInterface.GenericTypeArguments[0];
        }


    }
}
