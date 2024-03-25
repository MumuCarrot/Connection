using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LogInPage
{
    /// <summary>
    /// Логика взаимодействия для client_window_chat_frame.xaml
    /// </summary>
    public partial class ClientWindowChatFrame : Page
    {
        public bool IsEmpty { get { return MessageTextBox.Text.Length == 0; } }
        private readonly SolidColorBrush mySolidColorBrush = new();
        Client client;

        public ClientWindowChatFrame(Client client)
        {
            InitializeComponent();
            MessageTextBox.Focus();
            client.UpdateChat();
            scrollViewer.ScrollToEnd();

            this.client = client;
        }

        private void Send_Click(object? sender, RoutedEventArgs? e)
        {
            UploadMessage(new Message {
                MessageDateTime = DateTime.Now.ToString(),
                Login = client.CurrentUser?.Login ?? "user not found",
                Content = MessageTextBox.Text,
                MessageType = "text"
            }, true);
            client.Message(MessageTextBox.Text, "text");

            MessageTextBox.Text = string.Empty;
        }

        public void UploadMessage(Message message, bool? isMy)
        {
            MessageFrame msg = new(message, isMy);
            
            Table.Children.Add(msg);
        }

        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!IsEmpty) 
            { 
                switch (e.Key)
                {
                    case Key.Enter:
                        Send_Click(null, null);
                        break;
                }
            }
        }

        private void MessageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsEmpty)
            {
                mySolidColorBrush.Color = Color.FromRgb(68, 181, 249);
            }
            else
            {
                mySolidColorBrush.Color = Color.FromRgb(162, 220, 255);
            }
            SendMsg.Background = mySolidColorBrush;
        }
    }
}
