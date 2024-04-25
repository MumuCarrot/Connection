using System.Windows;
using System.Windows.Controls;
using Connect.message;

namespace LogInPage
{
    /// <summary>
    /// Message frame realisation
    /// </summary>
    public partial class MessageFrame : UserControl
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="message">
        /// Content
        /// </param>
        /// <param name="isMy">
        /// Is message of current user or not
        /// </param>
        public MessageFrame(Message message, bool? isMy)
        {
            InitializeComponent();

            username.Text = message.Username;
            content.Text = message.Content?.Text;
            if (message.Time is not null)
                time.Text = ((DateTime)message.Time).ToString("HH:mm");

            if (isMy is not null && (bool)isMy)
            {
                frame.HorizontalAlignment = HorizontalAlignment.Right;
                frame.CornerRadius = new(11, 11, 0, 8);
            }
            else 
            {
                frame.HorizontalAlignment = HorizontalAlignment.Left;
                frame.CornerRadius = new(11, 11, 8, 0);
            }
        }
    }
}
