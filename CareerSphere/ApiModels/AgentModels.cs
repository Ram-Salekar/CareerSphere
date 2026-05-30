using System.Collections.Generic;

namespace CareerSphere.ApiModels
{
    public class AgentResponse
    {
        public string FinalAnswer { get; set; }
        public bool IsComplete { get; set; }
        public int TotalSteps { get; set; }
        public List<ReActStep> Steps { get; set; } = new List<ReActStep>();
    }

    public class ReActStep
    {
        public int StepNumber { get; set; }
        public string Thought { get; set; }
        public string Action { get; set; }
        public string Observation { get; set; }
    }

    public class ToolCall
    {
        public string ToolName { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
    }

    // What user sees
    public class AgentUserResponse
    {
        public string Response { get; set; }
        public bool IsComplete { get; set; }
        public int TotalSteps { get; set; }
    }
}
