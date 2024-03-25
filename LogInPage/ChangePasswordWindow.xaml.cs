using System.Windows;

namespace LogInPage
{
    /// <summary>
    /// Логика взаимодействия для ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        Client client;
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
