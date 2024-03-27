using System.Windows;
using System.Windows.Controls;

namespace LogInPage
{
    /// <summary>
    /// Логика взаимодействия для chat_list.xaml
    /// </summary>
    public partial class chat_list : Page
    {
        public chat_list(ClientWindow cw)
        {
            InitializeComponent();

            ChatList.Children.Add(new ListButton(cw, "Main chat", "Тут общаются пока только сосиска и сочная."));
        }
    }
}
