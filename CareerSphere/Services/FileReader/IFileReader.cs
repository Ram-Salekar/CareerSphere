namespace CareerSphere.Services.FileReader
{
    public interface IFileReader
    {
        public Task<string> ReadFileAsync();
        public string ExtractTextFromPdf(Stream stream);
    }
}
