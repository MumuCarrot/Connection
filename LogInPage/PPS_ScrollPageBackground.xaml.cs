using System.Windows;
using System.Windows.Controls;

namespace LogInPage
{
    public partial class PPS_ScrollPageBackground : Page
    {
        ProfilePictureSetter profilePictureSetter;
        public PPS_ScrollPageBackground(ProfilePictureSetter pps)
        {
            InitializeComponent();

            profilePictureSetter = pps;
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn) 
            {
                profilePictureSetter.PPBackground.Background = btn.Background;
            }
        }
    }
}
