﻿namespace VoteAnalyzer.Parser
{
    public interface IParser<in TIn, out TOut>
    {
        TOut Parse(TIn argument);
    }
}
