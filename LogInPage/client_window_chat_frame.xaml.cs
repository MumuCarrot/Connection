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

        public ClientWindowChatFrame()
        {
            InitializeComponent();
            MessageTextBox.Focus();
            Client.UpdateChat();
            scrollViewer.ScrollToEnd();
        }

        private void Send_Click(object? sender, RoutedEventArgs? e)
        {
            UploadMessage(new Message {
                MessageDateTime = DateTime.Now.ToString(),
                Login = Client.CurrentUser?.Login ?? "user not found",
                Content = MessageTextBox.Text,
                MessageType = "text"
            });
            Client.Message(MessageTextBox.Text, "text");

            MessageTextBox.Text = string.Empty;
        }

        public void UploadMessage(Message message)
        {
            MessageFrame msg = new(message);
            
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
