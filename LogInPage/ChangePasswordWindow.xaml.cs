using System.IO;
using System.Windows;
using System.Xml.Serialization;
using static LogInPage.MainWindow;

namespace LogInPage
{
    /// <summary>
    /// Change password window
    /// </summary>
    public partial class ChangePasswordWindow : Window
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
        public ChangePasswordWindow(Client client)
        {
            InitializeComponent();

            ContBtnLeft.Content = "Yes";
            ContBtnRight.Content = "No";
            this.client = client;
        }

        /// <summary>
        /// Button event
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
        private void ContBtnLeft_Click(object sender, RoutedEventArgs e)
        {
            if (ContBtnLeft.Content is string str && str.Equals("Yes"))
            {
                ContBtnLeft.Content = "Continue";
                ContBtnRight.Content = "Cancel";

                frame.Content = new ChangePasswordWindowFields(client);
            }
            else 
            {
                if (frame.Content is ChangePasswordWindowFields cpwf) 
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
        /// <summary>
        /// Close button
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
        private void ContBtnRight_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Drag and move
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
