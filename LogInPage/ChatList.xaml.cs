using System.Windows;
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
                    this.Add(newListButton);
                }

                List<ListButton> list = [];

                foreach (var i in chatList.Children)
                {
                    if (i is ListButton listButton)
                    {
                        if (listButton.ProfilePictureSource.ToString().Contains("default"))
                        {
                            list.Add(listButton);
                        }
                    }
                }

                clientWindow.client.GetRequestUpdateChatPictures(list);
            }
        }

        public void Add(UIElement uIElement)
        {
            try
            {
                if (uIElement is ListButton lb)
                {
                    chatList.Children.Add(uIElement);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool ContainsId(string id)
        {
            foreach (var i in chatList.Children)
            {
                if (i is ListButton lb)
                {
                    if (lb.Id == id)
                    {
                        return true;
                    }
                }
            }
            return false;
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
        public void ListButton_Click(object? sender, EventArgs e)
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

        public UIElement this[string id]
        {
            get
            {
                foreach (var ch in chatList.Children)
                {
                    if (ch is ListButton lb)
                    {
                        if (lb.Id is not null && lb.Id == id) return lb;
                    }
                }
                throw new Exception("Index does not exist.");
            }
        }
    }
}
