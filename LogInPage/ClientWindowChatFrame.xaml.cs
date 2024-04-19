using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LogInPage
{
    /// <summary>
    /// Client window chat frame
    /// </summary>
    public partial class ClientWindowChatFrame : Page
    {
        /// <summary>
        /// If chat frame empty
        /// </summary>
        public bool IsEmpty { get { return MessageTextBox.Text.Length == 0; } }
        /// <summary>
        /// Chat id from MongoDB
        /// </summary>
        public string ChatId { get; private set; }
        /// <summary>
        /// Is content loaded
        /// </summary>
        public bool IsContentLoaded { get; set; } = false;
        /// <summary>
        /// Current client
        /// </summary>
        private readonly Client client;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="client">
        /// Current client
        /// </param>
        /// <param name="chatId">
        /// Chat id
        /// </param>
        public ClientWindowChatFrame(Client client, string chatId)
        {
            InitializeComponent();

            MessageTextBox.Focus();
            scrollViewer.ScrollToEnd();

            this.client = client;
            this.ChatId = chatId;
        }

        /// <summary>
        /// Send button click
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
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

            client.PostRequestMessage(message, ChatId);

            MessageTextBox.Text = string.Empty;

            scrollViewer.ScrollToEnd();
        }
        /// <summary>
        /// Uploading message
        /// </summary>
        /// <param name="message">
        /// Message
        /// </param>
        /// <param name="isMy">
        /// Is my indicator
        /// </param>
        public void UploadMessage(Message message, bool? isMy)
        {
            MessageFrame msg = new(message, isMy);
            
            Table.Children.Add(msg);
        }
        /// <summary>
        /// Escape preview
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
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
        /// <summary>
        /// Message text changed
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
        private void MessageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SolidColorBrush mySolidColorBrush = new();
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

                    int rowCount;
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
