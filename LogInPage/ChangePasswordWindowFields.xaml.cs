using System.Windows.Controls;

namespace LogInPage
{
    /// <summary>
    /// Change password window field
    /// </summary>
    public partial class ChangePasswordWindowFields : Page
    {
        /// <summary>
        /// Current client
        /// </summary>
        private readonly Client client;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="client">
        /// Current client
        /// </param>
        public ChangePasswordWindowFields(Client client)
        {
            InitializeComponent();
            this.client = client;
        }

        /// <summary>
        /// Changes password if fields are changed
        /// </summary>
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
