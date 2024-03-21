using Microsoft.Win32;
using System.Windows.Controls;

namespace LogInPage
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class ClientWindowSettingsFrame : Page
    {
        public ClientWindowSettingsFrame()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            UserName.Text = Client.CurrentUser?.UserName;
            UserLogin.Text = Client.CurrentUser?.Login;
            AboutMe.Text = Client.CurrentUser?.AboutMe;
        }

        private void Update_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (UserLogin.Text != Client.CurrentUser?.Login ||
                UserName.Text != Client.CurrentUser?.UserName ||
                AboutMe.Text != Client.CurrentUser?.AboutMe) 
            {
                if (Client.CurrentUser is not null) 
                { 
                    Client.CurrentUser.UserName = UserName.Text;
                    Client.CurrentUser.Login = UserLogin.Text;
                    Client.CurrentUser.AboutMe = AboutMe.Text;
                    Client.UpdateUser();
                }
            }
        }

        private void ChangeAvatar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Image file (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*",
                Title = "Choose your file"
            };

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string selectedFileName = openFileDialog.FileName;
                Client.UploadAvatar(selectedFileName);
            }
        }
    }
}
