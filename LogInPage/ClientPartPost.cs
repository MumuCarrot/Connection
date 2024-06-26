﻿using Connect.message;

namespace LogInPage
{
    public partial class Client
    {
        /// <summary>
        /// Post switch responce
        /// </summary>
        /// <param name="responce">
        /// Responce from the server
        /// </param>
        /// <exception cref="Exception">
        /// Server is not responding.
        /// </exception>
        private void PostResponce(string responce)
        {
            int methodIndex = responce.IndexOf(' ');
            if (methodIndex == -1) throw new Exception("Get method was not found.");

            string method = responce[..methodIndex];
            switch (method)
            {
                case "--MSG":
                    PostResponceMessage(responce);
                    break; // MSG
                case "--CHAT":
                    PostResponceChat(responce);
                    break; // MSG
            }
        }

        /// <summary>
        /// Recive message from other user
        /// </summary>
        /// <param name="responce">
        /// Responce from the server
        /// </param>
        private void PostResponceMessage(string responce)
        {
            Message? message = JsonExtractor<Message>(responce, "json", right: 1);

            if (message is not null && CurrenWindow is not null && CurrenWindow is ClientWindow cw)
            {
                cw.UploadMessage(message, message.Username?.Equals(CurrentUser?.Login));
            }
        }

        /// <summary>
        /// Recive status of posting chat
        /// </summary>
        /// <param name="responce">
        /// Responce from the server
        /// </param>
        /// <exception cref="Exception">
        /// If status was false
        /// </exception>
        private void PostResponceChat(string responce)
        {
            UserChatPreloadSaveOutOfChanges = UserChatPreload;

            bool answer = responce.Contains("TRUE");

            if (answer)
            {
                GetRequestUpdateChatList();
            }
            else throw new Exception("Chat creation failed!");
        }
    }
}
