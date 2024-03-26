using System.IO;
using System.Windows;
using static LogInPage.MainWindow;
using System.Xml.Serialization;

namespace LogInPage
{
    /// <summary>
    /// Логика взаимодействия для ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private readonly Client client;
        public ChangePasswordWindow(Client client)
        {
            InitializeComponent();

            ContBtnLeft.Content = "Yes";
            ContBtnRight.Content = "No";
            this.client = client;
        }

        private void ContBtnLeft_Click(object sender, RoutedEventArgs e)
        {
            if (ContBtnLeft.Content is string str && str.Equals("Yes"))
            {
                ContBtnLeft.Content = "Continue";
                ContBtnRight.Content = "Cancel";

                frame.Content = new change_password_window_fields(client);
            }
            else 
            {
                if (frame.Content is change_password_window_fields cpwf) 
                {
                    cpwf.ChangePassword();

                    if (client is not null && client.CurrentUser is not null && client.StayInClient)
                    {
                        XmlSerializer x = new(typeof(UserLock));

                        using TextWriter writer = new StreamWriter("userlock.xml");
                        x.Serialize(writer, new UserLock
                        {
                            Login = client.CurrentUser.Login,
                            Password = client.CurrentUser.Password
                        });
                    }

                    this.Close();
                }
            }
        }

        private void ContBtnRight_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Polygon_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
