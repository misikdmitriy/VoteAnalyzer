using System;
using System.IO;

namespace VoteAnalyzer.PdfIntegration.Models
{
    public class PdfFileInfo
    {
        public string Directory { get; set; }
        public string FileName { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((PdfFileInfo) obj);
        }

        protected bool Equals(PdfFileInfo other)
        {
            return Path.Combine(Directory, FileName)
                .Equals(Path.Combine(other.Directory, other.FileName),
                StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Directory?.GetHashCode() ?? 0) * 397) ^ (FileName?.GetHashCode() ?? 0);
            }
        }
    }
}
