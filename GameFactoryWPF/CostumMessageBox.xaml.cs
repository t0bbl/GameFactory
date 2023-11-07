using System.Windows;

namespace GameFactoryWPF
{
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox(string message)
        {
            InitializeComponent();
            MessageText.Text = message;
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;

        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }

}