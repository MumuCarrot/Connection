using Newtonsoft.Json;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;


namespace LogInPage
{
    /// <summary>
    /// Static realisation of a CLIENT class
    /// </summary>
    public class Client
    {
        #region PUBLIC FIELDS & PROPERTIES
        /// <summary>
        /// Connection status shortcut
        /// </summary>
        public static bool Connected { get { return (tcpClient is not null) && tcpClient.Connected; } }
        /// <summary>
        /// Server ip
        /// </summary>
        public string HostName { get; } = "127.0.0.1";
        /// <summary>
        /// Server port
        /// </summary>
        public int Port { get; } = 7007;
        /// <summary>
        /// Message size
        /// </summary>
        public int MessageSize { get; } = 10240;
        /// <summary>
        /// User class
        /// </summary>
        public User? CurrentUser { get; set; }

        public Window? CurrenWindow { get; set; }
        /// <summary>
        /// Server's answer
        /// </summary>
        public string Answer { get; set; } = string.Empty;

        public bool ImageSenderUnready { get; set; } = true;

        public bool CloseClient { get; private set; } = false;

        public bool StayInClient { get; set; }
        #endregion

        #region PRIVATE FIELDS & PROPERTIES
        /// <summary>
        /// Connection
        /// </summary>
        private static TcpClient? tcpClient;
        /// <summary>
        /// Stream
        /// </summary>
        private static NetworkStream? stream;
        /// <summary>
        /// Initialization thread
        /// </summary>
        private static Thread? mainClientThread;
        #endregion

        /// <summary>
        /// Client initialization
        /// </summary>
        public void Start()
        {
            // Connection
            tcpClient = new(HostName, Port);
            // Stream initialization
            stream = tcpClient.GetStream();
            // Thread initialization
            mainClientThread = new(new ThreadStart(ReadAnswer));
            mainClientThread?.Start();
        }

        /// <summary>
        /// Log In GET request
        /// </summary>
        /// <param name="login">
        /// Login from LogInPage
        /// </param>
        /// <param name="password">
        /// Password from LogInPage
        /// </param>
        /// <exception cref="Exception">
        /// Server is not responding
        /// </exception>
        public void LogIn(string login, string password)
        {
            if (tcpClient is not null && Connected)
            {
                SendRequest($"GET --USER_CHECK login{{{login}}} password{{{password}}}");
            }
            else throw new Exception("Server is not responding.");
        }

        /// <summary>
        /// Sign Up GET request
        /// </summary>
        /// <param name="login">
        /// Login from LogInPage
        /// </param>
        /// <param name="password">
        /// Password from LogInPage
        /// </param>
        /// <exception cref="Exception">
        /// Server is not responding
        /// </exception>
        public void SignUp(string login, string password)
        {
            if (tcpClient is not null && Connected)
            {
                Answer = string.Empty;
                LogIn(login, password);

                while (Answer == string.Empty) ;

                if (Answer.Contains($"GET --USER_CHECK login{{{login}}} password{{{password}}}") && Answer.Contains("ANSWER{status{false}}"))
                {
                    SendRequest($"POST --USER login{{{login}}} password{{{password}}}");
                }
            }
            else throw new Exception("Server is not responding.");
        }

        /// <summary>
        /// Message POST request
        /// </summary>
        /// <param name="message">
        /// Message to post
        /// </param>
        /// <param name="type">
        /// Type of message
        /// </param>
        /// <exception cref="Exception">
        /// Server is not responding
        /// </exception>
        public void Message(string message, string type)
        {
            if (tcpClient is not null && Connected && CurrentUser is not null)
            {
                SendRequest($"POST --MSG login{{{CurrentUser.Login}}} content{{{message}}} msg_time{{{DateTime.Now}}} msg_type{{{type}}}");
            }
            else throw new Exception("Server is not responding.");
        }

        /// <summary>
        /// Update messages in chat
        /// </summary>
        /// <param name="count">
        /// How much messages should be added
        /// </param>
        /// <exception cref="Exception">
        /// Server is not responding
        /// </exception>
        public void UpdateChat(int count = 50)
        {
            if (tcpClient is not null && Connected)
            {
                SendRequest($"GET --ACMSG count{{{count}}}");
            }
            else throw new Exception("Server is not responding.");
        }

        public void UpdateUser()
        {
            if (tcpClient is not null && Connected)
            {
                string json = JsonConvert.SerializeObject(CurrentUser);

                SendRequest($"PATCH --UPD_USER user{{{json}}}");
            }
            else throw new Exception("Server is not responding.");
        }

        public void UpdateUserPassword(string password)
        {
            if (tcpClient is not null && Connected)
            {
                string?[]? str = [password, CurrentUser?.Login];
                string json = JsonConvert.SerializeObject(str);

                SendRequest($"PATCH --UPD_UPASSWORD json{{{json}}}");
            }
            else throw new Exception("Server is not responding.");
        }

        public void UploadAvatar(string path)
        {
            if (tcpClient is not null && Connected)
            {
                ProfilePicture ava = new()
                {
                    UserName = CurrentUser?.UserName ?? "null",
                    Login = CurrentUser?.Login ?? "null",
                    AvatarDateTime = DateTime.Now.ToString()
                };

                string json = JsonConvert.SerializeObject(ava);
                ImageSenderUnready = true;
                SendRequest($"PATCH --UPD_AVATAR avatar{{{json}}}");

                Thread uploader = new(new ParameterizedThreadStart(SendImage));
                uploader.Start(path);
            }
        }

        private void SendImage(object? path)
        {
            if (tcpClient is not null && Connected && stream is not null)
            {
                if (path is not null && path is string pathString)
                {

                    while (ImageSenderUnready) Thread.Sleep(50);

                    byte[] image_bytes = File.ReadAllBytes(pathString);

                    byte[] part;
                    for (int i = 0; i < image_bytes.Length; i += 4000)
                    {

                        while (ImageSenderUnready) Thread.Sleep(50);

                        if (i + 4000 < image_bytes.Length)
                        {
                            part = image_bytes[i..(i + 4000)];
                            string jsonIN = JsonConvert.SerializeObject(part);
                            if (stream.CanRead)
                            {
                                ImageSenderUnready = true;
                                SendRequest($"PATCH --UPD_AVATAR part{{{jsonIN}}}");
                            }
                        }
                        else
                        {
                            part = image_bytes[i..image_bytes.Length];
                            string jsonIN = JsonConvert.SerializeObject(part);
                            if (stream.CanRead)
                            {
                                ImageSenderUnready = true;
                                SendRequest($"PATCH --UPD_AVATAR part{{{jsonIN}}}");
                            }
                        }
                    }

                    while (ImageSenderUnready) Thread.Sleep(500);

                    SendRequest($"PATCH --UPD_AVATAR status{{close}}");
                }
            }
            else throw new Exception("Server is not responding.");
        }

        /// <summary>
        /// Send method
        /// </summary>
        /// <param name="message">
        /// Message that will be requested
        /// </param>
        /// <exception cref="Exception">
        /// Stream is null
        /// </exception>
        private static void SendRequest(string message)
        {
            byte[] reqBytes = Encoding.UTF8.GetBytes(message);
            if (stream is not null)
                stream.Write(reqBytes, 0, reqBytes.Length);
            else throw new Exception("Stream is null.");
        }

        /// <summary>
        /// Close client
        /// </summary>
        public void Close()
        {
            CloseClient = true;
            stream?.Close();
            tcpClient?.Close();
        }

        /// <summary>
        /// Reading thread
        /// </summary>
        /// <exception cref="Exception">
        /// Stream is null.
        /// If something wasn't found.
        /// </exception>
        private void ReadAnswer()
        {
            try
            {
                if (stream is not null)
                {
                    while (!CloseClient)
                    {
                        // Waiting for responce
                        while (!stream.DataAvailable || CloseClient) Thread.Sleep(500);

                        if (CloseClient) break;

                        // Reading answer
                        byte[] bytesBuff = new byte[MessageSize];
                        if (stream is not null)
                            stream.Read(bytesBuff, 0, bytesBuff.Length);
                        else throw new Exception("Stream is null. #CR0001");

                        // Translating answer
                        Answer = Encoding.UTF8.GetString(bytesBuff);

                        // Searching for key word
                        int keyWordIndex = Answer.IndexOf(' ');
                        if (keyWordIndex == -1) throw new Exception("Key word was not found.");

                        // Getting key word
                        string keyWord = Answer[..keyWordIndex];

                        // Switching key words
                        switch (keyWord)
                        {
                            // GET key word logic
                            case "GET":

                                // Searching for method
                                int startMethodIndex = Answer.IndexOf("--");
                                if (startMethodIndex == -1) throw new Exception("GET method was not found.");
                                int endMethodIndex = Answer.IndexOf(' ', startMethodIndex);
                                if (endMethodIndex == -1) throw new Exception("GET method was not found.");

                                // Getting method
                                string method = Answer[startMethodIndex..endMethodIndex];

                                // Switching method
                                switch (method)
                                {
                                    // Returns status true if user already exist
                                    case "--USER_CHECK":
                                        // Searching for JSON start point
                                        int startIndexUC = Answer.IndexOf("json{") + "json{".Length;
                                        if (startIndexUC == -1) throw new Exception("JSON start point wasn't found.");
                                        int endIndexUC = Answer.IndexOf('}') + 1;
                                        if (endIndexUC == -1) throw new Exception("JSON end point wasn't found.");

                                        // Getting JSON into string
                                        string jsonUC = Answer[startIndexUC..endIndexUC];

                                        CurrentUser = JsonConvert.DeserializeObject<User>(jsonUC);

                                        break; // --USER_CHECK 

                                    case "--ACMSG":
                                        // Searching for JSON start point
                                        int startIndex = Answer.IndexOf('{') + 1;
                                        if (startIndex == -1) throw new Exception("JSON start point wasn't found.");
                                        int endIndex = Answer.LastIndexOf('}');
                                        if (endIndex == -1) throw new Exception("JSON end point wasn't found.");

                                        // Getting JSON into string
                                        string json = Answer[startIndex..endIndex];

                                        // Try of JSON deserialization
                                        try
                                        {
                                            // JSON deserialization
                                            MessageConteiner? messageList = JsonConvert.DeserializeObject<MessageConteiner>(json);

                                            if (messageList is not null && CurrenWindow is not null && CurrenWindow is ClientWindow cw)
                                            {
                                                foreach (var message in messageList.Messages)
                                                {
                                                    cw.UploadMessage(message, message.Login.Equals(CurrentUser?.Login));
                                                }
                                            }
                                            Answer = string.Empty;
                                        }
                                        catch (JsonException ex)
                                        {
                                            MessageBox.Show("Ошибка при десериализации JSON: " + ex.Message);
                                        }
                                        break; // --ACMSG

                                }
                                break; // GET
                            case "POST":
                                // Searching for method
                                int startMethodIndexPost = Answer.IndexOf("--");
                                if (startMethodIndexPost == -1) throw new Exception("POST method was not found.");
                                int endMethodIndexPost = Answer.IndexOf(' ', startMethodIndexPost);
                                if (endMethodIndexPost == -1) throw new Exception("POST method was not found.");

                                // Getting method
                                string methodPost = Answer[startMethodIndexPost..endMethodIndexPost];
                                switch (methodPost)
                                {
                                    case "--MSG":
                                        // Searching for JSON start point
                                        int startIndex = Answer.IndexOf('{') + 1;
                                        if (startIndex == -1) throw new Exception("JSON start point wasn't found.");
                                        int endIndex = Answer.LastIndexOf('}');
                                        if (endIndex == -1) throw new Exception("JSON end point wasn't found.");

                                        // Getting JSON into string
                                        string json = Answer[startIndex..endIndex];

                                        // Try of JSON deserialization
                                        try
                                        {
                                            // JSON deserialization
                                            MessageConteiner? messageList = JsonConvert.DeserializeObject<MessageConteiner>(json);

                                            if (messageList is not null && CurrenWindow is not null && CurrenWindow is ClientWindow cw)
                                            {
                                                foreach (var message in messageList.Messages)
                                                {
                                                    cw.UploadMessage(message, message.Login.Equals(CurrentUser?.Login));
                                                }
                                            }
                                            Answer = string.Empty;
                                        }
                                        catch (JsonException ex)
                                        {
                                            MessageBox.Show("Ошибка при десериализации JSON: " + ex.Message);
                                        }
                                        break; // MSG
                                }
                                break; // POST
                            case "PATCH":
                                // Searching for method
                                int startMethodIndexPatch = Answer.IndexOf("--");
                                if (startMethodIndexPatch == -1) throw new Exception("PATCH method was not found.");
                                int endMethodIndexPatch = Answer.IndexOf(' ', startMethodIndexPatch);
                                if (endMethodIndexPatch == -1) throw new Exception("PATCH method was not found.");

                                string methodPatch = Answer[startMethodIndexPatch..endMethodIndexPatch];

                                switch (methodPatch)
                                {
                                    case "--UPD_AVATAR":
                                        if (Answer.Contains("ready")) ImageSenderUnready = false;

                                        break; // UPD_AVATAR
                                }

                                break; // PATCH
                        }
                    }
                }
            }
            catch
            {
                Close();
            }
        }
    }
}
