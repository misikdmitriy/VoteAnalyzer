namespace VoteAnalyzer.Parser.Parsers
{
    /// <summary>
    /// Parsing strategy
    /// </summary>
    /// <typeparam name="TIn">Input argument</typeparam>
    /// <typeparam name="TOut">Parsing result</typeparam>
    public interface IParser<in TIn, out TOut>
    {
        TOut Parse(TIn argument);
    }
}
