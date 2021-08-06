using System;
using System.Collections.Generic;
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
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data;

namespace MESJobCloser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OracleCommand com = new OracleCommand();
        OracleConnection con = new OracleConnection();
        public MainWindow()
        {
            InitializeComponent();
            var dataAccesCon = new DataAccess();
            con = dataAccesCon.SetConnectionString();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Checks the username/password exists in the database, handled case sensitivity issues on the username
            string Username = txtUsername.Text.Trim();
            string Password = txtPassword.Password.Trim();
            if (con == null)
            {
                MessageBox.Show("Connection value incorrect. Connection could not be established", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            else

            con.Open();
            com.Connection = con;
            var oracleQuery = $"Select * from TBLCONSOLEUSER WHERE UPPER(USERNAME)='{Username.ToUpper()}' AND PASSWORD='{Password}'";
            var oracleCom = new OracleCommand(oracleQuery, con);


            try
            {
                OracleDataReader dr = oracleCom.ExecuteReader();
                if (dr.HasRows)
                {
                    MessageBox.Show("Login Successful", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                    DataWindow dataDisplay = new DataWindow();
                    dataDisplay.Show();
                    this.Close();
                    con.Close();

                }
                else
                {
                    MessageBox.Show("Username/Password Incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    con.Close();
                }
            }
            catch (Exception ex) { };
        }


        // below is logic to remove the default text from the username/password field when they are entered
        // also adds it back in if no input was given

        private void txtUserEnter(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text.Equals(@"Username"))
            {
                txtUsername.Text = "";
            }
        }

        private void txtUserLeave(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text.Equals(""))
            {
                txtUsername.Text = @"Username";
            }
        }

        private void txtPassEnter(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password.Equals("Password"))
            {
                txtPassword.Password = "";
            }
        }

        private void txtPassLeave(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password.Equals(""))
            {
                txtPassword.Password = "Password";
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            //closes the connection and closes the application
            if (con == null)
            {
                this.Close();
            }
            else
                con.Close();
            this.Close();

        }
    }
}

