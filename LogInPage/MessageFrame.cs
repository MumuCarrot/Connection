using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LogInPage
{
    class MessageFrame : UserControl
    {
        public SolidColorBrush frameColorBrush = new(Color.FromRgb(191, 230, 255));
        private CornerRadius frameCornerRadius = new(8);
        private Thickness frameMargin = new(8, 5, 8, 0);

        public bool isMyMessage = false;

        private readonly Border? frame;
        private readonly Grid? frameGrid;
        private readonly TextBlock? username;
        private readonly TextBlock? content;
        private readonly TextBlock? dateTime;

        public MessageFrame(Message message, bool? isMy)
        {
            if (message.MessageType == "text") 
            { 
                username = new TextBlock
                {
                    Text = message.Login,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Padding = new Thickness(5, 0, 5, 0),
                    FontWeight = FontWeights.Bold
                };

                isMyMessage = isMy is not null && (bool)isMy;

                content = new TextBlock
                {
                    Text = message.Content,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    TextWrapping = TextWrapping.Wrap,
                    Padding = new Thickness(5, 0, 5, 0)
                };

                dateTime = new TextBlock
                {
                    Text = DateTime.Parse(message.MessageDateTime).ToString("HH:mm"),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Padding= new Thickness(5, 0, 5, 0)
                };

                frameGrid = new Grid
                {
                    RowDefinitions = 
                    { 
                        new RowDefinition { Height = GridLength.Auto }, 
                        new RowDefinition { Height = GridLength.Auto }, 
                        new RowDefinition { Height = GridLength.Auto }, 
                        new RowDefinition { Height = GridLength.Auto }
                    },
                };

                frameGrid.Children.Add(username);
                Grid.SetRow(username, 0);

                frameGrid.Children.Add(content);
                Grid.SetRow(content, 2);

                frameGrid.Children.Add(dateTime);
                Grid.SetRow(dateTime, 3);

                frame = new Border
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Background = frameColorBrush,
                    CornerRadius = frameCornerRadius,
                    Margin = frameMargin,
                    Child = frameGrid
                };

                if (isMyMessage) frame.HorizontalAlignment = HorizontalAlignment.Right;

                Content = frame;
            }
        }
    }
}
