using CareerSphere.ApiModels;
using CareerSphere.ApiModels.ChatBotApiModel;
using CareerSphere.ApiModels.ChatBotModels;
using CareerSphere.Manager.JobManager;
using CareerSphere.Services.AiChatBotService;
using CareerSphere.Services.CarrerAgentService;
using CareerSphere.Services.FileReader;
using System.Text;
using System.Text.RegularExpressions;

namespace CareerSphere.Services.CareerAgentService
{
    public class CareerAgentService : ICareerAgentService
    {
        private readonly IOpenRouterService _openRouterService;
        private readonly IJobManager _jobManager;
        private readonly IFileReader _fileReader;
        private readonly ILogger<CareerAgentService> _logger;

        private const int MaxSteps = 5;

        public CareerAgentService(
            IOpenRouterService openRouterService,
            IJobManager jobManager,
            IFileReader fileReader,
            ILogger<CareerAgentService> logger)
        {
            _openRouterService = openRouterService;
            _jobManager = jobManager;
            _fileReader = fileReader;
            _logger = logger;
        }

       
        public async Task<AgentResponse> RunAgentAsync(
            string? userGoal,
            string? resumeText)
        {
            _logger.LogInformation(
                "ReAct Agent started. Goal: {Goal}", userGoal);

            // Load master agent system prompt
            var systemPrompt = await _fileReader.CareerAdvicePrompt();

            // Build initial user message
            var userMessage = BuildUserMessage(userGoal, resumeText);

            // Master agent conversation memory
            var messages = new List<AiMessage>
            {
                new AiMessage { role = "system", content = systemPrompt },
                new AiMessage { role = "user",   content = userMessage  }
            };

            var steps = new List<ReActStep>();
            var calledTools = new HashSet<string>(); // prevent duplicate calls

            // =========================================
            // 🔹 THE REACT LOOP
            // =========================================
            for (int i = 0; i < MaxSteps; i++)
            {
                _logger.LogInformation(
                    "ReAct iteration {Step} of {Max}", i + 1, MaxSteps);

                // Master agent thinks
                var aiResponse = await _openRouterService
                    .SendRawAsync(messages);

                _logger.LogInformation(
                    "AI response: {Response}", aiResponse);

                // Add to memory
                messages.Add(new AiMessage
                {
                    role = "assistant",
                    content = aiResponse
                });

                // Parse master agent decision
                var thought = ParseThought(aiResponse);
                var action = ParseAction(aiResponse);
                var finalAnswer = ParseFinalAnswer(aiResponse);

                // ─── CASE 1: Master has final answer ───
                if (!string.IsNullOrWhiteSpace(finalAnswer))
                {
                    _logger.LogInformation(
                        "Agent completed in {Steps} steps", i + 1);

                    steps.Add(new ReActStep
                    {
                        StepNumber = i + 1,
                        Thought = thought,
                        Action = "FINAL ANSWER",
                        Observation = finalAnswer
                    });

                    return new AgentResponse
                    {
                        FinalAnswer = finalAnswer,
                        Steps = steps,
                        TotalSteps = i + 1,
                        IsComplete = true
                    };
                }

                // ─── CASE 2: Master wants to call slave ───
                if (!string.IsNullOrWhiteSpace(action))
                {
                    var toolCall = ParseToolCall(action);

                    // Guard — prevent same slave called twice
                    if (toolCall != null &&
                        calledTools.Contains(toolCall.ToolName))
                    {
                        _logger.LogWarning(
                            "Slave {Tool} already called. Skipping.",
                            toolCall.ToolName);

                        messages.Add(new AiMessage
                        {
                            role = "user",
                            content = $"OBSERVATION: {toolCall.ToolName} " +
                                      $"already called. " +
                                      $"Use existing data to continue."
                        });
                        continue;
                    }

                    // Call slave agent
                    var observation = await CallSlaveAgentAsync(toolCall);

                    // Track this slave as called
                    if (toolCall != null)
                        calledTools.Add(toolCall.ToolName);

                    // Record step
                    steps.Add(new ReActStep
                    {
                        StepNumber = i + 1,
                        Thought = thought,
                        Action = action,
                        Observation = observation
                    });

                    // Feed slave result back to master
                    messages.Add(new AiMessage
                    {
                        role = "user",
                        content = $"OBSERVATION: {observation}"
                    });

                    continue;
                }

                // ─── CASE 3: Master confused — nudge ───
                _logger.LogWarning(
                    "No action or final answer on step {Step}. Nudging.",
                    i + 1);

                messages.Add(new AiMessage
                {
                    role = "user",
                    content = "Please continue. " +
                              "Use ACTION: to call a tool " +
                              "or FINAL ANSWER: if you have enough information."
                });
            }

            // Max steps reached
            _logger.LogWarning("Agent reached max steps.");

            return new AgentResponse
            {
                FinalAnswer = "I was unable to complete the full analysis. " +
                              "Please try with a more specific goal.",
                Steps = steps,
                TotalSteps = MaxSteps,
                IsComplete = false
            };
        }

       
        private async Task<string> CallSlaveAgentAsync(ToolCall? toolCall)
        {
            if (toolCall == null)
                return "Could not parse tool call. Continuing.";

            _logger.LogInformation(
                "Calling slave agent: {Tool}", toolCall.ToolName);

            try
            {
                return toolCall.ToolName.ToLower().Trim() switch
                {
                    "fetch_jobs"
                        => await FetchJobsAgentAsync(toolCall.Parameters),

                    "get_skill_roadmap"
                        => await SkillRoadmapAgentAsync(toolCall.Parameters),

                    "get_interview_questions"
                        => await InterviewQuestionsAgentAsync(toolCall.Parameters),

                    "get_salary_info"
                        => await SalaryInfoAgentAsync(toolCall.Parameters),

                    _ => $"Unknown tool: {toolCall.ToolName}. " +
                         $"Available: fetch_jobs, get_skill_roadmap, " +
                         $"get_interview_questions, get_salary_info"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Slave {Tool} failed: {Error}",
                    toolCall.ToolName, ex.Message);

                return $"Tool {toolCall.ToolName} failed. " +
                       $"Continuing with available data.";
            }
        }

        // =========================================
        // 🔹 SLAVE AGENT 1 — Job Fetcher
        // Calls JSearchService → returns job summary
        // =========================================
        private async Task<string> FetchJobsAgentAsync(
            Dictionary<string, string> parameters)
        {
            var role = parameters.GetValueOrDefault(
                "role", "Software Developer");
            var location = parameters.GetValueOrDefault(
                "location", "India");

            _logger.LogInformation(
                "FetchJobs slave: role={Role}, location={Location}",
                role, location);

            var jobs = await _jobManager.GetJobsForResumeAsync(
                role, location, null, 0);

            if (jobs == null || jobs.Count == 0)
                return $"No jobs found for {role} in {location}. " +
                       $"Try different role or location.";

            var summary = jobs
                .Take(5)
                .Select(j =>
                    $"- {j.Title} at {j.Company} | {j.Location}");

            return $"Found {jobs.Count} jobs for {role} in {location}:\n" +
                   string.Join("\n", summary);
        }

        
        private async Task<string> SkillRoadmapAgentAsync(
            Dictionary<string, string> parameters)
        {
            var currentSkills = parameters.GetValueOrDefault(
                "currentSkills", "");
            var targetRole = parameters.GetValueOrDefault(
                "targetRole", "Software Developer");

            _logger.LogInformation(
                "SkillRoadmap slave: skills={Skills}, target={Target}",
                currentSkills, targetRole);

            var messages = new List<AiMessage>
            {
                new AiMessage
                {
                    role    = "system",
                    content = "You are a career coach. " +
                              "Give top 5 specific skills to learn " +
                              "with realistic timeline. " +
                              "Be specific not generic. No markdown."
                },
                new AiMessage
                {
                    role    = "user",
                    content = $"Current skills: {currentSkills}\n" +
                              $"Target role: {targetRole}\n" +
                              $"What should I learn and in what order?"
                }
            };

            return await _openRouterService.SendRawAsync(messages);
        }

       
        private async Task<string> InterviewQuestionsAgentAsync(
            Dictionary<string, string> parameters)
        {
            var role = parameters.GetValueOrDefault(
                "role", "Software Developer");

           

            var result = await _openRouterService
                .GenerateInterviewQuestionsAsync(role);

            if (result?.Questions == null ||
                result.Questions.Count == 0)
                return "Could not generate interview questions.";

            var questions = result.Questions
                .Take(5)
                .Select(q =>
                    $"- [{q.Difficulty}] {q.Question}");

            return $"Top interview questions for {role}:\n" +
                   string.Join("\n", questions);
        }

        
        private async Task<string> SalaryInfoAgentAsync(
            Dictionary<string, string> parameters)
        {
            var role = parameters.GetValueOrDefault(
                "role", "Software Developer");
            var location = parameters.GetValueOrDefault(
                "location", "India");

            _logger.LogInformation(
                "SalaryInfo slave: role={Role}, location={Location}",
                role, location);

            var messages = new List<AiMessage>
            {
                new AiMessage
                {
                    role    = "system",
                    content = "You are a salary expert for Indian tech industry. " +
                              "Give realistic salary ranges in LPA. " +
                              "Be specific. No markdown."
                },
                new AiMessage
                {
                    role    = "user",
                    content = $"Salary for {role} in {location} India?\n" +
                              $"Give ranges for:\n" +
                              $"Fresher (0-1yr)\n" +
                              $"Mid level (2-4yr)\n" +
                              $"Senior (5+yr)"
                }
            };

            return await _openRouterService.SendRawAsync(messages);
        }

        // =========================================
        // 🔹 HELPERS
        // =========================================

        // Build initial message combining goal + resume
        private string BuildUserMessage(
            string? userGoal,
            string? resumeText)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(userGoal))
                sb.AppendLine($"USER GOAL:\n{userGoal}\n");

            if (!string.IsNullOrWhiteSpace(resumeText))
            {
                // Trim resume to avoid token overflow
                var trimmed = resumeText.Length > 6000
                    ? resumeText.Substring(0, 6000)
                    : resumeText;

                sb.AppendLine($"RESUME:\n{trimmed}\n");
            }

            sb.AppendLine(
                "Help this user achieve their specific goal. " +
                "Only use tools you genuinely need. " +
                "Be specific and personalized.");

            return sb.ToString();
        }

        // Parse THOUGHT from AI response
        private string ParseThought(string response)
        {
            var match = Regex.Match(response,
                @"THOUGHT:\s*(.+?)(?=ACTION:|FINAL ANSWER:|$)",
                RegexOptions.Singleline | RegexOptions.IgnoreCase);

            return match.Success
                ? match.Groups[1].Value.Trim()
                : string.Empty;
        }

        // Parse ACTION from AI response
        private string ParseAction(string response)
        {
            var match = Regex.Match(response,
                @"ACTION:\s*(.+?)(?=THOUGHT:|OBSERVATION:|FINAL ANSWER:|$)",
                RegexOptions.Singleline | RegexOptions.IgnoreCase);

            return match.Success
                ? match.Groups[1].Value.Trim()
                : string.Empty;
        }

        // Parse FINAL ANSWER from AI response
        private string ParseFinalAnswer(string response)
        {
            var match = Regex.Match(response,
                @"FINAL ANSWER:\s*(.+?)$",
                RegexOptions.Singleline | RegexOptions.IgnoreCase);

            return match.Success
                ? match.Groups[1].Value.Trim()
                : string.Empty;
        }

        // Parse tool name + parameters from ACTION string
        private ToolCall? ParseToolCall(string action)
        {
            try
            {
                // fetch_jobs(role="Software Developer", location="Pune")
                var nameMatch = Regex.Match(action, @"^(\w+)\(");
                if (!nameMatch.Success) return null;

                var toolName = nameMatch.Groups[1].Value.Trim();
                var parameters = new Dictionary<string, string>();

                // Extract key="value" pairs
                var paramMatches = Regex.Matches(action,
                    @"(\w+)=""([^""]+)""");

                foreach (Match m in paramMatches)
                    parameters[m.Groups[1].Value] = m.Groups[2].Value;

                return new ToolCall
                {
                    ToolName = toolName,
                    Parameters = parameters
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "ParseToolCall failed: {Error}", ex.Message);
                return null;
            }
        }
    }
}