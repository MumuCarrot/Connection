using Newtonsoft.Json;
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
                    while (!stream.DataAvailable) Thread.Sleep(500);

                    byte[] bytesBuff = new byte[MessageSize];
                    if (stream is not null)
                        stream.Read(bytesBuff, 0, bytesBuff.Length);
                    else throw new Exception("Stream is null. #CR0001");

                    Answer = Encoding.UTF8.GetString(bytesBuff);

                    if (Answer.Contains("GET --USER_CHECK") && Answer.Contains("status{true}"))
                    {
                        int loginStart = Answer.IndexOf("login{") + "login{".Length;
                        int loginEnd = Answer.IndexOf('}', loginStart);

                        Login = Answer[loginStart..loginEnd];

                        int passwordStart = Answer.IndexOf("password{") + "password{".Length;
                        int passwordEnd = Answer.IndexOf('}', passwordStart);

                        // Извлекаем подстроку для пароля
                        Passw = Answer[passwordStart..passwordEnd];
                    }
                    else if (Answer.Contains("GET --ACMSG"))
                    {
                        // Находим позицию начала JSON-строки
                        int startIndex = Answer.IndexOf('{') + 1;

                        // Если JSON-строка найдена
                        if (startIndex != -1)
                        {
                            // Находим позицию конца JSON-строки
                            int endIndex = Answer.LastIndexOf('}');

                            // Если найден конец JSON-строки
                            if (endIndex != -1)
                            {
                                // Извлекаем JSON-строку из исходной строки
                                string json = Answer.Substring(startIndex, endIndex - startIndex);

                                // Попытка десериализации JSON-строки
                                try
                                {
                                    // Десериализация JSON-строки в список сообщений
                                    var messageList = JsonConvert.DeserializeObject<MessageConteiner>(json);

                                    if (messageList is not null)
                                        foreach (var message in messageList.Messages)
                                        {
                                            ClientWindow.UploadMessage(message.MessageDateTime, message.Login, message.Content, message.MessageType);
                                        }
                                }
                                catch (JsonException ex)
                                {
                                    Console.WriteLine("Ошибка при десериализации JSON: " + ex.Message);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Конец JSON-строки не найден.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("JSON-строка не найдена.");
                        }
                    }
                }
            }
        }
    }
}
