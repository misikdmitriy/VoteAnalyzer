using System.Linq;

namespace VoteAnalyzer.Common.Constants
{
    public class Constants
    {
        public static readonly char[] Numbers = Enumerable.Range(0, 10).Select(n => n.ToString()[0]).ToArray();
        public static readonly string[][] ExistingVotes = {
            new [] { "За" },
            new [] { "Проти" },
            new [] { "Відсутній" },
            new [] { "Не", "голосував" },
            new [] { "Утримався" }
        };
    }
}
