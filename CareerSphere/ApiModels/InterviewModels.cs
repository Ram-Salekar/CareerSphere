using System.Collections.Generic;

namespace CareerSphere.ApiModels
{
    public class InterviewQuestionResult
    {
        public string Role { get; set; }
        public List<InterviewQuestion> Questions { get; set; } = new List<InterviewQuestion>();
    }

    public class InterviewQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Difficulty { get; set; }
    }
}
