using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        }

        private void Send_Click(object? sender, RoutedEventArgs? e)
        {
            string dateTime = DateTime.Now.ToString();
            string userName = Client.Login;
            string message = MessageTextBox.Text;
            string messageType = "text";
            UploadMessage(dateTime, userName, message, messageType);
            Client.Message(message, "text");

            MessageTextBox.Text = string.Empty;
        }

        public void UploadMessage(string dateTime, string userName, string message, string type)
        {
            TextBlock tb = new()
            {
                Text = "[ " + dateTime + " ] " + userName + " : " + message
            };
            Table.Children.Add(tb);
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
