using System;
using System.Collections.Generic;

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
        /// Votting session number
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Session link
        /// </summary>
        public Guid SessionId { get; set; }
    }
}
