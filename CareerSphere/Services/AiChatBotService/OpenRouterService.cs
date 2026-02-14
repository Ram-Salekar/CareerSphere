using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CareerSphere.ApiModels.ChatBotModels;
using CareerSphere.ApiModels.ChatBotApiModel;
using CareerSphere.Repository.MessageRepos;
using CareerSphere.Services.FileReader;

namespace CareerSphere.Services.AiChatBotService
{
    public class OpenRouterService : IOpenRouterService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IFileReader _fileReader;
        private readonly IMessage _messageRepo;

        public OpenRouterService(
            HttpClient httpClient,
            IConfiguration configuration,
            IFileReader fileReader,
            IMessage messageRepo)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _fileReader = fileReader;
            _messageRepo = messageRepo;
        }

       
        private async Task<string> SendToOpenRouterAsync(List<AiMessage> messages)
        {
            var apiKey = _configuration["OpenRouter:ApiKey"];
            var model = _configuration["OpenRouter:Model"];
            var baseUrl = _configuration["OpenRouter:BaseUrl"];

            var requestBody = new
            {
                model = model,
                messages = messages
            };

            var json = JsonSerializer.Serialize(requestBody);

            using var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                $"{baseUrl}/chat/completions");

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            httpRequest.Headers.Add("HTTP-Referer", "https://CarrerSphere.com");
            httpRequest.Headers.Add("X-Title", "CarrerSphere");

            httpRequest.Content =
                new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<OpenRouterResponse>(
                responseContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return result?.choices?
                .FirstOrDefault()?
                .message?
                .content
                ?? "No response from AI.";
        }

       
        public async Task<MessageResponseApiModel> GetResponseModel(
            string message,
            Guid conversationId)
        {
            var lastMessages =
                await _messageRepo.Last5messages(conversationId);

            var aiMessages = lastMessages
                .OrderBy(m => m.createdAt)
                .Select(m => new AiMessage
                {
                    role = m.role,
                    content = m.content
                })
                .ToList();

            aiMessages.Add(new AiMessage
            {
                role = "user",
                content = message
            });

            var aiResponse = await SendToOpenRouterAsync(aiMessages);

            return new MessageResponseApiModel
            {
                content = aiResponse,
                role = "assistant",
                createdAt = DateTime.UtcNow,
                createdBy = "AI"
            };
        }

        // =========================================
        // 🔹 Resume Analyzer (System + User)
        // =========================================
        public async Task<string> ResumeAnalyzingAgent(string resume)
        {
            var prompt = await _fileReader.ReadFileAsync();

            var messages = new List<AiMessage>
            {
                new AiMessage
                {
                    role = "system",
                    content = prompt
                },
                new AiMessage
                {
                    role = "user",
                    content = resume
                }
            };

            return await SendToOpenRouterAsync(messages);
        }
    }
}