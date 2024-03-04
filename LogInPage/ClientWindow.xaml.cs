using System.Windows;
using System.Windows.Input;

namespace LogInPage
{
    /// <summary>
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        public readonly static Client client;
        private static readonly client_window_nothing_frame? clientWindowNothingFrame;
        private static readonly ClientWindowChatFrame? clientWindowChatFrame;

        public ClientWindow()
        {
            InitializeComponent();
        }

        static ClientWindow()
        {
            client = MainWindow.client;
            clientWindowNothingFrame = new();
            clientWindowChatFrame = new();
        }

        public static void UploadMessage(string dt, string user_name, string message, string type)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                clientWindowChatFrame?.UploadMessage(dt, user_name, message, type);
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
            Client.Close();
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

        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    ChatFrame.Content = clientWindowNothingFrame;
                    break;
            }
        }
    }
}
