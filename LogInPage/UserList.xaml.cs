using System.Windows;
using System.Windows.Controls;
using Connect.user;

namespace LogInPage
{
    /// <summary>
    /// Class of user list
    /// </summary>
    public partial class UserList : Page
    {
        /// <summary>
        /// Current client window
        /// </summary>
        ClientWindow clientWindow;
        
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="cw">
        /// Client window
        /// </param>
        public UserList(ClientWindow cw)
        {
            clientWindow = cw;

            InitializeComponent();
        }

        /// <summary>
        /// Add ui element to user list
        /// </summary>
        /// <param name="obj"></param>
        public void Add(UIElement obj)
        {
            try
            {
                if (obj is ListButton lb)
                {
                    lb.Click += Button_CLick;
                    userList.Children.Add(obj);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Click
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Element
        /// </param>
        private void Button_CLick(object? sender, EventArgs e)
        {
            if (sender is ListButton lb)
            {
                User? user = null;
                foreach (var u in clientWindow.userSearchResult)
                {
                    if (u.Login.Equals(lb.TitleText))
                    {
                        user = u;
                        break;
                    }
                }

                if (user is not null)
                {
                    clientWindow.ChatFrame.Content = new UserBioPage(user, clientWindow);
                }
            }
        }
    }
}
