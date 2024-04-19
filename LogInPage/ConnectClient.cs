#define DEBUG

using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace LogInPage
{
    /// <summary>
    /// Realisation of a client
    /// </summary>
    public partial class Client
    {
        #region PUBLIC FIELDS & PROPERTIES
        /// <summary>
        /// Connection status shortcut
        /// </summary>
        /// <remarks>
        /// <c>True</c> if user connected
        /// <br/>
        /// <c>False</c> if user doesen't
        /// </remarks>
        public static bool Connected { get { return (tcpClient is not null) && tcpClient.Connected; } }
        /// </summary>
        /// /// <summary>
        /// If login and password fit
        /// </summary>
        /// <remarks>
        /// <c>True</c> if user can log in
        /// <br/>
        /// <c>False</c> if login or password doesn't fit
        /// </remarks>
        public bool ServerConfirmation { get; set; } = false;
        /// <summary>
        /// Indicates should apllication save information about user or not
        /// </summary>
        /// <remarks>
        /// <c>True</c> to save user
        /// <br/>
        /// <c>False</c> to do not save user
        /// </remarks>
        public bool StayInClient { get; set; }
        /// <summary>
        /// Logged user in current usage
        /// </summary>
        public User? CurrentUser { get; set; } = null;
        /// <summary>
        /// Window that currently open
        /// </summary>
        public Window? CurrenWindow { get; set; }
        /// <summary>
        /// Users that was found in search
        /// </summary>
        public List<Chat>? UserChatPreload { get; set; }
        #endregion

        #region PRIVATE FIELDS & PROPERTIES
        /// <summary>
        /// Terminates all processes if the client closes
        /// </summary>
        private bool CloseClient { get; set; } = false;
        /// <summary>
        /// Host address
        /// </summary>
        private string HostName { get; } = "127.0.0.1";
        /// <summary>
        /// Host port
        /// </summary>
        private int Port { get; } = 7007;
        /// <summary>
        /// Message size
        /// </summary>
        private int MessageSize { get; } = 10240;
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
        /// <summary>
        /// Responce from the server
        /// </summary>
        private string Responce { get; set; } = string.Empty;
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
        /// Dispose client
        /// </summary>
        public void Close()
        {
            CloseClient = true;
            stream?.Close();
            tcpClient?.Close();
        }
        /// <summary>
        /// Json extractor
        /// </summary>
        /// <typeparam name="T">
        /// Type of instance that would be returned
        /// </typeparam>
        /// <param name="json">
        /// String that contains json
        /// </param>
        /// <param name="keyWord">
        /// The keyword by which the reading reference point will be set
        /// </param>
        /// <param name="left">
        /// Left shift
        /// </param>
        /// <param name="right">
        /// Right shift
        /// </param>
        /// <returns>
        /// New instance converted from json
        /// </returns>
        /// <exception cref="Exception">
        /// JSON start or end point wasn't found.
        /// </exception>
        private static T? JsonExtractor<T>(string json, string keyWord, int left = 0, int right = 0)
        {
            try
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
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
                return default;
            }
        }
        /// <summary>
        /// Log In GET request
        /// </summary>
        /// <param name="login">
        /// Login
        /// </param>
        /// <param name="password">
        /// Password
        /// </param>
        /// <exception cref="Exception">
        /// Server is not responding.
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

                SendRequest($"GET --LOG-IN json{{{json}}}");
            }
            else throw new Exception("Server is not responding.");
        }
        /// <summary>
        /// Log In GET request
        /// </summary>
        /// <param name="user">
        /// User
        /// </param>
        /// <exception cref="Exception">
        /// Server is not responding.
        /// </exception>
        public void GetRequestLogIn(User user)
        {
            if (tcpClient is not null && Connected)
            {
                string json = JsonConvert.SerializeObject(user);

                SendRequest($"GET --LOG-IN json{{{json}}}");
            }
            else throw new Exception("Server is not responding.");
        }
        /// <summary>
        /// Search for users by <paramref name="character"/>
        /// </summary>
        /// <param name="character">
        /// Character combination
        /// </param>
        /// <exception cref="Exception">
        /// Server is not responding.
        /// </exception>
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
        /// Server is not responding.
        /// </exception>
        public void GetRequestUpdateChat(string chatId, int count = 50)
        {
            if (tcpClient is not null && Connected)
            {
                string json = JsonConvert.SerializeObject(new string[] { chatId, count.ToString() });

                SendRequest($"GET --CHAT-HISTORY json{{{json}}}");
            }
            else throw new Exception("Server is not responding.");
        }
        /// <summary>
        /// Update's chat list of user gane last message, chatusers and last time of message
        /// </summary>
        /// <exception cref="Exception">
        /// Server is not responding.
        /// </exception>
        public void GetRequestUpdateChatList()
        {
            if (tcpClient is not null && Connected)
            {
                string json = JsonConvert.SerializeObject(CurrentUser?.Login);

                SendRequest($"GET --CHAT-LIST json{{{json}}}");
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
        /// Server is not responding.
        /// </exception>
        public void PostRequestSignUp(string login, string password)
        {
            if (tcpClient is not null && Connected)
            {
                Responce = string.Empty;

                User newUser = new()
                {
                    Login = login,
                    Password = password
                };

                GetRequestLogIn(newUser);

                while (Responce == string.Empty) ;

                if (Responce.Contains("ANSWER{status{false}}"))
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
        /// Server is not responding.
        /// </exception>
        public void PostRequestMessage(Message message, string chatId)
        {
            if (tcpClient is not null && Connected && CurrentUser is not null)
            {
                string json = JsonConvert.SerializeObject(message);

                SendRequest($"POST --MSG json{{{json}}}");
            }
            else throw new Exception("Server is not responding.");
        }
        /// <summary>
        /// Update user
        /// </summary>
        /// <exception cref="Exception">
        /// Server is not responding.
        /// </exception>
        public void PatchRequestUser()
        {
            if (tcpClient is not null && Connected)
            {
                string json = JsonConvert.SerializeObject(CurrentUser);

                SendRequest($"PATCH --UPD_USER user{{{json}}}");
            }
            else throw new Exception("Server is not responding.");
        }
        /// <summary>
        /// Update profile picture
        /// </summary>
        /// <exception cref="Exception">
        /// Server is not responding.
        /// </exception>
        public void PatchProfilePicture()
        {
            if (tcpClient is not null && Connected && CurrentUser is not null)
            {
                CurrentUser.UserProfilePicture.Login = CurrentUser.Login;
                string json = JsonConvert.SerializeObject(CurrentUser.UserProfilePicture);

                SendRequest($"PATCH --PROFILE-PIC user{{{json}}}");
            }
            else throw new Exception("Server is not responding.");
        }
        /// <summary>
        /// Update password
        /// </summary>
        /// <param name="password">
        /// Password
        /// </param>
        /// <exception cref="Exception">
        /// Server is not responding.
        /// </exception>
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
        /// <summary>
        /// Send method
        /// </summary>
        /// <param name="message">
        /// Message that will be requested
        /// </param>
        /// <exception cref="Exception">
        /// Stream is null.
        /// </exception>
        private static void SendRequest(string message)
        {
            byte[] reqBytes = Encoding.UTF8.GetBytes(message);
            if (stream is not null)
                stream.Write(reqBytes, 0, reqBytes.Length);
            else throw new Exception("Stream is null.");
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
                        Responce = Encoding.UTF8.GetString(bytesBuff);

                        // Searching for key word
                        int keyWordIndex = Responce.IndexOf(' ');
                        if (keyWordIndex == -1) throw new Exception("Key word was not found.");

                        // Getting key word
                        string keyWord = Responce[..keyWordIndex];

                        // Switching key words
                        switch (keyWord)
                        {
                            // GET key word logic
                            case "GET":
                                GetResponce(Responce[(keyWordIndex + 1)..]);
                                break; // GET
                            case "POST":
                                PostResponce(Responce[(keyWordIndex + 1)..]);
                                break; // POST
                            case "PATCH":
                                PatchResponce(Responce[(keyWordIndex + 1)..]);
                                break; // PATCH
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("1. " + ex.Message);
#endif
            }
        }
    }
}
