using System;

namespace VoteAnalyzer.DataAccessLayer.Entities
{
    /// <summary>
    /// Deputy Information
    /// </summary>
    public class Deputy : IIdentifiable<Guid>
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Deputy Name
        /// </summary>
        public string Name { get; set; }
    }
}
