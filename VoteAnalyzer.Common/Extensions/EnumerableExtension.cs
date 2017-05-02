using System;
using System.Collections.Generic;
using System.Linq;

namespace VoteAnalyzer.Common.Extensions
{
    public static class EnumerableExtension
    {
        public static int IndexOfByPredicate<TModel>(this IEnumerable<TModel> enumerable, 
            Func<TModel, bool> predicate)
        {
            for (var i = 0; i < enumerable.Count(); i++)
            {
                if (predicate(enumerable.ElementAt(i)))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
