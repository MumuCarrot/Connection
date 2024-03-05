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
                Login = Client.Login,
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
            switch (e.Key)
            {
                case Key.Enter:
                    Send_Click(null, null);
                    break;
            }
        }
    }
}
