using System.Windows.Controls;

namespace LogInPage
{
    public partial class SignUpPage : Page
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LoginTextBox.Focus();
        }
    }
}
