namespace Connect.message
{
    /// <summary>
    /// Chat implementation
    /// </summary>
    public class Chat
    {
        public string? Id { get; set; }

        public string[] Chatusers { get; set; } = new string[0];

        public Message[] Messages { get; set; } = new Message[0];
    }

    /// <summary>
    /// Message implementation
    /// </summary>
    public class Message
    {
        public string? Username { get; set; }

        public ContentMessage? Content { get; set; }

        public DateTime? Time { get; set; }
    }

    /// <summary>
    /// Message content implementation
    /// </summary>
    public class ContentMessage
    {
        public string? Text { get; set; }

        public string? Image { get; set; }
    }
}
