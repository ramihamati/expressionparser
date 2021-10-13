using System;
using System.Collections.Generic;
using System.Linq;

namespace Digitteck.ExpressionParser.ExprCommon
{
    /// <summary>
    /// A feature class used to limit context interactions based on certain conditions
    /// Determiner will provide a value of true to proceed and false otherwise
    ///  Proceed<int> proceed = new Proceed<int>();
    ///        proceed.If((value) => value % 2 == 0);
    ///        proceed.If(value => value > 5);
    ///  Proceed<char> proceedChar = new Proceed<char>();
    ///        proceedChar.WithAny(new List<char> { 'a', 'b', 'c' });
    /// </summary>
    public class Proceed<T>
    {
        //test
        private List<T> ProceedValues;

        private List<Predicate<T>> Determiners;

        private IEqualityComparer<T> Comparer;

        private Func<T, T, bool> TEquals { get; set; }

        public Proceed()
        {
            ProceedValues = new List<T>();
            Determiners = new List<Predicate<T>>();

            if (Comparer == null)
            {
                TEquals = (x, y) => x.Equals(y);
            }
            else
            {
                TEquals = Comparer.Equals;
            }
        }

        public Proceed(IEqualityComparer<T> comparer) : base()
        {
            this.Comparer = comparer;
        }

        public void With(T ProceedValue)
        {
            this.ProceedValues.Add(ProceedValue);
        }
        public void If(Predicate<T> Determiner)
        {
            this.Determiners.Add(Determiner);
        }

        public void WithAny(List<T> proceedValues)
        {
           this.ProceedValues = this.ProceedValues.Concat(proceedValues).ToList();
        }
        public bool ShouldProceed(T Context)
        {
            bool test1 = (ProceedValues.Count == 0) ? 
                            true : ProceedValues.Any(det => TEquals(Context, det) == true);

            bool test2 = (Determiners.Count == 0) ? true : Determiners.All(det => det(Context));

            return test1 && test2;
        }
        public bool ShouldNotProceed(T Context)
        {
            return !(ShouldProceed(Context));
        }
    }
}
