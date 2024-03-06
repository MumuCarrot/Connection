using System.Windows;
using System.Windows.Input;

namespace LogInPage
{
    public partial class MainWindow : Window
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public readonly static Client client = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_Hide(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MurkaPolygon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

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
                        if (ConnectionFrame.Content is SignInPage sip)
                            if (sip.LoginTextBox.Text.Length >= 6 && sip.PasswordTextBox.Password.Length >= 5)
                            {
                                bool connected = false;
                                while (!connected)
                                {
                                    try
                                    {
                                        if (Client.Connected != true)
                                            client.Start();
                                        connected = true;
                                    }
                                    catch
                                    {
                                        Thread.Sleep(2000);
                                    }
                                }
                                Client.LogIn(sip.LoginTextBox.Text, sip.PasswordTextBox.Password);
                                while (Client.Answer.Length <= 0) ;
                                if (Client.Answer.Contains("status{true}"))
                                {
                                    var clw = new ClientWindow();
                                    clw.Show();

                                    this.Close();
                                }
                            }
                            else
                            {
                                LogoBar.FontSize = 30;
                                LogoBar.Text = "Login or password\ntoo short.";
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
                        if (ConnectionFrame.Content is SignUpPage sup)
                            if (sup.LoginTextBox.Text.Length >= 6 && 
                                sup.PasswordTextBox.Password.Length >= 5 && 
                                sup.PassPasswordTextBox.Password.Length >= 5)
                            {
                                if (sup.PasswordTextBox.Password.Equals(sup.PassPasswordTextBox.Password))
                                {
                                    bool connected = false;
                                    while (!connected)
                                    {
                                        try
                                        {
                                            if (Client.Connected != true)
                                                client.Start();
                                            connected = true;
                                        }
                                        catch
                                        {
                                            Thread.Sleep(2000);
                                        }
                                    }
                                    Client.SignUp(sup.LoginTextBox.Text, sup.PasswordTextBox.Password);
                                    if (sup.LoginTextBox.Text.ToString().Equals(Client.CurrentUser?.Login) && sup.PasswordTextBox.Password.ToString().Equals(Client.CurrentUser?.Password))
                                    {
                                        var clw = new ClientWindow();
                                        clw.Show();

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