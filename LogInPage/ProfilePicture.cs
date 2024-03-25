using Newtonsoft.Json;

namespace LogInPage
{
    /// <summary>
    /// An implementation of profile image
    /// </summary>
    class ProfilePicture
    {
        [JsonProperty("datetime")]
        public string AvatarDateTime { get; set; } = string.Empty;
        [JsonProperty("username")]
        public string UserName { get; set; } = string.Empty;
        [JsonProperty("login")]
        public string Login { get; set; } = string.Empty;
        [JsonProperty("bytes")]
        public byte[] Bytes { get; set; } = new byte[1024];
    }
}
