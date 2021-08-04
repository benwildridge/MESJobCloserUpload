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
        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["dev"].ConnectionString);
        OracleCommand com = new OracleCommand();
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string Username = txtUsername.Text.Trim();
            string Password = txtPassword.Password.Trim();
            con.Open();
            com.Connection = con;
            var oracleQuery = $"Select * from TBLCONSOLEUSER WHERE UPPER(USERNAME)='{Username.ToUpper()}' AND PASSWORD='{Password}'";
            var test = new OracleCommand(oracleQuery,con);
            
            
            try { OracleDataReader dr = test.ExecuteReader(); 
                if(dr.HasRows)
                {
                    MessageBox.Show("Login Successful", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                    DataWindow dataDisplay = new DataWindow();
                    dataDisplay.Show();
                    this.Close();
                    
                }
                else
                {
                    MessageBox.Show("Username/Password Incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    con.Close();
                }
            }
            catch (Exception ex) { };
        }




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
            con.Close();
            this.Close();
        }
    }
}

