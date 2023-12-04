using ClassLibrary;
using CoreGameFactory.Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for MatchDetail.xaml
    /// </summary>
    public partial class MatchDetail : UserControl
    {
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

            GameWindow HistoryGameWindow = new GameWindow();

            List<Grid> HistoricBoards = HistoryGameWindow.CreateHistoryPlayboard(p_Moves, p_Match);

            foreach (Grid HistoricBoard in HistoricBoards)
            {
                var MoveHistoryTab = new TabItem();
                MoveHistoryTab.Content = HistoricBoard;
                MoveHistory.Items.Add(MoveHistoryTab);
            }
            Grid.SetRow(MoveHistory, 0);
            Grid.SetColumnSpan(MoveHistory, 3);
            MoveHistoryGrid.Children.Add(MoveHistory);
            MoveHistory.SelectionChanged += MoveHistory_SelectionChanged;
        }


        private void MoveHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BackButton.Visibility = MoveHistory.SelectedIndex == 0 ? Visibility.Hidden : Visibility.Visible;
            ForwardButton.Visibility = MoveHistory.SelectedIndex == MoveHistory.Items.Count - 1 ? Visibility.Hidden : Visibility.Visible;
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
