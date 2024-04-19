using System.Windows;
using System.Windows.Controls;

namespace LogInPage
{
    public partial class user_list : Page
    {
        ClientWindow clientWindow;
        public user_list(ClientWindow cw)
        {
            clientWindow = cw;

            InitializeComponent();
        }

        public void Add(UIElement obj)
        {
            try
            {
                if (obj is ListButton lb)
                {
                    lb.Click += Button_CLick;
                    UserList.Children.Add(obj);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

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
                    clientWindow.ChatFrame.Content = new userBioPage(user);
                }
            }
        }
    }
}
