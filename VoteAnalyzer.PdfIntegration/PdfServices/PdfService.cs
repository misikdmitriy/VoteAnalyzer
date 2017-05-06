using System.IO;
using System.Text;

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

using VoteAnalyzer.PdfIntegration.Models;

using Path = System.IO.Path;

namespace VoteAnalyzer.PdfIntegration.PdfServices
{
    public class PdfService : IPdfService
    {
        public string GetContent(PdfFileInfo info, int page)
        {
            var path = BuildPath(info);

            if (Exists(info))
            {
                using (var pdfReader = new PdfReader(path))
                {
                    var text = PdfTextExtractor.GetTextFromPage(pdfReader, page);

                    return Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8,
                        Encoding.Default.GetBytes(text)));
                }
            }

            throw new FileNotFoundException($"File {path} not found");
        }

        public int GetNumberOfPages(PdfFileInfo info)
        {
            var path = BuildPath(info);

            if (Exists(info))
            {
                using (var pdfReader = new PdfReader(path))
                {
                    return pdfReader.NumberOfPages;
                }
            }

            throw new FileNotFoundException($"File {path} not found");
        }

        public bool Exists(PdfFileInfo info)
        {
            var path = BuildPath(info);

            return File.Exists(path);
        }

        private string BuildPath(PdfFileInfo info)
        {
            return Path.Combine(info.Directory, info.FileName);
        }
    }
}
