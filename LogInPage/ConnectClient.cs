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
    public partial class Client
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
        public User? CurrentUser { get; set; } = null;

        public Window? CurrenWindow { get; set; }
        /// <summary>
        /// Server's answer
        /// </summary>
        public string Answer { get; set; } = string.Empty;

        public bool ImageSenderUnready { get; set; } = true;

        public bool CloseClient { get; private set; } = false;

        public bool StayInClient { get; set; }

        public List<string>? UserChatIds { get; set; }
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
        public void GetRequestLogIn(string login, string password)
        {
            if (tcpClient is not null && Connected)
            {
                string json = JsonConvert.SerializeObject(new User
                {
                    Login = login,
                    Password = password
                });

                SendRequest($"GET --USER_CHECK json{{{json}}}");
            }
            else throw new Exception("Server is not responding.");
        }

        public void GetRequestLogIn(User user)
        {
            if (tcpClient is not null && Connected)
            {
                string json = JsonConvert.SerializeObject(user);

                SendRequest($"GET --USER_CHECK json{{{json}}}");
            }
            else throw new Exception("Server is not responding.");
        }

        public void GetRequestUsersByLogin(string character)
        {
            if (tcpClient is not null && Connected)
            {
                string json = JsonConvert.SerializeObject(new string[] { CurrentUser?.Login ?? "undef", character });

                SendRequest($"GET --USER-LIST json{{{json}}}");
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
        public void GetRequestUpdateChat(int count = 50)
        {
            if (tcpClient is not null && Connected)
            {
                string json = JsonConvert.SerializeObject(count);

                SendRequest($"GET --ACMSG json{{{json}}}");
            }
            else throw new Exception("Server is not responding.");
        }

        public void GetRequestUpdateChatList()
        {
            if (tcpClient is not null && Connected)
            {
                string userLogin = JsonConvert.SerializeObject(CurrentUser?.Login);

                SendRequest($"GET --CHAT-LIST json{{{userLogin}}}");
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
        public void PostRequestSignUp(string login, string password)
        {
            if (tcpClient is not null && Connected)
            {
                Answer = string.Empty;

                User newUser = new()
                {
                    Login = login,
                    Password = password
                };

                GetRequestLogIn(newUser);

                while (Answer == string.Empty) ;

                if (Answer.Contains("ANSWER{status{false}}"))
                {
                    string json = JsonConvert.SerializeObject(newUser);

                    SendRequest($"POST --USER json{{{json}}}");
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
        public void PostRequestMessage(Message message)
        {
            if (tcpClient is not null && Connected && CurrentUser is not null)
            {
                string json = JsonConvert.SerializeObject(message);

                SendRequest($"POST --MSG json{{{json}}}");
            }
            else throw new Exception("Server is not responding.");
        }

        public void PatchRequestUser()
        {
            if (tcpClient is not null && Connected)
            {
                string json = JsonConvert.SerializeObject(CurrentUser);

                SendRequest($"PATCH --UPD_USER user{{{json}}}");
            }
            else throw new Exception("Server is not responding.");
        }

        public void PatchRequestPassword(string password)
        {
            if (tcpClient is not null && Connected)
            {
                string?[]? str = [password, CurrentUser?.Login];
                string json = JsonConvert.SerializeObject(str);

                SendRequest($"PATCH --UPD_UPASSWORD json{{{json}}}");
            }
            else throw new Exception("Server is not responding.");
        }

        public void PatchRequestProfilePicture(string path)
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

                Thread uploader = new(new ParameterizedThreadStart(PatchRequestProfilePictureImage));
                uploader.Start(path);
            }
        }

        private void PatchRequestProfilePictureImage(object? path)
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

        private static T? JsonExtractor<T>(string json, string keyWord, int left = 0, int right = 0)
        {
            // Searching for JSON start point
            int start = json.IndexOf($"{keyWord}{{") + $"{keyWord}{{".Length;
            if (start == -1) throw new Exception("JSON start point wasn't found.");
            int endpointStart = json.LastIndexOf("},") + "},".Length;
            if (endpointStart == -1) endpointStart = start;
            int end = json.IndexOf('}', endpointStart);
            if (end == -1) throw new Exception("JSON end point wasn't found.");

            string str = json[(start + left)..(end + right)];

            return JsonConvert.DeserializeObject<T>(str);
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
                        try
                        {
                            while (!stream.DataAvailable)
                            {
                                Thread.Sleep(500);
                            }
                        }
                        catch
                        {
                            if (CloseClient) break;
                            else continue;
                        }

                        // Reading answer
                        byte[] bytesBuff = new byte[MessageSize];
                        if (stream is not null)
                            stream.Read(bytesBuff, 0, bytesBuff.Length);
                        else throw new Exception("Stream is null.");

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
                                GetResponce(Answer[(keyWordIndex + 1)..]);
                                break; // GET
                            case "POST":
                                PostResponce(Answer[(keyWordIndex + 1)..]);
                                break; // POST
                            case "PATCH":
                                PatchResponce(Answer[(keyWordIndex + 1)..]);
                                break; // PATCH
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("1. " + ex.Message);
            }
        }
    }
}
