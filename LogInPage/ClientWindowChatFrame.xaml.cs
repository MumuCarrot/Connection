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
        /// Is content loaded
        /// </summary>
        public bool IsContentLoaded { get; set; } = false;
        /// <summary>
        /// Client window
        /// </summary>
        private readonly ClientWindow clientWindow;
        /// <summary>
        /// Current client
        /// </summary>
        private readonly Client client;
        /// <summary>
        /// Current chat
        /// </summary>
        public readonly Chat chat;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="clientWindow">
        /// Client Window
        /// </param>
        /// <param name="chatId">
        /// Chat id
        /// </param>
        public ClientWindowChatFrame(ClientWindow clientWindow, Chat chat)
        {
            InitializeComponent();

            MessageTextBox.Focus();
            scrollViewer.ScrollToEnd();

            this.clientWindow = clientWindow;
            client = clientWindow.client;
            this.chat = chat;
        }

        /// <summary>
        /// Set information about chat and create rules
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            clientWindow.CurrentChat = this;
            clientWindow.UpdateChat();
            clientWindow.CurrentChat.IsContentLoaded = true;

            string chatName = string.Empty;

            if (chat.Chatusers is not null) 
            {
                foreach (var i in chat.Chatusers) 
                {
                    if (client.CurrentUser is not null && !i.Equals(client.CurrentUser.Login)) 
                    { 
                        chatName = i.ToString();
                        break;
                    }
                }
            }

            ChatName.Text = chatName;
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

            client.PostRequestMessage(message, chat.Id ?? "undefined");

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
