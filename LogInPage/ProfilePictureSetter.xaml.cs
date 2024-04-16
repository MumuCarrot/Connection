using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace LogInPage
{
    /// <summary>
    /// Логика взаимодействия для ProfilePictureSetter.xaml
    /// </summary>
    public partial class ProfilePictureSetter : Window
    {
        public ProfilePictureSetter()
        {
            InitializeComponent();

            ColorTB.IsChecked = true;
        }

        private void TB_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton tb)
            {
                if (tb.Name == "ColorTB")
                {
                    PictureTB.IsChecked = false;
                }
                else
                {
                    ColorTB.IsChecked = false;
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
    }
}
