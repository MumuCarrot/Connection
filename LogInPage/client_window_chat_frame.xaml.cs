using System.Globalization;
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
        private readonly Client client;

        public ClientWindowChatFrame(Client client)
        {
            InitializeComponent();
            MessageTextBox.Focus();
            client.GetRequestUpdateChat();
            scrollViewer.ScrollToEnd();

            this.client = client;
        }

        private void Send_Click(object? sender, RoutedEventArgs? e)
        {
            Message message = new()
            {
                Time = DateTime.Now,
                Username = client.CurrentUser?.Login ?? "user not found",
                Content = new ContentMessage()
                { 
                    Text = MessageTextBox.Text,
                    Image = ""
                }
            };

            UploadMessage(message, true);

            client.PostRequestMessage(message);

            MessageTextBox.Text = string.Empty;

            scrollViewer.ScrollToEnd();
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

                if (sender is TextBox textBox && textBox is not null)
                {
                    var formattedText = new FormattedText(
                        textBox.Text,
                        CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface(textBox.FontFamily, textBox.FontStyle, textBox.FontWeight, textBox.FontStretch),
                        textBox.FontSize,
                        Brushes.Black,
                        new NumberSubstitution(),
                        VisualTreeHelper.GetDpi(textBox).PixelsPerDip);

                    int rowCount = 0;
                    if (textBox.LineCount > 6)
                    {
                        rowCount = 6;
                    }
                    else 
                    { 
                        rowCount = textBox.LineCount;
                    }

                    textBox.Height = formattedText.Height * rowCount + 5;
                }
            }
            else
            {
                mySolidColorBrush.Color = Color.FromRgb(162, 220, 255);

                if (sender is TextBox textBox && textBox is not null) 
                {
                    textBox.Height = 21;
                }
            }
            SendMsg.Background = mySolidColorBrush;
        }
    }
}
