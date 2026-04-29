using CareerSphere.ApiModels.InterviewPrepModels;
using CareerSphere.Services.AiChatBotService;
using CareerSphere.Services.FileReader;

namespace CareerSphere.Manager.InterviewPreparationManager
{
    public class InterviewPreparationManager : IInterviewPreparationManager
    {
        private readonly IOpenRouterService _openRouterService;
        private readonly IFileReader fileReader;
        public InterviewPreparationManager(IOpenRouterService openRouter , IFileReader file) { 
            _openRouterService = openRouter;
            fileReader = file;
        }

      public async  Task<CoverLetterResponseModel> GenerateCoverLetterAsync(CoverLetterRequestModel requestModel, Guid userId)
        {
            string extractedText;

            using (var stream = requestModel.Resume.OpenReadStream())
            {
                extractedText = fileReader.ExtractTextFromPdf(stream);
            }
            var response = await _openRouterService.GenerateCoverLetterAsync(extractedText, requestModel.JobDescription);
            return new CoverLetterResponseModel
            {
                CoverLetterText = response,
                GeneratedAt = DateTime.UtcNow
            };

        }
    }
}
