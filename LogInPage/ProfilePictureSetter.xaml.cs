using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace LogInPage
{
    public partial class ProfilePictureSetter : Window
    {
        private ClientWindow clientWindow;
        public ProfilePictureSetter(ClientWindow clientWindow)
        {
            InitializeComponent();

            ColorTB.IsChecked = true;
            this.clientWindow = clientWindow;
        }

        private void TB_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton tb)
            {
                if (tb.Name == "ColorTB")
                {
                    PictureTB.IsChecked = false;

                    ScrollFrame.Content = new PPS_ScrollPageBackground(this);
                }
                else
                {
                    ColorTB.IsChecked = false;

                    ScrollFrame.Content = new PPS_ScrollPagePicture(this);
                }
            }
        }

        private void DeclineBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (clientWindow.client.CurrentUser is not null) 
            { 
                clientWindow.client.CurrentUser.UserProfilePicture.PPColor = PPBackground.Background.ToString();
                clientWindow.client.CurrentUser.UserProfilePicture.PictureName = ProfilePicture.ToString(PPHolder.Source);

                if (clientWindow.clientWindowSettingsFrame is not null)
                { 
                    clientWindow.clientWindowSettingsFrame.PPBackground = PPBackground.Background;
                    clientWindow.clientWindowSettingsFrame.PPPicture = clientWindow.client.CurrentUser.UserProfilePicture;
                }

                clientWindow.client.PatchProfilePicture();

                this.Close();
            }
        }
    }
}
