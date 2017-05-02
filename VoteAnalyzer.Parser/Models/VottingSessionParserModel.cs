namespace VoteAnalyzer.Parser.Models
{
    public class VottingSessionParserModel
    {
        /// <summary>
        /// Votting subject
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Votting session number
        /// </summary>
        public int? Number { get; set; }
        /// <summary>
        /// Session info
        /// </summary>
        public SessionParserModel SessionParserModel { get; set; }
    }
}
