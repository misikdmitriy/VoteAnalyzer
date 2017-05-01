using System;
using VoteAnalyzer.DataAccessLayer.Entities;

namespace VoteAnalyzer.Common
{
    public static class Translator
    {
        public static VoteAction ToKnownVoteAction(this string action)
        {
            if (action.Equals("За", StringComparison.InvariantCultureIgnoreCase))
            {
                return VoteAction.Agree;
            }
            if (action.Equals("Проти", StringComparison.InvariantCultureIgnoreCase))
            {
                return VoteAction.Against;
            }
            if (action.Equals("Відсутній", StringComparison.InvariantCultureIgnoreCase))
            {
                return VoteAction.Absent;
            }
            if (action.Equals("Не голосував", StringComparison.InvariantCultureIgnoreCase))
            {
                return VoteAction.Hold;
            }

            throw new ArgumentException("Unknown vote action");
        }
    }
}
