using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
using System.Windows.Shapes;
using Oracle.ManagedDataAccess.Client;

namespace MESJobCloser
{
    /// <summary>
    /// Interaction logic for DataWindow.xaml
    /// </summary>
    public partial class DataWindow : Window
    {

        OracleConnection con = new OracleConnection();
        //Creates the connection to Oracle
        //Opens new oracle command
        OracleCommand com = new OracleCommand();

        public string productionOrder = "";

        public DataWindow()
        {

            InitializeComponent();
            var dataAccesCon = new DataAccess();
            con = dataAccesCon.SetConnectionString();
            //Pulls the initial data into the grid when the window opens
            RetrieveDataGrid();
        }

        public DataTable RetrieveDataGrid()
            // Pulls the initial data into the grid for on job load and puts it in a table to present to the user
            // Returns the datatable to be used in the request method
        {
            con.Close();
            con.Open();
            com = con.CreateCommand();
            com.CommandText = "SELECT PONUMBER, WORKCENTRE, MATERIAL, QTYREQUIRED FROM TBLPRODUCTIONLIST WHERE STATUSID ='2' AND WORKCENTRE = 'PACKAUT1'";
            com.CommandType = System.Data.CommandType.Text;
            OracleDataReader dr = com.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            gridOracleData.ItemsSource = dt.DefaultView;
            dr.Close();
            return dt;
            
        }

        private void RequestDataGridUpdate()

            //Returns the data grid via the button to pull the data, produces an error if no records are found

        {
            var productionOrderTable = RetrieveDataGrid();
            
            if (productionOrderTable.Rows.Count == 0)
            {
                MessageBox.Show("No active order found for PACKAUT1", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            con.Close();
        }

        private void UpdateJobStatus(String sql_stmt)
        {
            // Changes the status of the order to status 4 and commits it to the database, then refreshes the grid data to reflect
            
            String msg = "Order Successfully Finished. Please Restart MES";

            if(productionOrder == "")
            {
                MessageBox.Show("No Order Selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else {
            if(con.State == ConnectionState.Closed)
                {
                    con.Open();
                }else
            com = con.CreateCommand();
            com.CommandText = sql_stmt;
            com.CommandType = CommandType.Text;
            com.ExecuteNonQuery();
            MessageBox.Show(msg, "Update Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            RetrieveDataGrid();
            productionOrder = "";
            }
            
        }


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void RetrieveData_Click(object sender, RoutedEventArgs e)
        {
            //retrieves the data manually via button on screen

            this.RequestDataGridUpdate();
            con.Close();
        }

        private void BntLogout_Click(object sender, RoutedEventArgs e)
        {
            //closes connection to the DB and closes the application down
            con.Close();
            MainWindow logonDisplay = new MainWindow();
            this.Close();
            logonDisplay.Show();
        }

        private void FinishOrder_Click(object sender, RoutedEventArgs e)
        {
            // calls method to change the status of the order to status 4 and commits it to the database, then refreshes the grid data to reflect

            String sql = $"UPDATE TBLPRODUCTIONLIST SET STATUSID = '4' WHERE STATUSID = '2' AND WORKCENTRE = 'PACKAUT1' AND PONUMBER = '{productionOrder}'";
            this.UpdateJobStatus(sql);
            con.Close();

        }

        private void gridOracleData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //pulls out the production order to pass into the status change method, this ensure we only close a production order that has been selected by the user

            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                productionOrder = dr["PONUMBER"].ToString();

            }
        }
    }
}
