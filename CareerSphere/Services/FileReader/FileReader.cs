using System.Text;
using UglyToad.PdfPig;

namespace CareerSphere.Services.FileReader
{
    public class FileReader : IFileReader
    {
        public async Task<string> ReadFileAsync()
        {
            string filePath = "C:\\Users\\ROCK\\source\\repos\\CareerSphere\\CareerSphere\\Prompts\\ResumeAnalysis.txt";
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file at path {filePath} was not found.");
            }

            using (var reader = new StreamReader(filePath))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public string ExtractTextFromPdf(Stream stream)
        {
            var text = new StringBuilder();

            using (var document = PdfDocument.Open(stream))
            {
                foreach (var page in document.GetPages())
                {
                    text.AppendLine(page.Text);
                }
            }

            return text.ToString();
        }

    }
}
