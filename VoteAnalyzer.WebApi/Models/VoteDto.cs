using System;

namespace VoteAnalyzer.WebApi.Models
{
    public class VoteDto
    {
        public string DeputyName { get; set; }
        public string SessionName { get; set; }
        public DateTime SessionDate { get; set; }
        public string VottingSessionSubject { get; set; }
        public int? VottingSessionNumber { get; set; }
        public string Vote { get; set; }
    }
}