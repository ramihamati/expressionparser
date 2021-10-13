using System;
using System.Collections.Generic;
using System.Linq;

namespace Digitteck.ExpressionParser.ExprCommon.Extensions
{
    public static class ListExtensions
    {
        public static string JoinToString<T>(this IEnumerable<T> list, string sepparator = "")
        {
            return string.Join(sepparator, list);
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<int, T> action)
        {
            int index = 0;
            foreach (var item in enumerable)
            {
                action(index++, item);
            }
        }

        public static IList<T> ConcatMultiple<T>(this IEnumerable<T> @this, params IEnumerable<T>[] Lists)
        {
            IEnumerable<T> concatenated = new List<T>();
            concatenated = concatenated.Concat(@this);

            foreach (IEnumerable<T> list in Lists)
            {
                concatenated = concatenated.Concat(list);
            }
            return concatenated.ToList();
        }

        public static IEnumerable<int> AllIndicesOf<T>(this IEnumerable<T> @this, Predicate<T> Where)
        {
            int index = 0;
            List<int> indices = new List<int>();

            foreach (T item in @this)
            {
                if (Where(item))
                    indices.Add(index);
                index++;
            }
            return indices;
        }

        public static int CountDuplicates<T>(this IEnumerable<T> @this)
        {
            int duplicates = 0;
            //int occurences;
            ///store only duplicate items to increase performance
            List<T> duplicateItems = new List<T>();

            foreach (T item in @this)
            {
                if (!(duplicateItems.Contains(item)))
                {
                    int occurences = @this.Where<T>(x => x.Equals(item)).Count();
                    if (occurences > 1)
                    {
                        duplicateItems.Add(item);
                        duplicates++;
                    }
                }
            }
            return duplicates;
        }
        /// Checks if a list of items (or a string) contains any content of 
        /// the other list (or list of chars)
        public static bool ContainsAny<T>(this IEnumerable<T> @this, IEnumerable<T> other)
        {
            foreach (var part in @this)
                if (other.Contains(part))
                    return true;
            return false;
        }

        /// Checks if a list of items (or a string) contains only items from the other list
        public static bool ContainsOnly<T>(this IEnumerable<T> @this, IEnumerable<T> other)
        {
            foreach (var part in @this)
                if (!other.Contains(part))
                    return false;
            return true;
        }

        /// Checks if a list of items (or a string) contains only items from the other list
        public static bool ContainsOnly<T>(this IEnumerable<T> @this, T other)
        {
            foreach (var part in @this)
                if (!other.Equals(part))
                    return false;
            return true;
        }

        public static IEnumerable<T> TakeIf<T>(this IEnumerable<T> @this, Func<T, int, bool> If)
        {
            List<T> @return = new List<T>();
            List<T> iterableThis = @this.ToList();
            for (int i = 0; i < iterableThis.Count; i++)
            {
                if (If(iterableThis[i], i))
                    @return.Add(iterableThis[i]);
            }
            return @return;
        }
        public static IEnumerable<T> AddInstead<T>(this IEnumerable<T> @this, int fromIndex, int toIndex, T newItem)
        {
            List<T> newList = new List<T>();
            List<T> @thisList = @this.ToList();

            for (int j = 0; j <= fromIndex-1; j++)
                newList.Add(@thisList[j]);

            newList.Add(newItem);

            if (toIndex <= @thisList.Count() - 1)
                for (int j = toIndex + 1; j < @thisList.Count(); j++)
                    newList.Add(@thisList[j]);

            return newList;
        }
    }
}
