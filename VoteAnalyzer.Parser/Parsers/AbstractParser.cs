using System;

namespace VoteAnalyzer.Parser.Parsers
{
    public abstract class AbstractParser<TIn, TOut> : IParser<TIn, TOut>
        where TOut : class
    {
        public abstract TOut Parse(TIn argument);

        public bool TryParse(TIn argument, out TOut result)
        {
            try
            {
                result = Parse(argument);
                return true;
            }
            catch (Exception)
            {
                result = null;
                return false;
            }
        }
    }
}
