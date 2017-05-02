using System.Collections.Generic;

using VoteAnalyzer.Common;
using VoteAnalyzer.PdfIntegration.Models;
using VoteAnalyzer.PdfIntegration.PdfServices;

namespace VoteAnalyzer.PdfIntegration.PdfContainers
{
    public class PdfContainer : IPdfContainer
    {
        private readonly IDictionary<PdfFileInfo, string[]> _contentDictionary 
            = new Dictionary<PdfFileInfo, string[]>();

        private readonly IDictionary<PdfFileInfo, string[][]> _splittedTextDictionary 
            = new Dictionary<PdfFileInfo, string[][]>();

        private readonly IPdfService _pdfService;

        public PdfContainer(IPdfService pdfService)
        {
            _pdfService = pdfService;
        }

        public string[] GetSeparatedWords(PdfFileInfo fileInfo, int page)
        {
            if (!_splittedTextDictionary.ContainsKey(fileInfo))
            {
                var numberOfPages = _pdfService.GetNumberOfPages(fileInfo);
                var result = new List<string[]>();

                for (var pageNum = 0; pageNum < numberOfPages; pageNum++)
                {
                    var content = GetContent(fileInfo, pageNum + 1);
                    result.Add(WordSeparator.Split(content));
                }

                _splittedTextDictionary[fileInfo] = result.ToArray();
            }

            return _splittedTextDictionary[fileInfo][page];
        }

        public string GetContent(PdfFileInfo fileInfo, int page)
        {
            if (!_contentDictionary.ContainsKey(fileInfo))
            {
                var numberOfPages = _pdfService.GetNumberOfPages(fileInfo);
                var result = new List<string>();

                for (var pageNum = 0; pageNum < numberOfPages; pageNum++)
                {
                    var content = _pdfService.ConvertToText(fileInfo, pageNum + 1);
                    result.Add(content);
                }

                _contentDictionary[fileInfo] = result.ToArray();
            }

            return _contentDictionary[fileInfo][page];
        }
    }
}
