using CareerSphere.ApiModels.ChatBotApiModel;
using CareerSphere.ApiModels.JSearchApiModels;
using CareerSphere.Manager.JobManager;
using CareerSphere.Repository.ConversationRepos;
using CareerSphere.Repository.MessageRepos;
using CareerSphere.Services.AiChatBotService;
using CareerSphere.Services.FileReader;

namespace CareerSphere.Manager.ChatBotManager
{
    public class ChatBotManager : IChatBotManager
    {
        private readonly IOpenRouterService _openRouterService;
        private readonly IFileReader _fileReader;
        private readonly IMessage messageRepo;
        private readonly IConversation conversationRepo;
        private readonly IJobManager jobManager;

        public ChatBotManager(IOpenRouterService openRouterService, IFileReader fileReader, IMessage message, IConversation conversation, IJobManager jobManager)
        {
            _openRouterService = openRouterService;
            _fileReader = fileReader;
            this .messageRepo = message;
            this.conversationRepo = conversation;
            this.jobManager = jobManager;
        }

        public async Task<MessageResponseApiModel> GetChatBotResponse(MessagePostApiModel message,Guid userId)
        {
            if (message.conversationId == null)
            {
                message.conversationId = Guid.NewGuid();
                ConversationPostApiModel conversation = new ConversationPostApiModel
                {
                    conversationId = message.conversationId.Value,
                    userId = userId,
                    Title = message.content.Length > 20 ? message.content.Substring(0, 20) : message.content,
                    createdAt = DateTime.UtcNow,
                    createdBy = userId.ToString()

                };
                await conversationRepo.PostConversation(conversation);
            }
            await messageRepo.PostMessage(new MessagePostApiModel
                {
                    conversationId = message.conversationId.Value,
                    content = message.content,
                    role = "user",
                    createdAt = DateTime.UtcNow,
                    createdBy = userId.ToString()
                });
            

           var response = await _openRouterService.GetResponseModel(message.content ,(Guid) message.conversationId);

            await messageRepo.PostMessage(new MessagePostApiModel
                    {
                        conversationId = message.conversationId.Value,
                        content = response.content,
                        role = "assistant",
                        createdAt = DateTime.UtcNow,
                        createdBy = userId.ToString()
                    });

            return response;
        }

        public  async Task<string> ResumeAnalyzingAgent(IFormFile resume, Guid userId)
        {
            string extractedText;

            using (var stream = resume.OpenReadStream())
            {
                extractedText = _fileReader.ExtractTextFromPdf(stream);
            }

            var response = await _openRouterService.ResumeAnalyzingAgent(extractedText);
            return response;
        }

        public async Task <List<JobListing>> GetJobByResume(IFormFile resume, Guid userId)
        {
            string extractedText;

            using (var stream = resume.OpenReadStream())
            {
                extractedText = _fileReader.ExtractTextFromPdf(stream);
            }
            var result = await _openRouterService.ExtractJSearchParamsAsync(extractedText);

            if(result == null)
            {
                return null;
            }

            var joblist = await jobManager.GetJobsForResumeAsync(result.Role, result.Location, result.Skills, result.ExperienceYears);


            return joblist;

           
        }

    }
}
