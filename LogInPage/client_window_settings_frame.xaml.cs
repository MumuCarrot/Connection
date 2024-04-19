using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LogInPage
{
    public partial class ClientWindowSettingsFrame : Page
    {
        public Brush PPBackground { set { ProfilePictureHolder.Background = value; } }
        public ProfilePicture PPPicture { set { PictureOfPPHolder.Source = value.ToSource(ProfilePictureSize.i64px); } }

        private ClientWindow RefClientWindow { get; set; }
        private ProfilePicture? UserProfilePicture;

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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (RefClientWindow.client.CurrentUser is not null && UserProfilePicture is not null)
            {
                UserName.Text = RefClientWindow.client.CurrentUser.UserName;
                UserLogin.Text = RefClientWindow.client.CurrentUser.Login;
                AboutMe.Text = RefClientWindow.client.CurrentUser.AboutMe;
            }
        }

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

        private void ChangeAvatar_Click(object sender, RoutedEventArgs e)
        {
            ProfilePictureSetter pps = new ProfilePictureSetter(RefClientWindow);
            pps.ShowDialog();
        }

        private void ChangePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow cpw = new(RefClientWindow.client);

            cpw.ShowDialog();
        }

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
