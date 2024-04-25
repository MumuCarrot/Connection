using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Connect.profilePicture;
using Connect.user;
using Connect.message;

namespace LogInPage
{
    /// <summary>
    /// Profile page of other users
    /// </summary>
    public partial class UserBioPage : Page
    {
        /// <summary>
        /// User of current profile page
        /// </summary>
        private readonly User? _user = null;
        /// <summary>
        /// Current client
        /// </summary>
        private readonly Client? _client = null;
        /// <summary>
        /// Current client window
        /// </summary>
        private readonly ClientWindow? _clientWindow = null;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="user">
        /// The user whose page you want to show
        /// </param>
        /// <param name="clientWindow">
        /// Current client window
        /// </param>
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
        /// <summary>
        /// Opens or creates chat with this user
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
        private void ContactUserBtn_Click(object? sender, RoutedEventArgs? e)
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
                    if (_clientWindow is not null && _client.CurrentUser is not null && _user is not null)
                    {
                        _client.PreloadChatIsReady = false;
                        _client.PostRequestChat(_client.CurrentUser.Login, _user.Login);

                        while (!_client.PreloadChatIsReady) Thread.Sleep(100);

                        _clientWindow.SearchBox = string.Empty;
                        _clientWindow.ChatFrame.Content = _clientWindow.chatList;

                        if (_client.NewChat is not null)
                        {
                            var newListButton = new ListButton
                            {
                                Id = _client.NewChat.Id,
                                TitleText = _user.Login,
                                ProfilePictureBackground = PPBackground.Background,
                                ProfilePictureSource = PPHolder.Source
                            };

                            newListButton.Click += _clientWindow.chatList.ListButton_Click;

                            _clientWindow.chatList.chatList.Children.Add(newListButton);


                            if (_clientWindow.clientWindowChatFrameList is not null)
                            {
                                _clientWindow.clientWindowChatFrameList.Add(new ClientWindowChatFrame(_clientWindow, new Chat
                                {
                                    Id = _client.NewChat.Id,
                                    Chatusers = _client.NewChat.Chatusers,
                                }));

                                ContactUserBtn_Click(null, null);
                            }
                        }
                    }
                }
            }
        }
    }
}
