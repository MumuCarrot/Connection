using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Connect.profilePicture;
using Connect.user;

namespace LogInPage
{
    /// <summary>
    /// List button realisation
    /// </summary>
    public partial class ListButton : UserControl
    {
        /// <summary>
        /// Button click event
        /// </summary>
        public event EventHandler? Click = null;
        /// <summary>
        /// Data of button. Expect: User
        /// </summary>
        private object? _data = null;

        /// <summary>
        /// Primary class constructor
        /// </summary>
        public ListButton()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Class that can hold any data
        /// </summary>
        /// <param name="obj"></param>
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

        /// <summary>
        /// Title text alias
        /// </summary>
        public string TitleText
        {
            get { return Title.Text; }
            set { Title.Text = value; }
        }
        /// <summary>
        /// Underline alias
        /// </summary>
        public string UnderlineText
        {
            get { return Underline.Text; }
            set { Underline.Text = value; }
        }
        /// <summary>
        /// PPBackground alias
        /// </summary>
        public Brush ProfilePictureBackground 
        {
            get { return PPBackground.Background; }
            set { PPBackground.Background = value; }
        }
        /// <summary>
        /// PPHolder alias
        /// </summary>
        public ImageSource ProfilePictureSource
        {
            get { return PPHolder.Source; }
            set { PPHolder.Source = value; }
        }
        /// <summary>
        /// Any button Id. Null by default
        /// </summary>
        public string? Id { get; set; } = null;

        /// <summary>
        /// Click invoke
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Click?.Invoke(this, EventArgs.Empty);
        }
    }
}
