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

            if (clientWindow.client.UserChatIds is not null) 
            {
                foreach (var id in clientWindow.client.UserChatIds) 
                {
                    var newListButton = new ListButton
                    {
                        Id = id,
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
