using System;
using System.Collections.Generic;

namespace VoteAnalyzer.DataAccessLayer.Entities
{
    /// <summary>
    /// Parliamentary Session info
    /// </summary>
    public class Session : IIdentifiable<Guid>
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Occurs date
        /// </summary>
        public DateTime DateTime { get; set; }
    }
}
