namespace LogInPage
{
    public partial class Client
    {
        private void PatchResponce(string responce)
        {
            int methodIndex = responce.IndexOf(' ');
            if (methodIndex == -1) throw new Exception("Get method was not found.");

            string method = responce[..methodIndex];
            switch (method)
            {
                case "--UPD_AVATAR":
                    PatchImageUploader(responce);
                    break; // --UPD_AVATAR
            }
        }

        private void PatchImageUploader(string responce)
        {
            if (responce.Contains("ready")) ImageSenderUnready = false;
        }
    }
}