using System.Collections.Concurrent;
using System.Text;
using UglyToad.PdfPig;

namespace CareerSphere.Services.FileReader
{
    public class FileReader : IFileReader
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FileReader> _logger;

        // Cache prompts in memory — no need to read from disk every request
        private static readonly ConcurrentDictionary<string, string> _promptCache = new();

        public FileReader(IWebHostEnvironment env, ILogger<FileReader> logger)
        {
            _env = env;
            _logger = logger;
        }

       
        private async Task<string> ReadPromptAsync(string fileName)
        {
            // Return from cache if already loaded
            if (_promptCache.TryGetValue(fileName, out var cached))
                return cached;

            // Build path relative to project root — works on any machine
            var filePath = Path.Combine(_env.ContentRootPath, "Prompts", fileName);

            if (!File.Exists(filePath))
            {
                _logger.LogError("Prompt file not found: {FilePath}", filePath);
                throw new FileNotFoundException($"Prompt file not found: {fileName}");
            }

            var content = await File.ReadAllTextAsync(filePath);

            // Cache it for future requests
            _promptCache.TryAdd(fileName, content);

            _logger.LogInformation("Prompt loaded and cached: {FileName}", fileName);
            return content;
        }

       
        public Task<string> ReadFileAsync() => ReadPromptAsync("ResumeAnalysis.txt");
        public Task<string> ResumeExtract() => ReadPromptAsync("ResumeExtract.txt");
        public Task<string> JSearchParams() => ReadPromptAsync("JSearchParamsPrompt.txt");
        public Task<string> CoverLetterPrompt() => ReadPromptAsync("CoverLetterPrompt.txt");
        public Task<string> CareerAdvicePrompt() => ReadPromptAsync("MasterPrompt.txt");
        public Task<string> InterviewQuestionsPrompt() => ReadPromptAsync("InterviewQuestionsPrompt.txt");

        public string ExtractTextFromPdf(Stream stream)
        {
            var text = new StringBuilder();
            try
            {
                using var document = PdfDocument.Open(stream);
                foreach (var page in document.GetPages())
                {
                    text.AppendLine(page.Text);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("PDF extraction failed: {Message}", ex.Message);
                throw new InvalidOperationException("Failed to extract text from PDF.", ex);
            }

            var result = text.ToString().Trim();

            if (string.IsNullOrWhiteSpace(result))
                throw new InvalidOperationException("PDF appears to be empty or scanned. Please upload a text-based PDF.");

            return result;
        }
    }
}