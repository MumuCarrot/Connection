﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LogInPage
{
    /// <summary>
    /// Логика взаимодейтвия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        public readonly Client client;
        public readonly List<ClientWindowChatFrame>? clientWindowChatFrameList = [];
        public ClientWindowChatFrame? CurrentChat { get; set; }
        public readonly user_list userList;
        public List<User> userSearchResult = [];

        private readonly client_window_nothing_frame? clientWindowNothingFrame;
        private readonly ClientWindowSettingsFrame? clientWindowSettingsFrame;
        private readonly chat_list chatList;

        public ClientWindow(MainWindow mainWindow)
        {
            InitializeComponent();

            client = mainWindow.client;
            clientWindowNothingFrame = new();
            clientWindowSettingsFrame = new(this);
            client.CurrenWindow = this;
            FrameList.Content = chatList = new chat_list(this);
            userList = new(this);

            if (client.UserChatIds is not null) 
            {
                foreach (var id in client.UserChatIds) 
                { 
                    clientWindowChatFrameList.Add(new(client, id));
                }
            }
        }

        public void UploadMessage(Message message, bool? isMy)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                CurrentChat?.UploadMessage(message, isMy);
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

        public void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            client.Close();
            Close();
        }

        private void MaximizeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                MainGrid.Margin = new(7, 7, 7, 47);
            }
            else
            {
                WindowState = WindowState.Normal;
                MainGrid.Margin = new(0);
            }
        }

        private void MinimizedBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
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

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            if (ChatFrame.Content != clientWindowSettingsFrame)
            {
                ChatFrame.Content = clientWindowSettingsFrame;
            }
            else
            {
                ChatFrame.Content = clientWindowNothingFrame;
            }
        }

        private void SerchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SerchTextBox.Text.Length > 0)
            {
                client.GetRequestUsersByLogin(SerchTextBox.Text);
                FrameList.Content = userList;
            }
            else 
            {
                FrameList.Content = chatList;
            }
        }
    }
}
