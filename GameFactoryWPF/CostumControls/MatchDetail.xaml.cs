using CoreGameFactory.Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ClassLibrary;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for MatchDetail.xaml
    /// </summary>
    public partial class MatchDetail : UserControl
    {
        List<Move> Moves;
        private TabControl MoveHistory;
        public MatchDetail(List<Move> p_Moves, Match p_Match)
        {
            InitializeComponent();
            CloseButton.Click += CloseButton_Click;
            BackButton.Click += BackButton_Click;
            ForwardButton.Click += ForwardButton_Click;
            ShowMatchDetail(p_Moves, p_Match);
        }

        private void ShowMatchDetail(List<Move> p_Moves, Match p_Match)
        {
            MoveHistory = new TabControl
            {
                Style = (Style)FindResource("HiddenTabsStyle")
            };

            foreach (var move in p_Moves)
            {
                var moveHistoryItem = new TabItem();

                var historyGameWindow = new GameWindow();

                moveHistoryItem.Content = historyGameWindow.CreatePlayboard(p_Match);
                MoveHistory.Items.Add(moveHistoryItem);
            }
            Grid.SetRow(MoveHistory, 0);
            Grid.SetColumnSpan(MoveHistory, 3);
            MoveHistoryGrid.Children.Add(MoveHistory);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MoveHistory.SelectedIndex > 0)
                MoveHistory.SelectedIndex--;
        }
        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (MoveHistory.SelectedIndex < MoveHistory.Items.Count - 1)
                MoveHistory.SelectedIndex++;
        }
    }
}
