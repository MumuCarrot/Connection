using Connect.message;
using Connect.profilePicture;
using Connect.user;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace LogInPage
{
    public partial class Client
    {
        /// <summary>
        /// GET switch responce
        /// </summary>
        /// <param name="responce">
        /// Responce from the server
        /// </param>
        /// <exception cref="Exception">
        /// Server is not responding.
        /// </exception>
        private void GetResponce(string responce)
        {
            int methodIndex = responce.IndexOf(' ');
            if (methodIndex == -1) throw new Exception("Get method was not found.");

            string method = responce[..methodIndex];
            switch (method)
            {
                case "--LOG-IN":
                    this.GetResponceUserCheck(responce);
                    break; // --LOG-IN
                case "--CHAT-HISTORY":
                    this.GetResponceUpdateChat(responce);
                    break; // --CHAT-HISTORY
                case "--USER-LIST":
                    this.GetResponceUserByLogin(responce);
                    break; // --USER-LIST
                case "--CHAT-LIST":
                    this.GetResponceUpdateChatList(responce);
                    break; // --CHAT-LIST
                case "--CHAT-PICTURE":
                    this.GetResponceUpdateChatPicture(responce);
                    break; // --CHAT-PICTURE
            }
        }
        /// <summary>
        /// Check for user by login and password
        /// </summary>
        /// <param name="responce">
        /// Responce from the server
        /// </param>
        private void GetResponceUserCheck(string responce)
        {
            CurrentUser = JsonExtractor<User>(responce, "json", right: 2);
            if (!Responce.Contains("FALSE"))
            {
                ServerConfirmation = true;
            }
        }
        /// <summary>
        /// Update chat
        /// </summary>
        /// <param name="responce">
        /// Responce from the server
        /// </param>
        private void GetResponceUpdateChat(string responce)
        {
            List<Chat>? chatList = null;

            bool readed = false;
            int shift = 4;
            while (!readed)
            {
                try
                {
                    chatList = JsonExtractor<List<Chat>>(responce, "json", right: shift);
                    readed = true;
                }
                catch
                {
                    if (shift == 0)
                    {
                        throw new Exception("Message can't be readed.");
                    }
                    else shift -= 1;
                }
            }

            if (chatList is not null && CurrenWindow is not null && CurrenWindow is ClientWindow cw)
            {
                foreach (var chat in chatList)
                {
                    if (chat.Messages is not null)
                    {
                        foreach (var message in chat.Messages)
                        {
                            cw.UploadMessage(message, message.Username?.Equals(CurrentUser?.Login));
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Check for users by login
        /// </summary>
        /// <param name="responce">
        /// Responce from the server
        /// </param>
        private void GetResponceUserByLogin(string responce)
        {
            try
            {
                UserPackage? userPackege = JsonExtractor<UserPackage>(responce, "json", right: 4);
                if (userPackege is not null && CurrenWindow is not null && CurrenWindow is ClientWindow cw)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        cw.userSearchResult = userPackege.users;
                        foreach (var user in userPackege.users)
                        {
                            cw.userList.Add(new ListButton(user));
                        }
                    }));
                }
            }
            catch
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (CurrenWindow is not null && CurrenWindow is ClientWindow cw)
                    {
                        cw.userList = new UserList(cw);
                    }
                }));
            }
        }
        /// <summary>
        /// Update chat list
        /// </summary>
        /// <param name="responce">
        /// Responce from the server
        /// </param>
        private void GetResponceUpdateChatList(string responce)
        {
            List<Chat>? chat;
            try
            {
                chat = JsonExtractor<List<Chat>>(responce, "json", right: 4);
            }
            catch
            {
                chat = JsonExtractor<List<Chat>>(responce, "json", right: 2);
            }

            UserChatPreload = chat;

            if (chat is not null)
            {
                if (UserChatPreloadSaveOutOfChanges is not null && UserChatPreloadSaveOutOfChanges.Count > 0)
                {
                    foreach (var c in chat)
                    {
                        NewChat = c;
                    }
                }
            }

            PreloadChatIsReady = true;
        }

        private void GetResponceUpdateChatPicture(string responce)
        {
            Dictionary<string, (string, string)>? answer;
            try
            {
                answer = JsonExtractor<Dictionary<string, (string, string)>>(responce, "json", right: 2);
            }
            catch
            {
                answer = JsonExtractor<Dictionary<string, (string, string)>>(responce, "json", right: 0);
            }

            if (answer is not null)
            {
                if (CurrenWindow is ClientWindow clientWindow && clientWindow is not null)
                {
                    if (answer.Count > 0)
                    {
                        var d = Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            foreach (var a in answer)
                            {
                                if (clientWindow.chatList.ContainsId(a.Key))
                                {
                                    if (clientWindow.chatList[a.Key] is ListButton lb)
                                    {
                                        lb.ProfilePictureSource = a.Value.Item1.ToSource(ProfilePictureSize.i64px);
                                        lb.ProfilePictureBackground = new BrushConverter().ConvertFrom(a.Value.Item2) as SolidColorBrush ?? new SolidColorBrush(Color.FromArgb(255, 220, 241, 255));
                                    }
                                }
                            }
                        }));
                    }
                }
            }
        }
    }
}
