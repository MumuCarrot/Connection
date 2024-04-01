using System.Windows;
using System.Windows.Controls;

namespace LogInPage
{
    public partial class MessageFrame : UserControl
    {
        public MessageFrame(Message message, bool? isMy)
        {
            InitializeComponent();

            username.Text = message.Login;
            content.Text = message.Content;
            time.Text = DateTime.Parse(message.MessageDateTime).ToString("HH:mm");

            if (isMy is not null && (bool)isMy)
            {
                frame.HorizontalAlignment = HorizontalAlignment.Right;
                frame.CornerRadius = new(8, 8, 0, 8);
            }
            else 
            {
                frame.HorizontalAlignment = HorizontalAlignment.Left;
                frame.CornerRadius = new(8, 8, 8, 0);
            }
        }
    }
}
