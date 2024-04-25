using System.Windows.Controls;

namespace LogInPage
{
    /// <summary>
    /// Sign up page
    /// </summary>
    public partial class SignUpPage : Page
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        public SignUpPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loaded
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LoginTextBox.Focus();
        }
    }
}
