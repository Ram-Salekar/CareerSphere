namespace CareerSphere.ApiModels.ChatBotModels
{
    public class ChatBotResponseModel
    {
        public string Response { get; set; }
        public string Content { get; set; }
    }
    public class OpenRouterResponse
    {
        public List<Choice> choices { get; set; }
    }

    public class Choice
    {
        public Message message { get; set; }
    }

    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }

}
