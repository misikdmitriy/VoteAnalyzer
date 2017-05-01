using System;
using System.IO;
using iTextSharp.text.pdf;
using VoteAnalyzer.Common.Models;
using VoteAnalyzer.DataAccessLayer.Entities;

namespace VoteAnalyzer.Parser
{
    class HeaderParser : IParser<ParseInfo, Session>
    {
        public Session Parse(ParseInfo argument)
        {
            if (File.Exists(argument.Directory))
            {
                var pdfReader = new PdfReader(Path.Combine(argument.Directory, argument.FileName));


            }

            throw new NotImplementedException();
        }
    }
}
