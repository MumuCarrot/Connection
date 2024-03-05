using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LogInPage
{
    class MessageFrame : UserControl
    {
        public SolidColorBrush frameColorBrush = new(Color.FromRgb(191, 230, 255));
        private CornerRadius frameCornerRadius = new(8);
        private Thickness frameMargin = new(8, 5, 0, 0);

        private readonly Border? frame;
        private readonly Grid? frameGrid;
        private readonly TextBlock? login;
        private readonly TextBlock? content;
        private readonly TextBlock? dateTime;

        public MessageFrame(Message message)
        {
            if (message.MessageType == "text") 
            { 
                login = new TextBlock
                {
                    Text = message.Login,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Padding = new Thickness(5, 0, 5, 0),
                    FontWeight = FontWeights.Bold
                };

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

                frameGrid.Children.Add(login);
                Grid.SetRow(login, 0);

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

                Content = frame;
            }
        }
    }
}
