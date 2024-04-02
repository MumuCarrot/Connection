using Newtonsoft.Json;

namespace LogInPage
{
    public class MessageConteiner() 
    {
        [JsonProperty("messages")]
        public List<Message> Messages { get; set; } = [];
    }

    public class Chat
    {
        public string? Id { get; set; }

        public string[]? Chatusers { get; set; }

        public Message[]? Messages { get; set; }
    }

    public class Message
    {
        public string? Username { get; set; }

        public ContentMessage? Content { get; set; }

        public DateTime Time { get; set; }
    }

    public class ContentMessage
    {
        public string? Text { get; set; }

        public string? Image { get; set; }
    }
}
