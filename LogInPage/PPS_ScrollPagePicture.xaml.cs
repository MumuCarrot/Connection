using System.Windows;
using System.Windows.Controls;

namespace LogInPage
{
    public partial class PPS_ScrollPagePicture : Page
    {
        ProfilePictureSetter profilePictureSetter;

        public PPS_ScrollPagePicture(ProfilePictureSetter pps) 
        {
            InitializeComponent();

            profilePictureSetter = pps;
        }

        private void PictureButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && profilePictureSetter is not null)
            {
                if (btn.Content is Image img) 
                { 
                    profilePictureSetter.PPHolder.Source = img.Source;
                }
            }
        }
    }
}
