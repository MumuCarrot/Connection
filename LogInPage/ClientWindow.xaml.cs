﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Connect.user;
using Connect.message;

namespace LogInPage
{
    /// <summary>
    /// Client window is a realisation of messanger window
    /// </summary>
    public partial class ClientWindow : Window
    {
        /// <summary>
        /// Current client
        /// </summary>
        public readonly Client client;

        /// <summary>
        /// Client window hello page
        /// </summary>
        private readonly ClientWindowNothingFrame? clientWindowNothingFrame;

        /// <summary>
        /// Chat's of current user
        /// </summary>
        public readonly List<ClientWindowChatFrame>? clientWindowChatFrameList = [];

        /// <summary>
        /// User settings page
        /// </summary>
        public readonly ClientWindowSettingsFrame? clientWindowSettingsFrame;

        /// <summary>
        /// Current open chat
        /// </summary>
        public ClientWindowChatFrame? CurrentChat { get; set; }

        /// <summary>
        /// Found users
        /// </summary>
        public List<User> userSearchResult = [];

        /// <summary>
        /// List of chat buttons
        /// </summary>
        public readonly ChatList chatList;

        /// <summary>
        /// List of user buttons
        /// </summary>
        public UserList userList;

        /// <summary>
        /// SearchTextBox alias
        /// </summary>
        public string? SearchBox { get { return SerchTextBox.Text; } set { SerchTextBox.Text = value; } }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="mainWindow">
        /// Login page
        /// </param>
        public ClientWindow(MainWindow mainWindow)
        {
            InitializeComponent();

            client = mainWindow.client;
            clientWindowNothingFrame = new();
            clientWindowSettingsFrame = new(this);
            client.CurrenWindow = this;
            FrameList.Content = chatList = new ChatList(this);
            userList = new(this);

            if (client.UserChatPreload is not null)
            {
                foreach (var chat in client.UserChatPreload)
                {
                    clientWindowChatFrameList.Add(new(this, chat));
                }
            }
        }

        /// <summary>
        /// Sends a request to upload messages in current chat
        /// </summary>
        /// <param name="toUpdate">
        /// Number of messages
        /// </param>
        public void UpdateChat(int toUpdate = 50)
        {
            if (CurrentChat is not null)
            {
                if (!CurrentChat.IsContentLoaded)
                {
                    client.GetRequestUpdateChat(CurrentChat?.chat.Id ?? "null", toUpdate);
                }
            }
        }
        /// <summary>
        /// Upload messages into current chat
        /// </summary>
        /// <param name="message">
        /// Message
        /// </param>
        /// <param name="isMy">
        /// Was it messages of current user or not
        /// </param>
        public void UploadMessage(Message message, bool? isMy)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                CurrentChat?.UploadMessage(message, isMy);
            }));
        }
        /// <summary>
        /// Drag and move window
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
        private void DragAndMove(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                Point mouseDownPoint = e.GetPosition(this);
                Left = mouseDownPoint.X - Width / 2;
                Top = mouseDownPoint.Y - ToolBar.Height.Value / 2;
            }
            this.DragMove();
        }
        /// <summary>
        /// Close window
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
        public void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            client.Close();
            this.Close();
        }
        /// <summary>
        /// Maximize window
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
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
        /// <summary>
        /// Minimize button
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
        private void MinimizedBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        /// <summary>
        /// Escape keybinding to left chat/settings
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    ChatFrame.Content = clientWindowNothingFrame;
                    break;
            }
        }
        /// <summary>
        /// Opens settings page
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
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
        /// <summary>
        /// Search event
        /// </summary>
        /// <param name="sender">
        /// Sender
        /// </param>
        /// <param name="e">
        /// Event
        /// </param>
        private void SerchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                userList = new UserList(this);
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
            catch (Exception ex)
            {
                MessageBox.Show("ClientWindow 1. " + ex.Message);
            }
        }
    }
}
