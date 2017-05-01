using System.IO;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using VoteAnalyzer.Common.Models;
using Path = System.IO.Path;

namespace VoteAnalyzer.Common
{
    public class PdfConverter
    {
        public string ConvertToText(ParseInfo info)
        {
            var path = Path.Combine(info.Directory, info.FileName);

            if (File.Exists(path))
            {
                var pdfReader = new PdfReader(path);

                var text = PdfTextExtractor.GetTextFromPage(pdfReader, info.Page);

                return Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(text)));
            }

            return null;
        }
    }
}
