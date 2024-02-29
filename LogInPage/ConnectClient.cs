using Org.BouncyCastle.Asn1.Ocsp;
using System.Net.Sockets;
using System.Text;
using System.Windows.Controls;

namespace LogInPage
{
    public class Client()
    {
        #region PUBLIC FIELDS & PROPERTIES
        /// <summary>
        /// Connection status
        /// </summary>
        public bool? Connected { get { return (tcpClient is not null) ? tcpClient?.Connected : null; } }
        public static string HostName { get; } = "127.0.0.1";       // Server ip
        public static int Port { get; } = 7007;                     // Server port
        public static int MessageSize { get; } = 1024;              // Request size
        public string Login { get; set; } = string.Empty;           // User login
        public string Passw { get; set; } = string.Empty;           // User password
        public string Answer { get; set; } = string.Empty;
        #endregion

        #region PRIVATE FIELDS & PROPERTIES
        private TcpClient? tcpClient;                               // Client TCP connection
        private Thread? mainClientThread;                           // Initialization thread
        private NetworkStream? stream;                              // Client network stream
        private Page? chat;
        #endregion

        /// <summary>
        /// Client initialization
        /// </summary>
        public void StartClient()
        {
            // Connection
            tcpClient = new(HostName, Port);
            // Stream initialization
            stream = tcpClient.GetStream();
            // Thread initialization
            mainClientThread = new(new ThreadStart(ClientProcesses));
            mainClientThread.Start();
        }

        /// <summary>
        /// Threads to start
        /// </summary>
        private void ClientProcesses()
        {
            Thread reader = new(new ThreadStart(ReadAnswer));
            reader.Start();
        }

        /// <summary>
        /// LogIn GET request
        /// </summary>
        /// <param name="login">Login from LogInPage</param>
        /// <param name="password">Password from LogInPage</param>
        /// <exception cref="Exception"></exception>
        public void LogIn(string login, string password)
        {
            if (tcpClient is not null && tcpClient.Connected)
            {
                SendRequest($"GET --USER_CHECK login{{{login}}} password{{{password}}}");
            }
            else throw new Exception("Server is not responding.");
        }

        public void SignUp(string login, string password)
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

        public void Message(string message, string type)
        {
            if (tcpClient is not null && tcpClient.Connected)
            {
                SendRequest($"POST --MSG login{{{Login}}} content{{{message}}} msg_time{{{DateTime.Now}}} msg_type{{{type}}}");
            }
            else throw new Exception("Server is not responding.");
        }

        public void UpdateChat(Page chat, int count = 50)
        {
            this.chat = chat;
            SendRequest($"GET --ACMSG count{{{count}}}");
        }

        /// <summary>
        /// Reading thread
        /// </summary>
        /// <exception cref="Exception">
        /// Stream is null.
        /// #CR0001 - Client reader exception
        /// </exception>
        protected void ReadAnswer()
        {
            while (true)
            {
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
                    this.Login = answer[loginStart..loginEnd];

                    // Находим позиции начала и конца пароля
                    int passwordStart = answer.IndexOf("password{") + "password{".Length;
                    int passwordEnd = answer.IndexOf('}', passwordStart);

                    // Извлекаем подстроку для пароля
                    this.Passw = answer[passwordStart..passwordEnd];
                }
                else if (answer.Contains("GET --ACMSG"))
                {
                    if (this.chat is client_window_chat_frame cwcf) 
                    {
                        int loginStart = answer.IndexOf("login{") + "login{".Length;
                        int loginEnd = answer.IndexOf('}', loginStart);

                        string login = answer[loginStart..loginEnd];

                        int contentStart = answer.IndexOf("content{") + "content{".Length;
                        int contentEnd = answer.IndexOf('}', contentStart);

                        string content = answer[contentStart..contentEnd];

                        int msg_timeStart = answer.IndexOf("msg_time{") + "msg_time{".Length;
                        int msg_timeEnd = answer.IndexOf('}', msg_timeStart);

                        string msg_time = answer[msg_timeStart..msg_timeEnd];

                        int typeStart = answer.IndexOf("type{") + "type{".Length;
                        int typeEnd = answer.IndexOf('}', typeStart);

                        string type = answer[typeStart..typeEnd];

                        cwcf.UploadMessage(null, client_name:login, dt:msg_time, message:content, type:type);
                    }
                }
            }
        }

        /// <summary>
        /// Send method
        /// </summary>
        /// <param name="message">Message that will be requested</param>
        /// <exception cref="Exception">
        /// Stream is null
        /// #CR0002 - Client sender exception
        /// </exception>
        private void SendRequest(string message)
        {
            byte[] reqBytes = Encoding.UTF8.GetBytes(message);
            if (stream is not null)
                stream.Write(reqBytes, 0, reqBytes.Length);
            else throw new Exception("Stream is null. #CR0002");
        }
    }
}
