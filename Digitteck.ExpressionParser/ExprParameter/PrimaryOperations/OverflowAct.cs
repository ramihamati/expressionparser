using System;

namespace Digitteck.ExpressionParser.ExprParameter.PrimaryOperations
{
    public static class OverflowAct
    {
        //Additions of primary numerical values
        public static Parameter AddWithOverflow(Int32 first, Int32 second)
        {
            int resultInt = first + second;
            if (resultInt >= second && resultInt >= first)
                return ParameterInt32.Create(resultInt);
            long resultLong = (long)first + second;

            return ParameterInt64.Create(resultLong);
        }

        public static Parameter AddWithOverflow(Int32 first, Int64 second)
        {
            long resultLong = (long)first + second;

            return ParameterInt64.Create(resultLong);
        }

        public static Parameter AddWithOverflow(Int64 first, Int32 second)
            => AddWithOverflow(second, first);

        public static Parameter AddWithOverflow(Int32 first, Single second)
        {
            Single result = first + second;
            if (result >= second && result >= first)
                return ParameterSingle.Create(result);
            Double resultDouble = (double)first + (double)second;
            return ParameterDouble.Create(resultDouble);
        }

        public static Parameter AddWithOverflow(Single first, Int32 second)
            => AddWithOverflow(second, first);

        public static Parameter AddWithOverflow(Int32 first, Double second)
        {
            Double resultDouble = (double)first + (double)second;
            return ParameterDouble.Create(resultDouble);
        }

        public static Parameter AddWithOverflow(Double first, Int32 second)
            => AddWithOverflow(second, first);


        public static Parameter AddWithOverflow(Int64 first, Int64 second)
            => ParameterInt64.Create(first + second);

        public static Parameter AddWithOverflow(Int64 first, Single second)
            => ParameterDouble.Create((double)first + second);

        public static Parameter AddWithOverflow(Single first, Int64 second)
            => AddWithOverflow(second, first);

        public static Parameter AddWithOverflow(Single first, Single second)
        {
            float resultSingle = first + second;
            if (resultSingle >= first && resultSingle >= second)
                return ParameterSingle.Create(resultSingle);
            return ParameterDouble.Create((double)first + second);
        }

        public static Parameter AddWithOverflow(Single first, Double second)
            => ParameterDouble.Create(second + first);

        public static Parameter AddWithOverflow(Double first, Single second)
            => AddWithOverflow(second, first);

        public static Parameter AddWithOverflow(Double first, Double second)
            => ParameterDouble.Create(first + second);

        //Subtractions of primary numerical values
        public static Parameter SubWithOverflow(Int32 first, Int32 second)
        {
            Int32 resultInt = first - second;
            if (resultInt <= first)
                return ParameterInt32.Create(first - second);
            Int64 resultLong = (long)first - second;
            return ParameterInt64.Create(resultLong);
        }

        public static Parameter SubWithOverflow(Int64 first, Int32 second)
        {
            return ParameterInt64.Create(first - second);
        }

        public static Parameter SubWithOverflow(Int32 first, Int64 second)
            => ParameterInt64.Create((long)first - second);

        public static Parameter SubWithOverflow(Int32 first, Single second)
        {
            float resultSingle = (float)first - second;
            if (resultSingle <= first)
                return ParameterSingle.Create(resultSingle);
            return ParameterDouble.Create((double)first - second);
        }

        public static Parameter SubWithOverflow(Single first, Int32 second)
        {
            float resultSingle = first - second;
            if (resultSingle <= first)
                return ParameterSingle.Create(resultSingle);
            return ParameterDouble.Create((double)first - second);
        }

        public static Parameter SubWithOverflow(Double first, Int32 second)
            => ParameterDouble.Create(first - second);

        public static Parameter SubWithOverflow(Int32 first, Double second)
                    => ParameterDouble.Create((double)first - second);


        public static Parameter SubWithOverflow(Int64 first, Int64 second)
            => ParameterInt64.Create(first - second);

        public static Parameter SubWithOverflow(Int64 first, Single second)
            => ParameterDouble.Create((double)first - second);

        public static Parameter SubWithOverflow(Single first, Int64 second)
            => SubWithOverflow(second, first);

        public static Parameter SubWithOverflow(Single first, Single second)
        {
            return ParameterDouble.Create((double)first - second);
        }

        public static Parameter SubWithOverflow(Single first, Double second)
            => ParameterDouble.Create(second - first);

        public static Parameter SubWithOverflow(Double first, Single second)
            => SubWithOverflow(second, first);

        public static Parameter SubWithOverflow(Double first, Double second)
            => ParameterDouble.Create(first - second);

        //multiplication of primary values
        public static Parameter MultWithOverflow(Int32 first, Int32 second)
        {
            int resultInt = first * second;
            if (resultInt >= first && resultInt >= second)
                return ParameterInt32.Create(resultInt);
            return ParameterInt64.Create((long)first * second);
        }

        public static Parameter MultWithOverflow(Int64 first, Int32 second)
            => ParameterInt64.Create(first * second);

        public static Parameter MultWithOverflow(Int32 first, Int64 second)
            => ParameterInt64.Create((long)first * second);

        public static Parameter MultWithOverflow(Int32 first, Single second)
        {
            float resultSingle = first * second;
            if (resultSingle >= first && resultSingle >= second)
                return ParameterSingle.Create(resultSingle);
            return ParameterDouble.Create((double)first * second);
        }

        public static Parameter MultWithOverflow(Single first, Int32 second)
            => MultWithOverflow(second, first);

        public static Parameter MultWithOverflow(Int32 first, Double second)
            => ParameterDouble.Create((double)first * second);

        public static Parameter MultWithOverflow(Double first, Int32 second)
            => MultWithOverflow(second, first);


        public static Parameter MultWithOverflow(Int64 first, Int64 second)
           => ParameterInt64.Create(first * second);

        public static Parameter MultWithOverflow(Int64 first, Single second)
            => ParameterDouble.Create((double)first * second);

        public static Parameter MultWithOverflow(Single first, Int64 second)
            => MultWithOverflow(second, first);

        public static Parameter MultWithOverflow(Single first, Single second)
        {
            return ParameterDouble.Create((double)first * second);
        }

        public static Parameter MultWithOverflow(Single first, Double second)
            => ParameterDouble.Create(second * first);

        public static Parameter MultWithOverflow(Double first, Single second)
            => MultWithOverflow(second, first);

        public static Parameter MultWithOverflow(Double first, Double second)
            => ParameterDouble.Create(first * second);

        //division of primary values
        public static Parameter DivWithOverflow(Int32 first, Int32 second)
        {
            return ParameterDouble.Create((double)first / second);
        }

        public static Parameter DivWithOverflow(Int64 first, Int32 second)
            => ParameterDouble.Create((double)first / second);

        public static Parameter DivWithOverflow(Int32 first, Int64 second)
            => ParameterDouble.Create((double)first / second);

        public static Parameter DivWithOverflow(Int32 first, Single second)
        {
            return ParameterDouble.Create((double)first / second);
        }

        public static Parameter DivWithOverflow(Single first, Int32 second)
            => DivWithOverflow(second, first);

        public static Parameter DivWithOverflow(Int32 first, Double second)
            => ParameterDouble.Create((double)first / second);

        public static Parameter DivWithOverflow(Double first, Int32 second)
            => DivWithOverflow(second, first);


        public static Parameter DivWithOverflow(Int64 first, Int64 second)
           => ParameterDouble.Create((double)first / second);

        public static Parameter DivWithOverflow(Int64 first, Single second)
            => ParameterDouble.Create((double)first / second);

        public static Parameter DivWithOverflow(Single first, Int64 second)
            => DivWithOverflow(second, first);

        public static Parameter DivWithOverflow(Single first, Single second)
        {
            return ParameterSingle.Create(first / second);
        }

        public static Parameter DivWithOverflow(Single first, Double second)
            => ParameterDouble.Create(second / first);

        public static Parameter DivWithOverflow(Double first, Single second)
            => DivWithOverflow(second, first);

        public static Parameter DivWithOverflow(Double first, Double second)
            => ParameterDouble.Create(first / second);

        //power of primary values
        public static Parameter PowWithOverflow(Int32 first, Int32 second)
        {
            return ParameterDouble.Create(Math.Pow(first, second));
        }

        public static Parameter PowWithOverflow(Int64 first, Int32 second)
             => ParameterDouble.Create(Math.Pow(first, second));

        public static Parameter PowWithOverflow(Int32 first, Int64 second)
            => ParameterDouble.Create(Math.Pow(first, second));

        public static Parameter PowWithOverflow(Int32 first, Single second)
        => ParameterDouble.Create(Math.Pow(first, second));

        public static Parameter PowWithOverflow(Single first, Int32 second)
         => ParameterDouble.Create(Math.Pow(first, second));

        public static Parameter PowWithOverflow(Int32 first, Double second)
          => ParameterDouble.Create(Math.Pow(first, second));

        public static Parameter PowWithOverflow(Double first, Int32 second)
          => ParameterDouble.Create(Math.Pow(first, second));


        public static Parameter PowWithOverflow(Int64 first, Int64 second)
         => ParameterDouble.Create(Math.Pow(first, second));

        public static Parameter PowWithOverflow(Int64 first, Single second)
         => ParameterDouble.Create(Math.Pow(first, second));

        public static Parameter PowWithOverflow(Single first, Int64 second)
        => ParameterDouble.Create(Math.Pow(first, second));

        public static Parameter PowWithOverflow(Single first, Single second)
         => ParameterDouble.Create(Math.Pow(first, second));

        public static Parameter PowWithOverflow(Single first, Double second)
         => ParameterDouble.Create(Math.Pow(first, second));

        public static Parameter PowWithOverflow(Double first, Single second)
            => ParameterDouble.Create(Math.Pow(first, second));

        public static Parameter PowWithOverflow(Double first, Double second)
             => ParameterDouble.Create(Math.Pow(first, second));

        //mod of primary values
        public static Parameter ModWithOverflow(Int32 first, Int32 second)
        {
            return ParameterInt32.Create(first % second);
        }

        public static Parameter ModWithOverflow(Int64 first, Int32 second)
        {
            return ParameterInt64.Create(first % second);
        }

        public static Parameter ModWithOverflow(Int32 first, Int64 second)
            => ParameterInt64.Create(first % second);

        public static Parameter ModWithOverflow(Int32 first, Single second)
            => ParameterSingle.Create(first % second);

        public static Parameter ModWithOverflow(Single first, Int32 second)
            => ParameterSingle.Create(first % second);

        public static Parameter ModWithOverflow(Int32 first, Double second)
            => ParameterDouble.Create(first % second);

        public static Parameter ModWithOverflow(Double first, Int32 second)     
            => ParameterDouble.Create(first % second);


        public static Parameter ModWithOverflow(Int64 first, Int64 second)
            => ParameterInt64.Create(first % second);

        public static Parameter ModWithOverflow(Int64 first, Single second)
            => ParameterSingle.Create(first % second);

        public static Parameter ModWithOverflow(Single first, Int64 second)
            => ParameterSingle.Create(first % second);

        public static Parameter ModWithOverflow(Single first, Single second)
            => ParameterSingle.Create(first % second);

        public static Parameter ModWithOverflow(Single first, Double second)
            => ParameterDouble.Create(first % second);

        public static Parameter ModWithOverflow(Double first, Single second)
            => ParameterDouble.Create(first % second);

        public static Parameter ModWithOverflow(Double first, Double second)
             => ParameterDouble.Create(first % second);
    }
}
