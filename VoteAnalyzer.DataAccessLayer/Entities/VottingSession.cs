using System;

namespace VoteAnalyzer.DataAccessLayer.Entities
{
    /// <summary>
    /// One votting session
    /// </summary>
    public class VottingSession : IIdentifiable<Guid>
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Votting subject
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Session link
        /// </summary>
        public Session Session { get; set; }
    }
}
