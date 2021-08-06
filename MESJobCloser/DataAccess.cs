using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Oracle.ManagedDataAccess.Client;

namespace MESJobCloser
{
    public class DataAccess
    {

        // defines the connecton to use for the oracle connection
        string currentEnvironment = ConfigurationManager.AppSettings.Get("currentEnvironment").ToUpper();

        // Creates a new oracle command

        OracleConnection con;
        

        public OracleConnection SetConnectionString()
        {
            if (currentEnvironment == "PROD")
            {
                con = new OracleConnection(ConfigurationManager.ConnectionStrings["PROD"].ConnectionString);
            }
            else if (currentEnvironment == "PREPROD")
            {
                con = new OracleConnection(ConfigurationManager.ConnectionStrings["PREPROD"].ConnectionString);
            }
            else if (currentEnvironment == "QA")
            {
                con = new OracleConnection(ConfigurationManager.ConnectionStrings["QA"].ConnectionString);
            }
            else if (currentEnvironment == "DEV")
            {
                con = new OracleConnection(ConfigurationManager.ConnectionStrings["DEV"].ConnectionString);
            }
            else
            {
                MessageBox.Show("Connection value incorrect. Connection will not be established", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return con;
        }
        
    }
}
