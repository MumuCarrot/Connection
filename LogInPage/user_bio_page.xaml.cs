using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LogInPage
{
    public partial class UserBioPage : Page
    {
        private readonly User? _user = null;
        private readonly Client? _client = null;
        private readonly ClientWindow? _clientWindow = null;
        public UserBioPage(User user, ClientWindow clientWindow)
        {
            InitializeComponent();

            _user = user;
            _client = clientWindow.client;
            _clientWindow = clientWindow;

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

        private void ContactUserBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_client is not null && _client.UserChatPreload is not null)
            {
                string? id = null;
                foreach (var chat in _client.UserChatPreload)
                {
                    if (chat.Chatusers is not null && chat.Chatusers.Any(u => u == _user?.Login))
                    {
                        id = chat.Id;
                        break;
                    }
                }

                if (id is not null)
                {
                    if (_clientWindow is not null)
                    {
                        _clientWindow.SearchBox = string.Empty;
                        _clientWindow.ChatFrame.Content = _clientWindow.chatList;
                        if (_clientWindow.clientWindowChatFrameList is not null)
                        {
                            var chat = _clientWindow.clientWindowChatFrameList.Find(c => c.chat.Id == id);

                            _clientWindow.ChatFrame.Content = chat;
                        }
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
