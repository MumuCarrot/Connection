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
        public string Message { get { return MessageTextBox.Text; } }
        public Client client = MainWindow.client;
        private readonly ClientWindow mainWindow;

        public ClientWindowChatFrame(ClientWindow cw)
        {
            InitializeComponent();
            MessageTextBox.Focus();
            client.UpdateChat();
            mainWindow = cw;
        }

        private void Send_Click(object? sender, RoutedEventArgs? e)
        {
            string dateTime = DateTime.Now.ToString();
            string userName = client.Login;
            string message = MessageTextBox.Text;
            string messageType = "text";
            UploadMessage(dateTime, userName, message, messageType);
            client.Message(Message, "text");
        }

        public void UploadMessage(string dateTime, string userName, string message, string type)
        {
            TextBlock tb = new()
            {
                Text = "[ " + dateTime + " ] " + userName + " : " + message
            };
            Table.Children.Add(tb);
        }

        private void EscapePage()
        {
            mainWindow.ChatFrame.Content = mainWindow.clientWindowNothingFrame;
        }

        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    Send_Click(null, null);
                    break;
                case Key.Escape:
                    EscapePage();
                    break;
            }
        }
    }
}
