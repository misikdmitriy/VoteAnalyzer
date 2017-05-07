using System.Linq;

namespace VoteAnalyzer.Common.Constants
{
    public class Constants
    {
        public static readonly string[][] ExistingVotes = {
            new [] { "За" },
            new [] { "Проти" },
            new [] { "Відсутній" },
            new [] { "Не", "голосував" },
            new [] { "Утримався" }
        };
    }
}
