using ClassLibrary;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;



namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public int Welcomed { get; set; } = 0;
        private string Username { get; set; }
        private string Password { get; set; }
        private string Password1 { get; set; }
        private string Password2 { get; set; }
        private int Ident { get; set; } = 0;
        private Player Player { get; set; }
        private string MessageText { get; set; }

        public Login()
        {
            InitializeComponent();
            this.MouseDown += MainWindow_MouseDown;

            LoginSection.Visibility = Visibility.Hidden;
            SignupSection.Visibility = Visibility.Hidden;
            LoginSection.RenderTransform = new TransformGroup()
            {
                Children = new TransformCollection()
                {
                new ScaleTransform() { ScaleX = -1 }
                }
            };
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Welcomed == 0)
            {
                Storyboard welcomeFlipOut = this.Resources["WelcomeFlipOut"] as Storyboard;
                if (welcomeFlipOut != null)
                {
                    welcomeFlipOut.Completed += WelcomeFlipOut_Completed;
                    welcomeFlipOut.Begin();
                }

                Welcomed = 1;
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Username = UsernameTextBox.Text;
            Password = PasswordTextBox.Password;
            if (!ClassLibrary.Player.ValidateLoginName(Username))
            {
                TextBox("Username must be between 3 and 16 characters long.");

                return;
            }

            if (Username == null || Password == null)
            {
                TextBox("Username or Password is not valid, please try again!");
                return;
            }
            string PasswordSave = Player.HashPassword(Password);

            try { Ident = ClassLibrary.Player.SQLLoginPlayer(Username, PasswordSave); }
            catch (Exception ex)
            {
                TextBox(ex.Message);
            }

            if (Ident != 0)
            {
                TextBox("Logged in as " + Username + " !");

                //Player = ClassLibrary.DataProvider.GetPlayerVariables(p_Ident);

            }
            else
            {
                TextBox("Wrong UserName or Password, Please try again or Signup!");
            };
        }
        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login_Click(sender, e);
            }
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            Storyboard LoginFlipOut = this.Resources["LoginFlipOut"] as Storyboard;
            if (LoginFlipOut != null)
            {
                LoginFlipOut.Completed += LoginFlipOut_Completed;
                LoginFlipOut.Begin();
            }
        }
        private void Signup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SignUpFunction_Click(sender, e);
            }
        }

        private void Guest_Click(object sender, RoutedEventArgs e)
        {
            TextBox("Logged in as Guest!");
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Storyboard SignupFlipOut = this.Resources["SignupFlipOut"] as Storyboard;
            if (SignupFlipOut != null)
            {
                SignupFlipOut.Completed += SignupFlipOut_Completed;
                SignupFlipOut.Begin();
            }
        }
        private void SignUpFunction_Click(object sender, RoutedEventArgs e)
        {
            Username = UsernameTextBoxSignup.Text;
            if (!ClassLibrary.Player.ValidateLoginName(Username))
            {
                MessageBox.Show("Username is not valid, please try again!");
                return;
            }
            if (!ClassLibrary.DataProvider.ValidateLoginNameForSignup(Username))
            {
                MessageBox.Show("Username is already taken, please try again!");
                return;
            }
            Password1 = PasswordTextBoxSignup1.Password;
            Password2 = PasswordTextBoxSignup2.Password;
            if (Password1 == Password2 && Player.ValidatePassword(Password1))
            {
                string PasswordSave = Player.HashPassword(Password1);
                Ident = Player.SQLSignUpPlayer(Username, PasswordSave);
                if (Ident != 0)
                {
                    MessageBox.Show("Signed up as " + Username + " ! Continue to LogIn");
                    Back_Click(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Passwords do not match or are not valid, please try again!");
                return;
            }
        }


        private void LoginFlipOut_Completed(object sender, EventArgs e)
        {
            LoginSection.Visibility = Visibility.Collapsed;

            SignupSection.Visibility = Visibility.Visible;

            Storyboard SignupFlipIn = this.Resources["SignupFlipIn"] as Storyboard;
            if (SignupFlipIn != null)
            {
                SignupFlipIn.Begin();

            }
        }
        private void WelcomeFlipOut_Completed(object sender, EventArgs e)
        {
            WelcomeSection.Visibility = Visibility.Collapsed;

            LoginSection.Visibility = Visibility.Visible;

            Storyboard loginFlipIn = this.Resources["LoginFlipIn"] as Storyboard;
            if (loginFlipIn != null)
            {
                loginFlipIn.Begin();

            }
        }
        private void SignupFlipOut_Completed(object sender, EventArgs e)
        {
            SignupSection.Visibility = Visibility.Collapsed;

            LoginSection.Visibility = Visibility.Visible;

            Storyboard loginFlipIn = this.Resources["LoginFlipIn"] as Storyboard;
            if (loginFlipIn != null)
            {
                loginFlipIn.Begin();

            }
        }

        private void TextBox(string p_Text)
        {
            CustomMessageBox customMessageBox = new CustomMessageBox(p_Text);
            customMessageBox.Owner = Application.Current.MainWindow;
            customMessageBox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            customMessageBox.ShowDialog();
        }

    }
}
