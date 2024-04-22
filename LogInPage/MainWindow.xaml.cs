using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace LogInPage
{
    /// <summary>
    /// Authorization/Registration window 
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Client for user using
        /// </summary>
        public readonly Client client;

        public class UserLock
        {
            public string Login { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        /// <summary>
        /// Window initialization
        /// </summary>
        public MainWindow()
        {
            client = new();

            if (File.Exists("user_account_lock.xml"))
            {
                XmlSerializer x = new(typeof(UserLock));

                UserLock? user = null;
                using (FileStream fs = new("user_account_lock.xml", FileMode.Open))
                {
                    user = x.Deserialize(fs) as UserLock;
                }

                // Loop util client connect to the server
                bool connected = false;
                while (!connected)
                {
                    if (Client.Connected != true)
                    {
                        client.Start();
                        Thread.Sleep(500);
                        continue;
                    }
                    connected = true;
                }

                client.GetRequestLogIn(user?.Login ?? "undef", user?.Password ?? "undef");


                // Wait for an answer status{true}
                while (!client.ServerConfirmation)
                {
                    Thread.Sleep(500);
                }

                client.GetRequestUpdateChatList();
                while (!client.PreloadChatIsReady) Thread.Sleep(500);

                var clw = new ClientWindow(this);
                clw.Show();

                // If true end this window
                this.Close();
            }

            InitializeComponent();
        }

        /// <summary>
        /// Minimization
        /// </summary>
        /// <param name="sender">
        /// Rectangle sender
        /// </param>
        /// <param name="e">
        /// Click
        /// </param>
        private void Button_Click_Hide(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Close
        /// </summary>
        /// <param name="sender">
        /// Rectangle sender
        /// </param>
        /// <param name="e">
        /// Click
        /// </param>
        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Drug and move
        /// </summary>
        /// <param name="sender">
        /// Polyghon sender
        /// </param>
        /// <param name="e">
        /// MouseDown
        /// </param>
        private void MurkaPolygon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        /// <summary>
        /// Log In
        /// </summary>
        /// <param name="sender">
        /// Button
        /// </param>
        /// <param name="e">
        /// Click
        /// </param>
        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            if (SignInBtn.Content.ToString() == "Sign In")
            {
                if (SignUpBtn.Content.ToString() == "Sign Up")
                {
                    ConnectionFrame.Content = new SignInPage();
                    SignUpBtn.Content = "Cancel";
                }
                else
                {
                    try
                    {
                        if (ConnectionFrame.Content is SignInPage signInPage)
                        {
                            // Check on entered data in login and password fields
                            if (signInPage.LoginTextBox.Text.Length >= 6 &&
                                signInPage.PasswordTextBox.Password.Length >= 5)
                            {
                                // Loop util client connect to the server
                                bool connected = false;
                                while (!connected)
                                {
                                    if (Client.Connected != true)
                                    {
                                        client.Start();
                                        Thread.Sleep(500);
                                        continue;
                                    }
                                    connected = true;
                                }

                                // Client connected and now log in him to the server
                                client.GetRequestLogIn(signInPage.LoginTextBox.Text, signInPage.PasswordTextBox.Password);

                                // Wait for an answer status{true}
                                while (!client.ServerConfirmation) Thread.Sleep(500);

                                if (client.CurrentUser is not null)
                                {
                                    if (StayCheck.IsChecked is not null && (bool)StayCheck.IsChecked)
                                    {
                                        XmlSerializer x = new(typeof(UserLock));

                                        using TextWriter writer = new StreamWriter("user_account_lock.xml");
                                        x.Serialize(writer, new UserLock
                                        {
                                            Login = signInPage.LoginTextBox.Text,
                                            Password = signInPage.PasswordTextBox.Password
                                        });

                                        client.StayInClient = (bool)StayCheck.IsChecked;
                                    }

                                    client.GetRequestUpdateChatList();
                                    while (!client.PreloadChatIsReady) Thread.Sleep(500);

                                    var clw = new ClientWindow(this);
                                    clw.Show();

                                    // If true end this window
                                    this.Close();
                                }
                            }
                            else
                            {
                                LogoBar.FontSize = 30;
                                LogoBar.Text = "Login or password\ntoo short.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error");
                    }
                }
            }
            else
            {
                ConnectionFrame.Content = null;
                SignInBtn.Content = "Sign In";
            }
        }

        /// <summary>
        /// Sign Up
        /// </summary>
        /// <param name="sender">
        /// Button
        /// </param>
        /// <param name="e">
        /// Click
        /// </param>
        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (SignUpBtn.Content.ToString() == "Sign Up")
            {
                if (SignInBtn.Content.ToString() == "Sign In")
                {
                    ConnectionFrame.Content = new SignUpPage();
                    SignInBtn.Content = "Cancel";
                }
                else
                {
                    try
                    {
                        if (ConnectionFrame.Content is SignUpPage signUpPage)
                        {
                            // Check on entered data in login, password and pass password fields
                            if (signUpPage.LoginTextBox.Text.Length >= 6 &&
                                signUpPage.PasswordTextBox.Password.Length >= 5 &&
                                signUpPage.PassPasswordTextBox.Password.Length >= 5)
                            {
                                // Check on equality of password and pass password
                                if (signUpPage.PasswordTextBox.Password.Equals(signUpPage.PassPasswordTextBox.Password))
                                {
                                    // Loop util client connect to the server
                                    bool connected = false;
                                    while (!connected)
                                    {
                                        if (Client.Connected != true)
                                        {
                                            client.Start();
                                            Thread.Sleep(500);
                                            continue;
                                        }
                                        connected = true;
                                    }

                                    // Client connected and now sign up him to the server
                                    client.PostRequestSignUp(signUpPage.LoginTextBox.Text, signUpPage.PasswordTextBox.Password);

                                    // Wait for an answer and then check data on equality
                                    if (signUpPage.LoginTextBox.Text.ToString().Equals(client.CurrentUser?.Login) && signUpPage.PasswordTextBox.Password.ToString().Equals(client.CurrentUser?.Password))
                                    {
                                        var clw = new ClientWindow(this);
                                        clw.Show();

                                        if (StayCheck.IsChecked is not null && (bool)StayCheck.IsChecked)
                                        {
                                            XmlSerializer x = new(typeof(UserLock));

                                            using TextWriter writer = new StreamWriter("user_account_lock.xml");
                                            x.Serialize(writer, new UserLock
                                            {
                                                Login = signUpPage.LoginTextBox.Text,
                                                Password = signUpPage.PasswordTextBox.Password
                                            });
                                        }

                                        // If true end this window
                                        this.Close();
                                    }
                                }
                                else
                                {
                                    LogoBar.FontSize = 30;
                                    LogoBar.Text = "Password and pass\npassword is not equal";
                                }
                            }
                            else
                            {
                                LogoBar.FontSize = 30;
                                LogoBar.Text = "Login or password\ntoo short.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error");
                    }
                }
            }
            else
            {
                ConnectionFrame.Content = null;
                SignUpBtn.Content = "Sign Up";
            }
        }
    }
}