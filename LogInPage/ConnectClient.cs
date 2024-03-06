using Newtonsoft.Json;
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
        public static string HostName { get; } = "127.0.0.1";
        /// <summary>
        /// Server port
        /// </summary>
        public static int Port { get; } = 7007;
        /// <summary>
        /// Message size
        /// </summary>
        public static int MessageSize { get; } = 1024;
        
        public static User? CurrentUser { get; set; }
        /// <summary>
        /// Server's answer
        /// </summary>
        public static string Answer { get; set; } = string.Empty;
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
        public static void LogIn(string login, string password)
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
        public static void SignUp(string login, string password)
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
        public static void Message(string message, string type)
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
        public static void UpdateChat(int count = 50)
        {
            if (tcpClient is not null && Connected)
            {
                SendRequest($"GET --ACMSG count{{{count}}}");
            }
            else throw new Exception("Server is not responding.");
        }

        public static void UpdateUser(string message) 
        {
            if (tcpClient is not null && Connected)
            {
                string json = JsonConvert.SerializeObject(message);

                SendRequest($"PATCH --UPD_USER json{{{json}}}");
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
        public static void Close()
        {
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
                    while (true)
                    {
                        // Waiting for responce
                        while (!stream.DataAvailable) Thread.Sleep(500);

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

                                            if (messageList is not null)
                                            {
                                                foreach (var message in messageList.Messages)
                                                {
                                                    ClientWindow.UploadMessage(message);
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

                                            if (messageList is not null)
                                            {
                                                foreach (var message in messageList.Messages)
                                                {
                                                    ClientWindow.UploadMessage(message);
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
