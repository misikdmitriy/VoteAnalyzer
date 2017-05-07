using System;

namespace VoteAnalyzer.DataAccessLayer.Entities
{
    /// <summary>
    /// Parse file info
    /// </summary>
    public class ParsedFile : IIdentifiable<Guid>
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Path to file
        /// </summary>
        public string Path { get; set; }
    }
}
