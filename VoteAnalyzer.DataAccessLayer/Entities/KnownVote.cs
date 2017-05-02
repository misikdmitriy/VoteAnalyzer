using System;

namespace VoteAnalyzer.DataAccessLayer.Entities
{
    public class KnownVote : IIdentifiable<Guid>
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Vote
        /// </summary>
        public string Vote { get; set; }
    }
}
