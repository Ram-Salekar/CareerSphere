using CareerSphere.Utility.ChatBotModels;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using CareerSphere.Services.FileReader;

namespace CareerSphere.Services.AiChatBotService
{
    public class OpenRouterService : IOpenRouterService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IFileReader _fileReader;

        public OpenRouterService(HttpClient httpClient, IConfiguration configuration,IFileReader fileReader)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _fileReader = fileReader;
        }

        

        public async Task<string> GetResponseModel(string message)
        {
            var apiKey = _configuration["OpenRouter:ApiKey"];
            var model = _configuration["OpenRouter:Model"];
            var baseUrl = _configuration["OpenRouter:BaseUrl"];

            var requestBody = new
            {
                model = model,
                messages = new[]
                {
                    new { role = "user", content = message }
                }
            };
            var json = JsonSerializer.Serialize(requestBody);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/chat/completions");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            httpRequest.Headers.Add("HTTP-Referer", "https://yourdomain.com");
            httpRequest.Headers.Add("X-Title", "YourAppName");

            httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<OpenRouterResponse>(
                responseContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            var aiMessage = result?.choices?
                                  .FirstOrDefault()?
                                  .message?
                                  .content;

            return aiMessage ?? "No response from AI.";


        }

       
        public async Task<string> ResumeAnalyzingAgent(string resume)
        {
            var apiKey = _configuration["OpenRouter:ApiKey"];
            var model = _configuration["OpenRouter:Model"];
            var baseUrl = _configuration["OpenRouter:BaseUrl"];
            string prompt = _fileReader.ReadFileAsync().Result;
            var requestBody = new
            {
                 // Synchronously read the prompt template
                model = model,
                messages = new[]
                {
                    new {role = "system", content =prompt},
                    new { role = "user", content = resume }
                }
            };
            var json = JsonSerializer.Serialize(requestBody);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/chat/completions");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            httpRequest.Headers.Add("HTTP-Referer", "https://yourdomain.com");
            httpRequest.Headers.Add("X-Title", "YourAppName");

            httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<OpenRouterResponse>(
                responseContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            var aiMessage = result?.choices?
                                  .FirstOrDefault()?
                                  .message?
                                  .content;

            return aiMessage ?? "No response from AI.";
        }

    }
}
