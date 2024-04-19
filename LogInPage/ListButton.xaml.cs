using System.Windows.Controls;
using System.Windows.Media;

namespace LogInPage
{
    public partial class ListButton : UserControl
    {
        public event EventHandler? Click = null;
        private object? _data = null;

        public ListButton()
        {
            InitializeComponent();
        }

        public ListButton(object? obj)
            : this()
        {
            if (obj is not null) 
            {
                _data = obj;
                if (obj is User user) 
                {
                    Brush? convertedBrush = new BrushConverter().ConvertFrom(user.UserProfilePicture.PPColor) as SolidColorBrush;
                    if (convertedBrush is not null)
                    {
                        ProfilePictureBackground = convertedBrush;
                    }
                    ProfilePictureSource = user.UserProfilePicture.ToSource(ProfilePictureSize.i64px);
                    TitleText = user.Login;
                    UnderlineText = user.AboutMe;
                }
            }
        }

        public string TitleText
        {
            get { return Title.Text; }
            set { Title.Text = value; }
        }

        public string UnderlineText
        {
            get { return Underline.Text; }
            set { Underline.Text = value; }
        }

        public Brush ProfilePictureBackground 
        {
            get { return PPBackground.Background; }
            set { PPBackground.Background = value; }
        }

        public ImageSource ProfilePictureSource
        {
            get { return PPHolder.Source; }
            set { PPHolder.Source = value; }
        }

        public string? Id { get; set; } = null;

        private void UserControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Click?.Invoke(this, EventArgs.Empty);
        }
    }
}
