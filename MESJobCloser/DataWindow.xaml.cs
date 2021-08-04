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

        OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["dev"].ConnectionString);
        OracleCommand com = new OracleCommand();

        public string productionOrder = "";

        public DataWindow()
        {
            InitializeComponent();
            RetrieveDataGrid();
        }

        public DataTable RetrieveDataGrid()
        {
            con.Close();
            con.Open();
            com = con.CreateCommand();
            com.CommandText = "SELECT PONUMBER, WORKCENTRE, MATERIAL, MATERIALDESCRIPTION, QTYREQUIRED FROM TBLPRODUCTIONLIST WHERE STATUSID ='2' AND WORKCENTRE = 'PACKAUT1'";
            com.CommandType = System.Data.CommandType.Text;
            OracleDataReader dr = com.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            gridOracleData.ItemsSource = dt.DefaultView;
            dr.Close();
            return dt;
        }

        private void RequestDataGridUpdate()
        {
            var productionOrderTable = RetrieveDataGrid();
            
            if (productionOrderTable.Rows.Count == 0)
            {
                MessageBox.Show("No active order found for PACKAUT1", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void UpdateJobStatus(String sql_stmt)
        {
            
            String msg = "Order Successfully Finished. Refresh MES Screen.";

            if(productionOrder == "")
            {
                MessageBox.Show("No Order Selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else { 

            com = con.CreateCommand();
            com.CommandText = sql_stmt;
            com.CommandType = CommandType.Text;
            com.ExecuteNonQuery();
            MessageBox.Show(msg, "Update Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            RetrieveDataGrid();
            }
        }


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void RetrieveData_Click(object sender, RoutedEventArgs e)
        {
            this.RequestDataGridUpdate();
        }

        private void BntLogout_Click(object sender, RoutedEventArgs e)
        {
            con.Close();
            this.Close();
        }

        private void FinishOrder_Click(object sender, RoutedEventArgs e)
        {
            String sql = $"UPDATE TBLPRODUCTIONLIST SET STATUSID = '4' WHERE STATUSID = '2' AND WORKCENTRE = 'PACKAUT1' AND PONUMBER = '{productionOrder}'";
            this.UpdateJobStatus(sql);
            con.Close();
        }

        private void gridOracleData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                productionOrder = dr["PONUMBER"].ToString();

            }
        }
    }
}
