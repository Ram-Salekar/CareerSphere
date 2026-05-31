using CareerSphere.ApiModels;
using CareerSphere.ApiModels.ReActAgentModels;
using CareerSphere.Services.CareerAgentService;
using CareerSphere.Services.CarrerAgentService;
using CareerSphere.Services.FileReader;

namespace CareerSphere.Manager.CareerAgentManager
{
    public class CareerAgentManager : ICareerAgentManager
    {
        private readonly ICareerAgentService _agentService;
        private readonly IFileReader _fileReader;
        private readonly ILogger<CareerAgentManager> _logger;

        public CareerAgentManager(
            ICareerAgentService agentService,
            IFileReader fileReader,
            ILogger<CareerAgentManager> logger)
        {
            _agentService = agentService;
            _fileReader = fileReader;
            _logger = logger;
        }

        public async Task<AgentUserResponse> RunAgentAsync(
            AgentQueryRequest request,
            Guid userId)
        {
            
            if (string.IsNullOrWhiteSpace(request.Prompt)
                && request.ResumeFile == null)
            {
                throw new InvalidOperationException(
                    "Please provide your career goal " +
                    "or upload your resume.");
            }

           
            string? resumeText = null;
            if (request.ResumeFile != null)
            {
                var extension = Path.GetExtension(
                    request.ResumeFile.FileName).ToLower();

               

                using var stream = request.ResumeFile.OpenReadStream();
                resumeText = _fileReader.ExtractTextFromPdf(stream);

                if (string.IsNullOrWhiteSpace(resumeText))
                    throw new InvalidOperationException(
                        "Could not extract text from PDF. " +
                        "Please ensure it is not a scanned document.");

                _logger.LogInformation(
                    "Resume extracted. Length: {Length} chars",
                    resumeText.Length);
            }

           
            _logger.LogInformation(
                "Starting ReAct agent for user {UserId}", userId);

            var agentResult = await _agentService.RunAgentAsync(
                request.Prompt,
                resumeText);

          
            return new AgentUserResponse
            {
                Response = agentResult.FinalAnswer,
                IsComplete = agentResult.IsComplete,
                TotalSteps = agentResult.TotalSteps
            };
        }
    }
}