using System.Windows.Controls;

namespace LogInPage
{
    public partial class ListButton : UserControl
    {
        public event EventHandler? Click = null;
        
        public ListButton()
        {
            InitializeComponent();
        }

        public string TitleText
        {
            get { return Title.Text; }
            set { Title.Text = value; }
        }

        public string UnderlineText
        {
            get { return Underline.Text; }
            set { Underline.Text = value; }
        }

        public string? Id { get; set; } = null;

        private void UserControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Click?.Invoke(this, EventArgs.Empty);
        }
    }
}
