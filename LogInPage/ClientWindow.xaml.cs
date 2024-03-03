using System.Windows;
using System.Windows.Input;

namespace LogInPage
{
    /// <summary>
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        public client_window_nothing_frame clientWindowNothingFrame;
        public ClientWindowChatFrame clientWindowChatFrame;
        public ClientWindow()
        {
            InitializeComponent();

            Client.ClientWindowProperty = this;
            clientWindowNothingFrame = new();
            clientWindowChatFrame = new(this);
        }

        public void UploadMessage(string dt, string user_name, string message, string type)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                clientWindowChatFrame.UploadMessage(dt, user_name, message, type);
            }));
        }

        private void DragAndMove(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                Point mouseDownPoint = e.GetPosition(this);
                Left = mouseDownPoint.X - Width / 2;
                Top = mouseDownPoint.Y - ToolBar.Height.Value / 2;
            }
            DragMove();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MaximizeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal) WindowState = WindowState.Maximized;
            else WindowState = WindowState.Normal;
        }

        private void MinimizedBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MainChat_Click(object sender, RoutedEventArgs e)
        {
            ChatFrame.Content = clientWindowChatFrame;
        }
    }
}
