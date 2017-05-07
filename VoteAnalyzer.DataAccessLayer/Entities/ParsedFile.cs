using System;

namespace VoteAnalyzer.DataAccessLayer.Entities
{
    public class ParsedFile : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
    }
}
