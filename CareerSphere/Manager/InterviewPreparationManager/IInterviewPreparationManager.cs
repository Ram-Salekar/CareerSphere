using CareerSphere.ApiModels.InterviewPrepModels;

namespace CareerSphere.Manager.InterviewPreparationManager
{
    public interface IInterviewPreparationManager
    {
        Task<CoverLetterResponseModel> GenerateCoverLetterAsync(CoverLetterRequestModel requestModel, Guid userId);
    }
}
