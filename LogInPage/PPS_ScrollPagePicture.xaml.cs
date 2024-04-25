using System.Windows;
using System.Windows.Controls;

namespace LogInPage
{
    /// <summary>
    /// Class of profile picture image setter
    /// </summary>
    public partial class PPS_ScrollPagePicture : Page
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
        public PPS_ScrollPagePicture(ProfilePictureSetter pps) 
        {
            InitializeComponent();

            profilePictureSetter = pps;
        }

        /// <summary>
        /// Image button click
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
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
