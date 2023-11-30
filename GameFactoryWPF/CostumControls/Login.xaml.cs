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

        public delegate void PlayerLoggedInHandler(Player player);

        public event PlayerLoggedInHandler PlayerLoggedIn;

        /// <summary>
        /// Initializes the login window, sets up event handlers, and applies initial UI transformations.
        /// </summary>
        public Login()
        {
            InitializeComponent();
            this.MouseDown += MainWindow_MouseDown;
            ApplyInitialTransformations();
        }

        #region UI Event Handlers
        /// <summary>
        /// Handles the login process, validates user credentials, and manages the user session.
        /// </summary>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Username = UsernameTextBox.Text;
            Password = PasswordTextBox.Password;

            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || !Player.ValidateLoginName(Username))
            {
                TextBox("Username or Password is not valid, please try again!");
                return;
            }
            string PasswordSave = Player.HashPassword(Password);

            try { Ident = Player.SQLLoginPlayer(Username, PasswordSave); }
            catch (Exception ex)
            {
                TextBox(ex.Message);
            }

            if (Ident != 0)
            {
                Player = DataProvider.GetStatsAndVariables(Ident);

                OnPlayerLoggedIn(Player);

            }
            else
            {
                TextBox("Wrong UserName or Password, Please try again or Signup!");
                return;
            };
        }
        /// <summary>
        /// Allows a user to continue as a guest.
        /// </summary>
        private void Guest_Click(object sender, RoutedEventArgs e)
        {
            TextBox("Logged in as Guest!");
        }
        /// <summary>
        /// Handles the sign-up functionality, including user creation and validation.
        /// </summary>
        private void SignUpFunction_Click(object sender, RoutedEventArgs e)
        {
            Username = UsernameTextBoxSignup.Text;
            if (!Player.ValidateLoginName(Username))
            {
                MessageBox.Show("Username is not valid, please try again!");
                return;
            }
            if (!DataProvider.ValidateLoginNameForSignup(Username))
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
        #endregion

        #region Event Handlers Animations
        private void ApplyInitialTransformations()
        {
            LoginSection.Visibility = Visibility.Hidden;
            SignupSection.Visibility = Visibility.Hidden;
            LoginSection.RenderTransform = new TransformGroup
            {
                Children = new TransformCollection
        {
            new ScaleTransform { ScaleX = -1 }
        }
            };
        }
        /// <summary>
        /// Triggers the sign-up animation and process when the sign-up button is clicked.
        /// </summary>
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            Storyboard LoginFlipOut = this.Resources["LoginFlipOut"] as Storyboard;
            if (LoginFlipOut != null)
            {
                LoginFlipOut.Completed += LoginFlipOut_Completed;
                LoginFlipOut.Begin();
            }
        }
        /// <summary>
        /// Completes the login flip-out animation and updates UI visibility accordingly.
        /// </summary>
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
        /// <summary>
        /// Completes the welcome flip-out animation and updates UI visibility accordingly.
        /// </summary>
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
        /// <summary>
        /// Completes the sign-up flip-out animation and updates UI visibility accordingly.
        /// </summary>
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
        #endregion

        #region Helper Functions
        /// <summary>
        /// Handles mouse down events on the window. Triggers a storyboard animation when clicked for the first time.
        /// </summary>
        private void MainWindow_MouseDown(object Sender, MouseButtonEventArgs e)
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
        /// <summary>
        /// Initiates the login process when the Enter key is pressed.
        /// </summary>
        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login_Click(sender, e);
            }
        }
        /// <summary>
        /// Initiates the sign-up process when the Enter key is pressed within the sign-up section.
        /// </summary>
        private void Signup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SignUpFunction_Click(sender, e);
            }
        }
        /// <summary>
        /// Displays a custom message box with the provided text.
        /// </summary>
        /// <param name="p_Text">The message text to be displayed in the message box.</param>
        public static void TextBox(string p_Text)
        {
            CustomMessageBox customMessageBox = new CustomMessageBox(p_Text);
            customMessageBox.Owner = Application.Current.MainWindow;
            customMessageBox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            customMessageBox.ShowDialog();
        }
        /// <summary>
        /// Handles the back button click to revert to the previous UI state.
        /// </summary>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Storyboard SignupFlipOut = this.Resources["SignupFlipOut"] as Storyboard;
            if (SignupFlipOut != null)
            {
                SignupFlipOut.Completed += SignupFlipOut_Completed;
                SignupFlipOut.Begin();
            }
        }

        private void OnPlayerLoggedIn(Player player)
        {
            PlayerLoggedIn?.Invoke(player);
        }
        #endregion

    }
}
