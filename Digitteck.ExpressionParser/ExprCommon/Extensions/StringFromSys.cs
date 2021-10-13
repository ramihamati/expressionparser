using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprCommon.Extensions
{
    public static class StringFromSys
    {
        public static List<int> AllIndicesOf(this string @this, char Search)
        {
            List<int> indices = new List<int>();
            int cIndex = -1;
            while ((cIndex = @this.IndexOf(Search, cIndex + 1)) >= 0)
            {
                indices.Add(cIndex);
            }
            return indices;
        }

        public static List<int> AllIndicesOf(this string @this, string Search)
        {
            List<int> indices = new List<int>();
            int cIndex = -1 * Search.Length;
            while ((cIndex = @this.IndexOf(Search, cIndex + Search.Length)) >= 0)
            {
                indices.Add(cIndex);
            }
            return indices;
        }
        /// <summary>
        /// Searches multiple values in a string, and return the smallest found index or -1
        /// </summary>
        public static ListSearchResult<string> FirstIndexOfAll(this string @this, IEnumerable<string> Searches)
        {
            List<ListSearchResult<string>> indices = new List<ListSearchResult<string>>();
            foreach (string search in Searches)
            {
                indices.Add(new ListSearchResult<string>
                {
                    Index = @this.IndexOf(search),
                    Found = search
                });
            }
            //no search found anywhere will result in a 0-length array
            indices.RemoveAll(x => x.Index == -1);

            if (indices.Count() == 0)
                return new ListSearchResult<string> { Index = -1, Found = null };

            int minIndex = indices.Select(x => x.Index).Min();
            return indices.Where(x => x.Index == minIndex).Single();
        }

        //determines if passed value is any of the items in the list
        public static bool Is<T>(this T element, IEnumerable<T> anyOfListItems)
        {
            return anyOfListItems.ToList().Any((item) => item.Equals(element));
        }
        //determines if passed value is any of the items in the list
        public static bool Is<T>(this T element, IEnumerable<T> anyOfListItems, IEqualityComparer<T> comparer)
        {
            return anyOfListItems.ToList().Any((item) => comparer.Equals(element, item));
        }

        public static string SliceIncludeLastIndex(this string expression, int fromIndex, int ToIndex)
        {
            StringBuilder word = new StringBuilder();
            for (int i = fromIndex + 1; i <= ToIndex - 1; i++)
            {
                word.Append(expression[i]);
            }
            return word.ToString();
        }

    }
}
