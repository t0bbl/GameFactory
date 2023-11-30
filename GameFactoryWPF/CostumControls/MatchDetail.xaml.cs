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

        public MatchDetail(List<Move> p_Moves)
        {
            InitializeComponent();
            CloseButton.Click += CloseButton_Click;
            MoveHistoryList.Items.Clear();
            MoveHistoryList.ItemsSource = p_Moves;
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
