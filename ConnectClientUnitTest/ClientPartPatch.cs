namespace LogInPage
{
    public partial class Client
    {
        /// <summary>
        /// PATCH switch responce
        /// </summary>
        /// <param name="responce">
        /// Responce from the server
        /// </param>
        /// <exception cref="Exception">
        /// Server is not responding.
        /// </exception>
        private void PatchResponce(string responce)
        {
            int methodIndex = responce.IndexOf(' ');
            if (methodIndex == -1) throw new Exception("Get method was not found.");

            string method = responce[..methodIndex];
            switch (method)
            {
                default:
                    throw new NotImplementedException();
            }
        }
    }
}