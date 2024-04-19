using System.Windows.Controls;
using System.Windows.Media;

namespace LogInPage
{
    public partial class userBioPage : Page
    {
        private User? _user = null;
        public userBioPage(User user)
        {
            InitializeComponent();

            _user = user;

            PPHolder.Source = user.UserProfilePicture.ToSource(ProfilePictureSize.i64px);
            Brush? convertedBrush = new BrushConverter().ConvertFrom(user.UserProfilePicture.PPColor) as SolidColorBrush;
            if (convertedBrush is not null)
            {
                PPBackground.Background = convertedBrush;
            }
            UserName.Text = user.UserName;
            UserLogin.Text = user.Login;
            AboutMe.Text = user.AboutMe;
        }
    }
}
