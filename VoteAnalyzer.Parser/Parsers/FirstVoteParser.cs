using System;
using System.Collections.Generic;
using System.Linq;
using VoteAnalyzer.Common.Constants;
using VoteAnalyzer.Parser.Models;

namespace VoteAnalyzer.Parser.Parsers
{
    public class FirstVoteParser : AbstractParser<string[], FirstVoteParserModel>
    {
        public override FirstVoteParserModel Parse(string[] argument)
        {
            int taken;

            FindVote(argument, out taken);

            return new FirstVoteParserModel
            {
                Vote = Constants.ExistingVotes[taken].Aggregate((current, next) => $"{current} {next}")
            };
        }

        private static bool FindVote(IEnumerable<string> argument, out int taken)
        {
            for (var i = 0; i < argument.Count(); i++)
            {
                foreach (var existingVote in Constants.ExistingVotes)
                {
                    var isEquals = true;
                    for (var j = 0; j < existingVote.Length; j++)
                    {
                        if (!existingVote[j].Equals(argument.ElementAt(i + j),
                            StringComparison.InvariantCultureIgnoreCase))
                        {
                            isEquals = false;
                            break;
                        }
                    }

                    if (isEquals)
                    {
                        taken = Array.IndexOf(Constants.ExistingVotes, existingVote);
                        return true;
                    }
                }
            }

            taken = -1;
            return false;
        }
    }
}
