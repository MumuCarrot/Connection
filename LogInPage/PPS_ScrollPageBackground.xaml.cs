using System.Windows;
using System.Windows.Controls;

namespace LogInPage
{
    /// <summary>
    /// Class of profile picture background setter
    /// </summary>
    public partial class PPS_ScrollPageBackground : Page
    {
        /// <summary>
        /// Profile picture setter window
        /// </summary>
        ProfilePictureSetter profilePictureSetter;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="pps">
        /// Profile picture setter window
        /// </param>
        public PPS_ScrollPageBackground(ProfilePictureSetter pps)
        {
            InitializeComponent();

            profilePictureSetter = pps;
        }

        /// <summary>
        /// Color button click
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn) 
            {
                profilePictureSetter.PPBackground.Background = btn.Background;
            }
        }
    }
}
