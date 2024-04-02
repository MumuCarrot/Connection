namespace LogInPage
{
    public partial class Client
    {
        private void PostResponce(string responce)
        {
            int methodIndex = responce.IndexOf(' ');
            if (methodIndex == -1) throw new Exception("Get method was not found.");

            string method = responce[..methodIndex];
            switch (method)
            {
                case "--MSG":
                    PostMessage(responce);
                    break; // MSG
            }
        }

        private void PostMessage(string responce)
        {
            Message? message = JsonExtractor<Message>(responce, "json", right: 1);

            if (message is not null && CurrenWindow is not null && CurrenWindow is ClientWindow cw)
            {
                cw.UploadMessage(message, message.Username?.Equals(CurrentUser?.Login));
            }
        }
    }
}
