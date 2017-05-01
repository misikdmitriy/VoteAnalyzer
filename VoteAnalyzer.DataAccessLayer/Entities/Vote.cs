﻿using System;

namespace VoteAnalyzer.DataAccessLayer.Entities
{
    /// <summary>
    /// One deputy vote
    /// </summary>
    public class Vote : IIdentifiable<Guid>
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Owner of vote
        /// </summary>
        public Guid DeputyId { get; set; }
        /// <summary>
        /// Current vote (hold/against/etc.)
        /// </summary>
        public VoteAction VoteAction { get; set; }
        /// <summary>
        /// Votting session
        /// </summary>
        public Guid VottingSessionId { get; set; }
    }
}
