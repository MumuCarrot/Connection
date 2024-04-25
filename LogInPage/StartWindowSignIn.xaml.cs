using System.Windows.Controls;

namespace LogInPage
{
    /// <summary>
    /// Sign in page
    /// </summary>
    public partial class SignInPage : Page
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        public SignInPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loaded event
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
