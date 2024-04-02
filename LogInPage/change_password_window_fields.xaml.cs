using System.Windows.Controls;

namespace LogInPage
{
    /// <summary>
    /// Логика взаимодействия для change_password_window_fields.xaml
    /// </summary>
    public partial class change_password_window_fields : Page
    {
        Client client;
        public change_password_window_fields(Client client)
        {
            InitializeComponent();
            this.client = client;
        }

        public void ChangePassword() 
        {
            if (CurrentPassword.Password == client.CurrentUser?.Password && 
                NewPassword.Password == RepeatedNewPassword.Password && 
                NewPassword.Password.Length >= 5 && 
                NewPassword.Password != CurrentPassword.Password) 
            { 
                client.CurrentUser.Password = NewPassword.Password;
                client.PatchRequestPassword(NewPassword.Password);
            }
        }
    }
}
