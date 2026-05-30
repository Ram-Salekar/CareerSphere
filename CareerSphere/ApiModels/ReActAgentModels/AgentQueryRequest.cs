namespace CareerSphere.ApiModels.ReActAgentModels
{
    public class AgentQueryRequest
    {
        public string? Prompt { get; set; } 
        public IFormFile? ResumeFile { get; set; }
    }
}
