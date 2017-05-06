using System;
using System.Collections.Generic;
using System.Linq;

namespace VoteAnalyzer.Common.Extensions
{
    public static class EnumerableExtension
    {
        public static int IndexOfByPredicate<TModel>(this IEnumerable<TModel> enumerable, 
            Func<TModel, int, bool> predicate)
        {
            var array = enumerable.ToArray();

            for (var i = 0; i < array.Length; i++)
            {
                if (predicate(array[i], i))
                {
                    return i;
                }
            }

            return -1;
        }

        public static int LastIndexOfByPredicate<TModel>(this IEnumerable<TModel> enumerable,
            Func<TModel, int, bool> predicate)
        {
            var index = -1;
            var array = enumerable.ToArray();

            for (var i = 0; i < array.Length; i++)
            {
                if (predicate(array[i], i))
                {
                    index = i;
                }
            }

            return index;
        }

        public static int IndexOfSubsequence(this IEnumerable<string> strings, IEnumerable<string> substrings)
        {
            return strings.IndexOfByPredicate(StringPredicate(strings, substrings));
        }

        public static int LastIndexOfSubsequence(this IEnumerable<string> strings, IEnumerable<string> substrings)
        {
            return strings.LastIndexOfByPredicate(StringPredicate(strings, substrings));
        }

        private static Func<string, int, bool> StringPredicate(IEnumerable<string> strings, 
            IEnumerable<string> substrings)
        {
            return (s, i) =>
            {
                for (var j = 0; j < substrings.Count(); j++)
                {
                    if (!substrings.ElementAt(j).Equals(strings.ElementAt(i + j),
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        return false;
                    }
                }

                return true;
            };
        }
    }
}
