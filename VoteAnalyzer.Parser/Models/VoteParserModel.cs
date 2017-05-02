namespace VoteAnalyzer.Parser.Models
{
    public class VoteParserModel
    {
        /// <summary>
        /// Deputy info
        /// </summary>
        public DeputyParserModel DeputyParserModel { get; set; }
        /// <summary>
        /// Votting session info
        /// </summary>
        public VottingSessionParserModel VottingSessionParserModel { get; set; }
        /// <summary>
        /// Current vote
        /// </summary>
        public string Vote { get; set; }
    }
}
