using System.Windows;
using System.Windows.Controls;

namespace LogInPage
{
    /// <summary>
    /// Логика взаимодействия для user_list.xaml
    /// </summary>
    public partial class user_list : Page
    {
        public user_list()
        {
            InitializeComponent();
        }

        public void Add(UIElement obj)
        {
            try
            {
                UserList.Children.Add(obj);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
