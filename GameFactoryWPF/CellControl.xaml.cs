using ClassLibrary;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for CellControl.xaml
    /// </summary>
    public partial class CellControl : UserControl
    {
        public static readonly DependencyProperty CellContentProperty =
            DependencyProperty.Register("CellContent", typeof(string), typeof(CellControl));

        public event EventHandler<CellClickedEventArgs> CellClicked;

        public int Row { get; set; }
        public int Column { get; set; }

        public CellControl()
        {
            InitializeComponent();
        }






    }

}
