using CareerSphere.ApiModels.ChatBotApiModel;
using CareerSphere.ApiModels.ChatBotModels;
using CareerSphere.ApiModels.JSearchApiModels;
using CareerSphere.Repository.MessageRepos;
using CareerSphere.Services.FileReader;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

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


        private async Task<string> TrySendAsync(List<AiMessage> messages, string model)
        {
            var apiKey = _configuration["OpenRouter:ApiKey"];
            var baseUrl = _configuration["OpenRouter:BaseUrl"];
            var timeoutSeconds = int.TryParse(
                _configuration["OpenRouter:TimeoutSeconds"], out var t) ? t : 15;

            var requestBody = new { model, messages };
            var json = JsonSerializer.Serialize(requestBody);

            using var httpRequest = new HttpRequestMessage(
                HttpMethod.Post, $"{baseUrl}/chat/completions");

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);
            httpRequest.Headers.Add("HTTP-Referer", "https://CareerSphere.com");
            httpRequest.Headers.Add("X-Title", "CareerSphere");
            httpRequest.Content =
                new StringContent(json, Encoding.UTF8, "application/json");

            // Per-request timeout — triggers fallback if model is slow
            using var cts = new CancellationTokenSource(
                TimeSpan.FromSeconds(timeoutSeconds));

            var response = await _httpClient.SendAsync(httpRequest, cts.Token);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"Model '{model}' failed with {(int)response.StatusCode}: {errorBody}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<OpenRouterResponse>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result?.choices?
                .FirstOrDefault()?
                .message?
                .content
                ?? "No response from AI.";
        }


        private async Task<string> SendToOpenRouterAsync(List<AiMessage> messages)
        {
            var models = _configuration
                .GetSection("OpenRouter:FallbackModels")
                .Get<List<string>>();

            if (models == null || models.Count == 0)
                throw new Exception("No models configured in OpenRouter:FallbackModels.");

            Exception lastException = null;

            foreach (var model in models)
            {
                try
                {

                    var result = await TrySendAsync(messages, model);

                    return result;
                }
                catch (TaskCanceledException)
                {

                    lastException = new Exception($"Model '{model}' timed out.");
                }
                catch (Exception ex)
                {

                    lastException = ex;
                }
            }

            throw new Exception(
                $"All models failed. Last error: {lastException?.Message}");
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

        public async Task<string> GetJobList(string resume, List<JobListing> joblist)
        {
            var prompt = await _fileReader.ResumeExtract();

            return null;
        }

        public async Task<JSearchParamsResult> ExtractJSearchParamsAsync(string resumeText)
        {
            var prompt = await _fileReader.JSearchParams();

            var messages = new List<AiMessage>
            {
                 new AiMessage { role = "system", content = prompt },
                new AiMessage { role = "user",   content = resumeText }
            };

            var rawJson = await SendToOpenRouterAsync(messages);

            var clean = rawJson
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();

            return JsonSerializer.Deserialize<JSearchParamsResult>(clean,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<string> GenerateCoverLetterAsync(string resumeText, string jobDescription)
        {
            var prompt = await _fileReader.CoverLetterPrompt();

            var userContent = $"""
            RESUME:
            {resumeText}

            JOB DESCRIPTION:
            {jobDescription}
            """;

            var messages = new List<AiMessage>
            {
                new AiMessage { role = "system", content = prompt },
                new AiMessage { role = "user", content = userContent }
            };

            return await SendToOpenRouterAsync(messages);
        }
    }
}