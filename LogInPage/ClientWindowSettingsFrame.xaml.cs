using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LogInPage
{
    /// <summary>
    /// Client window settings frame
    /// </summary>
    public partial class ClientWindowSettingsFrame : Page
    {
        /// <summary>
        /// Profile picture background
        /// </summary>
        public Brush PPBackground { set { ProfilePictureHolder.Background = value; } }
        /// <summary>
        /// Profile picture image
        /// </summary>
        public ProfilePicture PPPicture { set { PictureOfPPHolder.Source = value.ToSource(ProfilePictureSize.i64px); } }
        /// <summary>
        /// Current client window
        /// </summary>
        private ClientWindow RefClientWindow { get; set; }
        /// <summary>
        /// Current profile picture
        /// </summary>
        private ProfilePicture? UserProfilePicture;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="cw">
        /// Current client window
        /// </param>
        public ClientWindowSettingsFrame(ClientWindow cw)
        {
            InitializeComponent();

            RefClientWindow = cw;

            if (RefClientWindow.client.CurrentUser is not null) 
            {
                UserProfilePicture = (ProfilePicture)RefClientWindow.client.CurrentUser.UserProfilePicture.Clone();

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                { 
                    PictureOfPPHolder.Source = UserProfilePicture.ToSource(ProfilePictureSize.i64px);
                    Brush? convertedBrush = new BrushConverter().ConvertFrom(UserProfilePicture.PPColor) as SolidColorBrush;
                    if (convertedBrush is not null) 
                    {
                        ProfilePictureHolder.Background = convertedBrush;
                    }
                }));
            }
        }

        /// <summary>
        /// Page loaded
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (RefClientWindow.client.CurrentUser is not null && UserProfilePicture is not null)
            {
                UserName.Text = RefClientWindow.client.CurrentUser.UserName;
                UserLogin.Text = RefClientWindow.client.CurrentUser.Login;
                AboutMe.Text = RefClientWindow.client.CurrentUser.AboutMe;
            }
        }
        /// <summary>
        /// Update button click
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (UserLogin.Text != RefClientWindow.client.CurrentUser?.Login ||
                UserName.Text != RefClientWindow.client.CurrentUser?.UserName ||
                AboutMe.Text != RefClientWindow.client.CurrentUser?.AboutMe)
            {
                if (RefClientWindow.client.CurrentUser is not null)
                {
                    RefClientWindow.client.CurrentUser.UserName = UserName.Text;
                    RefClientWindow.client.CurrentUser.Login = UserLogin.Text;
                    RefClientWindow.client.CurrentUser.AboutMe = AboutMe.Text;
                    RefClientWindow.client.PatchRequestUser();
                }
            }
        }
        /// <summary>
        /// Change profile picture click
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
        private void ChangeProfilePicture_Click(object sender, RoutedEventArgs e)
        {
            ProfilePictureSetter pps = new ProfilePictureSetter(RefClientWindow);
            pps.ShowDialog();
        }
        /// <summary>
        /// Change password button click
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
        private void ChangePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow cpw = new(RefClientWindow.client);

            cpw.ShowDialog();
        }
        /// <summary>
        /// Leave profile button click
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
        private void LeaveProfileBtn_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("user_account_lock.xml"))
            {
                File.Delete("user_account_lock.xml");

                if (File.Exists("user_chat_lock.xml"))
                {
                    File.Delete("user_chat_lock.xml");
                }
            }

            RefClientWindow.client.Close();
            var mainWindow = new MainWindow();
            mainWindow.Show();
            RefClientWindow.Close();
        }
    }
}
