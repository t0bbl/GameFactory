using System.Windows;
using System.Windows.Controls;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for CellControl.xaml
    /// </summary>
    public partial class CellControl : UserControl
    {
        // Define a DependencyProperty for binding
        public static readonly DependencyProperty CellContentProperty =
            DependencyProperty.Register("CellContent", typeof(string), typeof(CellControl));

        public string CellContent
        {
            get { return (string)GetValue(CellContentProperty); }
            set { SetValue(CellContentProperty, value); }
        }

        public CellControl()
        {
            InitializeComponent();
        }

        private void OnCellClicked(object sender, RoutedEventArgs e)
        {

            Button clickedButton = sender as Button;
            int row = Grid.GetRow(this);
            int col = Grid.GetColumn(this);



            MessageBox.Show($"You clicked cell {row}, {col}", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }

}
