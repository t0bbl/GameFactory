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
        /// <summary>
        /// Initializes a new instance of the MatchDetail class with the provided moves and match data.
        /// Sets up UI elements and binds event handlers for interaction.
        /// </summary>
        /// <param name="p_Moves">List of moves made during the match.</param>
        /// <param name="p_Match">The match for which the detail is displayed.</param>
        public MatchDetail(List<Move> p_Moves, Match p_Match)
        {
            InitializeComponent();
            CloseButton.Click += CloseButton_Click;
            BackButton.Click += BackButton_Click;
            ForwardButton.Click += ForwardButton_Click;
            ShowMatchDetail(p_Moves, p_Match);
        }

        /// <summary>
        /// Displays the match details, including a history of moves in a Game format.
        /// </summary>
        /// <param name="p_Moves">List of moves made during the match.</param>
        /// <param name="p_Match">The match for which the detail is displayed.</param>
        private void ShowMatchDetail(List<Move> p_Moves, Match p_Match)
        {
            MoveHistory = new TabControl
            {
                Style = (Style)FindResource("HiddenTabsStyle"),

            };

            GameWindow HistoryGameWindow = new GameWindow();

            List<Grid> HistoricBoards = HistoryGameWindow.CreateHistoryPlayboard(p_Moves, p_Match);

            foreach (Grid HistoricBoard in HistoricBoards)
            {
                var MoveHistoryTab = new TabItem();
                MoveHistoryTab.Content = HistoricBoard;
                MoveHistory.Items.Add(MoveHistoryTab);
            }
            MoveHistory.SelectionChanged += MoveHistory_SelectionChanged;
            HistoryBoard.Content = MoveHistory;
        }

        #region EventHandlers
        /// <summary>
        /// Handles the selection change of the move history tabs, updating the visibility of navigation buttons.
        /// </summary>
        private void MoveHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BackButton.Visibility = MoveHistory.SelectedIndex == 0 ? Visibility.Hidden : Visibility.Visible;
            ForwardButton.Visibility = MoveHistory.SelectedIndex == MoveHistory.Items.Count - 1 ? Visibility.Hidden : Visibility.Visible;
        }
        /// <summary>
        /// Closes the match detail view when the close button is clicked.
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// Navigates to the previous move in the move history when the back button is clicked.
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MoveHistory.SelectedIndex > 0)
                MoveHistory.SelectedIndex--;
        }
        /// <summary>
        /// Navigates to the next move in the move history when the forward button is clicked.
        /// </summary>
        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (MoveHistory.SelectedIndex < MoveHistory.Items.Count - 1)
                MoveHistory.SelectedIndex++;
        }
        #endregion
    }
}
