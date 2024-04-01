using System.Windows.Controls;

namespace LogInPage
{
    public partial class user_bio_page : Page
    {
        public user_bio_page()
        {
            InitializeComponent();
        }

        public string Username { set { UserName.Text = value; } }

        public string Login { set { UserLogin.Text = value; } }

        public string Bio { set { AboutMe.Text = value; } }
    }
}
