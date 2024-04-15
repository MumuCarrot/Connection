using System.Windows.Controls;

namespace LogInPage
{
    public partial class chat_list : Page
    {
        private ClientWindow clientWindow;
        public chat_list(ClientWindow cw)
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
                    ChatList.Children.Add(newListButton);
                }
            }
        }

        private void ListButton_Click(object? sender, EventArgs e)
        {
            if (clientWindow.clientWindowChatFrameList is not null && sender is ListButton lb && lb.Id is not null) 
            {
                foreach (var ch in clientWindow.clientWindowChatFrameList) 
                { 
                    if (ch.ChatId.Equals(lb.Id)) 
                    {
                        clientWindow.ChatFrame.Content = clientWindow.CurrentChat = ch;
                        clientWindow.UpdateChat();
                        clientWindow.CurrentChat.IsContentLoaded = true;
                        break;
                    }
                }
            }
        }
    }
}
