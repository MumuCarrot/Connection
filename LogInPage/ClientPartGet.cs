using System.Windows;

namespace LogInPage
{
    public partial class Client
    {
        private void GetResponce(string responce)
        {
            int methodIndex = responce.IndexOf(' ');
            if (methodIndex == -1) throw new Exception("Get method was not found.");

            string method = responce[..methodIndex];
            switch (method)
            {
                case "--USER_CHECK":
                    this.GetUserCheck(responce);
                    break; // --USER_CHECK
                case "--ACMSG":
                    this.GetUpdateChat(responce);
                    break; // --ACMSG
                case "--UBYLOG":
                    this.GetUserByLogin(responce);
                    break; // --UBYLOG
            }
        }

        private void GetUserCheck(string responce)
        {
            User? user = JsonExtractor<User>(responce, "json", right:1);
            if (user is not null)
            {
                CurrentUser = user;
            }
        }

        private void GetUpdateChat(string responce)
        {
            MessageConteiner? messageList = JsonExtractor<MessageConteiner>(responce, "json", right:3);

            if (messageList is not null && CurrenWindow is not null && CurrenWindow is ClientWindow cw)
            {
                foreach (var message in messageList.Messages)
                {
                    cw.UploadMessage(message, message.Login.Equals(CurrentUser?.Login));
                }
            }
        }

        private void GetUserByLogin(string responce)
        {
            UserPackege? userPackege = JsonExtractor<UserPackege>(responce, "json", right:3);

            if (userPackege is not null && CurrenWindow is not null && CurrenWindow is ClientWindow cw)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    cw.userSearchResult = userPackege.users;
                    foreach (var user in userPackege.users) 
                    {
                        cw.userList.Add(new ListButton 
                        { 
                            TitleText = user.Login,
                            UnderlineText = user.AboutMe
                        });
                    }
                })); 
            }
        }
    }
}
