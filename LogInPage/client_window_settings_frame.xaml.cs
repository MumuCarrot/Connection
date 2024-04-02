using Microsoft.Win32;
using System.IO;
using System.Windows.Controls;

namespace LogInPage
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class ClientWindowSettingsFrame : Page
    {
        private ClientWindow RefClientWindow { get; set; }
        public ClientWindowSettingsFrame(ClientWindow cw)
        {
            InitializeComponent();
            RefClientWindow = cw;
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            UserName.Text = RefClientWindow.client.CurrentUser?.UserName;
            UserLogin.Text = RefClientWindow.client.CurrentUser?.Login;
            AboutMe.Text = RefClientWindow.client.CurrentUser?.AboutMe;
        }

        private void Update_Click(object sender, System.Windows.RoutedEventArgs e)
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
                RefClientWindow.client.PatchRequestProfilePicture(selectedFileName);
            }
        }

        private void ChangePasswordBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ChangePasswordWindow cpw = new(RefClientWindow.client);

            cpw.ShowDialog();
        }

        private void LeaveProfileBtn_Click(object sender, System.Windows.RoutedEventArgs e)
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
