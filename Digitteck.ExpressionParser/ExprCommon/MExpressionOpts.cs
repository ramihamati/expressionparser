using Digitteck.ExpressionParser.ExprCommon.Extensions;
using Digitteck.ExpressionParser.MExpression.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digitteck.ExpressionParser.ExprCommon
{
    public class ExprOptions
    {
        ///If values of allowed characters change,update also the test data
        /// Allowed expression brackets
        public char OpenBracket = '(';
        public char ClosedBracket = ')';
        /// Not allowed brackets 
        public List<char> AlternativeOpenBrackets = new List<char> { '{', '[' };
        public List<char> AlternativeClosedBrackets = new List<char> { '}', ']' };

        public List<char> AllowedNumbers = "0123456789".ToList();
        public char AllowedNamePrefix = '_';
        public List<char> AllowedLetters = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm".ToList();
        public List<char> AllowedOperators = "+-^*/%".ToList();
        public List<char> AllowedBrackets;
        public char DecimalSeparator = '.';
        public char GroupSeparator = ',';
        /// <summary>
        /// Combined
        /// </summary>
        //public List<char> AllAlphaNumeric;
        public List<char> AllAllowedCharacters;
        public List<char> AllNamingCharacters;
        public List<char> AllNumberCharacters;
        public List<char> AllCharactersInNamesAndNumbers;//similar to AllNaming + Punctuation
        /// <summary>
        /// For Singular operators tests
        /// </summary>
        public char OperatorMinusSign = '-';
        public char OperatorPlusSign = '+';
        public char OperatorMultiplySign = '*';
        public char OperatorDivideSign = '-';
        public char OperatorModSign = '%';
        public char OperatorExpSign = '^';

        public ExprOptions()
        {
            ///Initialize
            AllowedBrackets = new List<char> { OpenBracket, ClosedBracket };

            //AllAlphaNumeric = AllowedLetters.Concat(AllowedNumbers).ToList();

            AllAllowedCharacters =
                ConcatMultiple(AllowedNumbers,
                               AllowedLetters,
                               AllowedOperators,
                               AllowedBrackets).ToList();

            AllAllowedCharacters.Add(DecimalSeparator);
            AllAllowedCharacters.Add(AllowedNamePrefix);
            AllAllowedCharacters.Add(GroupSeparator);

            AllNamingCharacters = ConcatMultiple(AllowedNumbers, AllowedLetters).ToList();
            AllNamingCharacters.Add(AllowedNamePrefix);

            AllCharactersInNamesAndNumbers = new List<char>();
            AllCharactersInNamesAndNumbers =
                AllCharactersInNamesAndNumbers.Concat(AllNamingCharacters).ToList();
            AllCharactersInNamesAndNumbers.Add(DecimalSeparator);

            AllNumberCharacters = new List<char>();
            AllNumberCharacters = 
                        AllNumberCharacters
                        .Concat(AllowedNumbers)
                        .Concat(AllowedOperators)
                        .ToList();
            AllNumberCharacters.Add(DecimalSeparator);

        }
        
        private static IList<T> ConcatMultiple<T>(params IEnumerable<T>[] Lists)
        {
            IEnumerable<T> concatenated = new List<T>();

            foreach (IEnumerable<T> list in Lists)
            {
                concatenated = concatenated.Concat(list);
            }
            return concatenated.ToList();
        }

        public bool ContainsNumbersAndPoint(string value)
        {
            if (value.Length == 0)
                return false;
             return value.ContainsOnly(this.AllNumberCharacters);
        }

        public bool HasNumericFormat(string value)
        {
            if (value.Length == 0)
                return false;
            bool test1 = value.ContainsOnly(this.AllNumberCharacters);
            bool test2 = true;
            if (value.ContainsAny(this.AllowedOperators))
            {
                test2 = value.First().Is(this.AllowedOperators) &&
                        value.Count(x => x.Is(this.AllowedOperators)) == 1;
            }
            bool test3 = true;
            if (value.Contains(this.DecimalSeparator))
            {
                test3 = value.Count(x => x == this.DecimalSeparator) == 1;
            }

            return test1 && test2 && test3;
        }

        public bool HasNameFormat(string value)
        {
            if (value.Length == 0)
                return false;
            //must contain all naming characters
            //must have at least 1 letter
            //must start with 1 letter at least or underscore
            return value.ContainsOnly(this.AllNamingCharacters)
                    && value.Any(ch => this.AllowedLetters.Contains(ch))
                    && (value.First().Is(this.AllowedLetters) || value.First() == AllowedNamePrefix);
        }

        public bool HasOperatorFormat(string value)
        {
            if (value.Length != 1) return false;

            return value[0].Is(this.AllowedOperators);
        }

        public bool HasFunctionFormat(string value)
        {
            int index = value.IndexOf(this.OpenBracket);
            //if it doesn't have any open bracket, and it cannot be the first one
            if (index < 1) return false;
            //last must be closed bracket
            if (value.Last() != this.ClosedBracket) return false;
            //all elements from the first open bracket until the beggining must form a name
            StringBuilder name = new StringBuilder();
            for (int i = index - 1; i >= 0; i--)
            {
                name.Append(value[i]);
            }
            return HasNameFormat(name.Reverse().ToString());
        }
    }
}
