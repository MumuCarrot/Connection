using Newtonsoft.Json;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LogInPage
{
    public enum ProfilePictureSize 
    { 
        i48px = 48,
        i64px = 64,
        i128px = 128,
    }
    /// <summary>
    /// An implementation of profile image
    /// </summary>
    public class ProfilePicture
    {
        [JsonProperty("color")]
        public Brush PPColor { get; set; } = new SolidColorBrush(Color.FromArgb(255, 220, 241, 255));
        [JsonProperty("image_name")]
        public string pictureName = "default";

        public ImageSource ToSource(ProfilePictureSize ppSize) => pictureName switch
        {
            "default" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\default_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            _ => throw new Exception($"Picture by name: {pictureName} wasn't found!")
        };
    }
}
