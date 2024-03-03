using System.Net.Sockets;
using System.Text;

namespace LogInPage
{
    /// <summary>
    /// Static realisation of a CLIENT class
    /// </summary>
    public class Client
    {
        #region PUBLIC FIELDS & PROPERTIES
        /// <summary>
        /// Connection status
        /// </summary>
        public static bool? Connected { get { return (tcpClient is not null) ? tcpClient?.Connected : null; } }
        public static string HostName { get; } = "127.0.0.1";               // Server ip
        public static int Port { get; } = 7007;                             // Server port
        public static int MessageSize { get; } = 1024;                      // Request size
        public static string Login { get; set; } = string.Empty;            // User login
        public static string Passw { get; set; } = string.Empty;            // User password
        public static string Answer { get; set; } = string.Empty;           // Server answer. TODO in a future: Change login request to delete this useles field
        #endregion

        #region PRIVATE FIELDS & PROPERTIES
        private static TcpClient? tcpClient;                                // Client TCP connection
        private static NetworkStream? stream;                               // Client network stream
        private static Thread? mainClientThread;                            // Initialization thread
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
            mainClientThread = new(new ThreadStart(StartClient));
            mainClientThread?.Start();
        }

        private void StartClient() 
        {
            Thread answThread = new(new ThreadStart(ReadAnswer));
            answThread.Start();
        }

        /// <summary>
        /// LogIn GET request
        /// </summary>
        /// <param name="login">Login from LogInPage</param>
        /// <param name="password">Password from LogInPage</param>
        /// <exception cref="Exception"></exception>
        public static void LogIn(string login, string password)
        {
            if (tcpClient is not null && tcpClient.Connected)
            {
                SendRequest($"GET --USER_CHECK login{{{login}}} password{{{password}}}");
            }
            else throw new Exception("Server is not responding.");
        }

        public static void SignUp(string login, string password)
        {
            if (tcpClient is not null && tcpClient.Connected)
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

        public static void Message(string message, string type)
        {
            if (tcpClient is not null && tcpClient.Connected)
            {
                SendRequest($"POST --MSG login{{{Login}}} content{{{message}}} msg_time{{{DateTime.Now}}} msg_type{{{type}}}");
            }
            else throw new Exception("Server is not responding.");
        }

        public static void UpdateChat(int count = 50)
        {
            SendRequest($"GET --ACMSG count{{{count}}}");
        }

        /// <summary>
        /// Send method
        /// </summary>
        /// <param name="message">Message that will be requested</param>
        /// <exception cref="Exception">
        /// Stream is null
        /// #CR0002 - Client sender exception
        /// </exception>
        private static void SendRequest(string message)
        {
            byte[] reqBytes = Encoding.UTF8.GetBytes(message);
            if (stream is not null)
                stream.Write(reqBytes, 0, reqBytes.Length);
            else throw new Exception("Stream is null. #CR0002");
        }

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
        /// #CR0001 - Client reader exception
        /// </exception>
        private void ReadAnswer()
        {
            if (stream is not null)
            {
                while (true)
                {
                    while (!stream.DataAvailable) Thread.Sleep(1000);

                    byte[] bytesBuff = new byte[MessageSize];
                    if (stream is not null)
                        stream.Read(bytesBuff, 0, bytesBuff.Length);
                    else throw new Exception("Stream is null. #CR0001");

                    string answer = Encoding.UTF8.GetString(bytesBuff);

                    Answer = answer;

                    if (answer.Contains("GET --USER_CHECK") && answer.Contains("status{true}"))
                    {
                        // Находим позиции начала и конца логина
                        int loginStart = answer.IndexOf("login{") + "login{".Length;
                        int loginEnd = answer.IndexOf('}', loginStart);

                        // Извлекаем подстроку для логина
                        Login = answer[loginStart..loginEnd];

                        // Находим позиции начала и конца пароля
                        int passwordStart = answer.IndexOf("password{") + "password{".Length;
                        int passwordEnd = answer.IndexOf('}', passwordStart);

                        // Извлекаем подстроку для пароля
                        Passw = answer[passwordStart..passwordEnd];
                    }
                    else if (answer.Contains("GET --ACMSG"))
                    {
                        int loginStart = answer.IndexOf("login{") + "login{".Length;
                        int loginEnd = answer.IndexOf('}', loginStart);

                        string userName = answer[loginStart..loginEnd];

                        int contentStart = answer.IndexOf("content{") + "content{".Length;
                        int contentEnd = answer.IndexOf('}', contentStart);

                        string message = answer[contentStart..contentEnd];

                        int msg_timeStart = answer.IndexOf("msg_time{") + "msg_time{".Length;
                        int msg_timeEnd = answer.IndexOf('}', msg_timeStart);

                        string dateTime = answer[msg_timeStart..msg_timeEnd];

                        int typeStart = answer.IndexOf("type{") + "type{".Length;
                        int typeEnd = answer.IndexOf('}', typeStart);

                        string messageType = answer[typeStart..typeEnd];

                        ClientWindow.UploadMessage(dateTime, userName, message, messageType);
                    }
                }
            }
        }
    }
}
