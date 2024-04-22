using System.Windows.Controls;

namespace LogInPage
{
    /// <summary>
    /// Chat list
    /// </summary>
    public partial class ChatList : Page
    {
        /// <summary>
        /// Current client window
        /// </summary>
        private readonly ClientWindow clientWindow;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="cw">
        /// Current client window
        /// </param>
        public ChatList(ClientWindow cw)
        {
            InitializeComponent();

            clientWindow = cw;

            if (clientWindow.client.UserChatPreload is not null) 
            {
                foreach (var chat in clientWindow.client.UserChatPreload) 
                {
                    string title = string.Empty;
                    if (chat.Chatusers is not null) 
                    { 
                        foreach (var j in chat.Chatusers) 
                        {
                            if (!j.Equals(clientWindow.client.CurrentUser?.Login)) title += j;
                        }
                    }

                    var newListButton = new ListButton
                    {
                        Id = chat.Id,
                        TitleText = title,
                        UnderlineText = chat.Messages?[0].Content?.Text ?? ""
                    };
                    newListButton.Click += ListButton_Click;
                    chatList.Children.Add(newListButton);
                }
            }
        }

        /// <summary>
        /// Button click route event
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
        private void ListButton_Click(object? sender, EventArgs e)
        {
            if (clientWindow.clientWindowChatFrameList is not null && sender is ListButton lb && lb.Id is not null) 
            {
                foreach (var ch in clientWindow.clientWindowChatFrameList) 
                { 
                    if (ch.chat.Id is not null && ch.chat.Id.Equals(lb.Id)) 
                    {
                        clientWindow.ChatFrame.Content = ch;
                        break;
                    }
                }
            }
        }
    }
}
