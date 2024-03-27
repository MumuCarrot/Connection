using Newtonsoft.Json;

namespace LogInPage
{
    public class UserPackege
    {
        [JsonProperty("users")]
        public List<User> users = [];
    }

    /// <summary>
    /// An implementation of user
    /// </summary>
    public class User
    {
        [JsonProperty("username")]
        public string UserName { get; set; } = string.Empty;
        [JsonProperty("login")]
        public string Login { get; set; } = string.Empty;
        [JsonProperty("password")]
        public string Password { get; set; } = string.Empty;
        [JsonProperty("aboutme")]
        public string AboutMe { get; set; } = string.Empty;
        [JsonProperty("profilepicturepath")]
        public string ProfilePicturePath { get; set; } = string.Empty;
    }
}
