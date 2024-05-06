using Newtonsoft.Json;

namespace Connect.profilePicture
{
    /// <summary>
    /// Size of profile picture
    /// </summary>
    public enum ProfilePictureSize
    {
        i48px = 48,
        i64px = 64,
        i128px = 128,
    }

    /// <summary>
    /// An implementation of profile picture
    /// </summary>
    public class ProfilePicture : ICloneable
    {
        public object Clone()
        {
            return new ProfilePicture
            {
                //PPColor = this.PPColor,
                PictureName = this.PictureName,
            };
        }

        /// <summary>
        /// Current background of profile picture
        /// </summary>
        //[JsonProperty("color")]
        //public string PPColor { get; set; } = new SolidColorBrush(Color.FromArgb(255, 220, 241, 255)).ToString();
        /// <summary>
        /// Current picture name
        /// </summary>
        [JsonProperty("image_name")]
        public string PictureName { get; set; } = "default";
        /// <summary>
        /// Variable for packing and requesting
        /// Implamenting login of user
        /// </summary>
        [JsonProperty("login")]
        public string? Login { get; set; } = null;

        /// <summary>
        /// Returns image sources by current name and size
        /// </summary>
        /// <param name="ppSize">
        /// Size of picture
        /// </param>
        /// <returns>
        /// Image source
        /// </returns>
        /// <exception cref="Exception">
        /// Image source wasn't found
        /// </exception>
        /*public ImageSource ToSource(ProfilePictureSize ppSize) => PictureName switch
        {
            "default" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\default_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            "angry" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\angry_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            "dizzy" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\dizzy_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            "flushed" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\flushed_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            "frown" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\frown_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            "grimace" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\grimace_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            "grin" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\grin_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            "kissing" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\kissing_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            "laughing" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\laughing_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            "smiling" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\smiling_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            "tongued" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\tongued_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            _ => throw new Exception($"Picture by name: {PictureName} wasn't found!")
        };*/

        /// <summary>
        /// Returns name of image by image source
        /// </summary>
        /// <param name="imageSource">
        /// Image source
        /// </param>
        /// <returns>
        /// Name of image
        /// </returns>
        //public static string ToString(ImageSource imageSource) => imageSource.ToString()[(imageSource.ToString().LastIndexOf("/") + 1)..imageSource.ToString().LastIndexOf("_x")];
    }

    /// <summary>
    /// New methods for string class
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Convert any constant string into ImageSource
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ppSize"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /*public static ImageSource ToSource(this string str, ProfilePictureSize ppSize) => str switch
        {
            "default" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\default_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            "angry" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\angry_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            "dizzy" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\dizzy_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            "flushed" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\flushed_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            "frown" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\frown_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            "grimace" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\grimace_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            "grin" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\grin_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            "kissing" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\kissing_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            "laughing" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\laughing_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            "smiling" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\smiling_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            "tongued" => new BitmapImage(new Uri($"\\Source\\user_pp\\user_pp_{(int)ppSize}px\\tongued_x{(int)ppSize}px.png", UriKind.RelativeOrAbsolute)),
            _ => throw new Exception($"Picture by name: {str} wasn't found!")
        };*/
    }
}
