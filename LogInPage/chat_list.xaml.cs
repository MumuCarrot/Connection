using System.Windows;
using System.Windows.Controls;

namespace LogInPage
{
    /// <summary>
    /// Логика взаимодействия для chat_list.xaml
    /// </summary>
    public partial class chat_list : Page
    {
        private ClientWindow clientWindow;
        public chat_list(ClientWindow cw)
        {
            InitializeComponent();

            clientWindow = cw;
        }

        private void ListButton_Click(object sender, EventArgs e)
        {
            clientWindow.ChatFrame.Content = clientWindow.clientWindowChatFrame;
        }
    }
}
