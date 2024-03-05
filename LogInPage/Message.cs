using Newtonsoft.Json;

namespace LogInPage
{
    public class MessageConteiner() 
    {
        [JsonProperty("messages")]
        public List<Message> Messages { get; set; } = [];
    }

    /// <summary>
    /// Class that implements message
    /// </summary>
    /// <param name="dateTime">
    /// Date and time of message
    /// </param>
    /// <param name="userName">
    /// Login of user
    /// </param>
    /// <param name="message">
    /// Message that user send
    /// </param>
    /// <param name="type">
    /// Type of message;
    /// it can be:
    /// text,
    /// pic,
    /// sticker,
    /// gif
    /// </param>
    public class Message
    {
        [JsonProperty("datetime")]
        public string MessageDateTime { get; set; } = string.Empty;
        [JsonProperty("login")]
        public string Login { get; set; } = string.Empty;
        [JsonProperty("content")]
        public string Content { get; set; } = string.Empty;
        [JsonProperty("type")]
        public string MessageType { get; set; } = string.Empty;
    }
}
