using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LogInPage
{
    public class ListButton : Button
    {
        private readonly ClientWindow clientWindow;
        public TextBlock TitleName { get; set; } = new();
        public TextBlock UnderLine { get; set; } = new();

        public ListButton(ClientWindow cw, string titleName, string underLine) 
        {
            clientWindow = cw;

            Background = new SolidColorBrush(Color.FromRgb(229, 243, 253));
            Height = 60;
            BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 0, 0));

            Grid grid = new()
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                },
                HorizontalAlignment = HorizontalAlignment.Left, 
                VerticalAlignment = VerticalAlignment.Top
            };

            TitleName.Text = titleName;
            TitleName.HorizontalAlignment = HorizontalAlignment.Left;
            TitleName.VerticalAlignment = VerticalAlignment.Top;
            grid.Children.Add(TitleName);
            Grid.SetRow(TitleName, 0);

            UnderLine.Text = underLine;
            grid.Children.Add(UnderLine);
            Grid.SetRow(UnderLine, 1);

            Content = grid;

            Click += Button_Click;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            clientWindow.ChatFrame.Content = clientWindow.clientWindowChatFrame;
        }
    }
}
