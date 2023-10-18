using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
            this.MouseDown += MainWindow_MouseDown;
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WelcomeSection.Visibility = Visibility.Collapsed;
            LoginSection.Visibility = Visibility.Visible;
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            MessageBox.Show("The button labeled '" + button.Content + "' has been clicked.");

        }
        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MessageBox.Show("The Login Event was triggerd with a Enter in Login/password.");
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            MessageBox.Show("The button labeled '" + button.Content + "' has been clicked.");
        }


    }
}
