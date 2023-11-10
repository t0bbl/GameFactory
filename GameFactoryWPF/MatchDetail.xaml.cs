using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLibrary;
using CoreGameFactory.Model;

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
            MoveHistoryList.Items.Clear();
            MoveHistoryList.ItemsSource = p_Moves;
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
