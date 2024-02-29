using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LogInPage
{
    /// <summary>
    /// Логика взаимодействия для client_window_chat_frame.xaml
    /// </summary>
    public partial class client_window_chat_frame : Page
    {
        public string Message { get { return MessageTextBox.Text; } }
        public Client client;
        private readonly Action? EscapePage;
        public client_window_chat_frame(Client client, Action esc)
        {
            InitializeComponent();
            MessageTextBox.Focus();
            this.client = client;
            EscapePage = esc;
            client.UpdateChat(this);
        }

        private void Send_Click(object? sender, RoutedEventArgs? e)
        {
            UploadMessage(sender);
        }

        public void UploadMessage(object? sender, string? dt = null, string? client_name = null, string message = "", string? type = null)
        {
            if (sender is not null)
            {
                if (MessageTextBox.Text.Length > 0)
                {
                    TextBlock newTb = new()
                    {
                        Text = $"[{DateTime.Now}] " + client.Login + " : " + MessageTextBox.Text
                    };
                    Table.Children.Add(newTb);
                    client.Message(MessageTextBox.Text, "text");
                    MessageTextBox.Text = string.Empty;
                    MessageTextBox.Focus();
                }
            }
            else
            {
                if (message.Length > 0)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        TextBlock newTb = new()
                        {
                            Text = $"[{dt}] " + client_name + " : " + message
                        };
                        Table.Children.Add(newTb);
                    }));
                }
            }
        }

        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    Send_Click(null, null);
                    break;
                case Key.Escape:
                    if (EscapePage is not null) EscapePage();
                    break;
            }
        }
    }
}
