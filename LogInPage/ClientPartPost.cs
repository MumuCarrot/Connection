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
            MessageConteiner? messageList = JsonExtractor<MessageConteiner>(responce, "json");

            if (messageList is not null && CurrenWindow is not null && CurrenWindow is ClientWindow cw)
            {
                foreach (var message in messageList.Messages)
                {
                    cw.UploadMessage(message, message.Login.Equals(CurrentUser?.Login));
                }
            }
        }
    }
}
