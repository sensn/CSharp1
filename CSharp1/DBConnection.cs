using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp1
{
    class DBConnection
    {
       private string conn;

        //Der Connection String enthält alle wichtigen Infos für die Verbindung
        private string datasource = "EDVSR19-05\\AGSQLSERVER";
        private string integratedSecurity = "SSPI";
        private string database="DefaultSeqDb";
        private string applicationIntent= "ReadWrite"; 
        private string connectionString = "Connection Timeout=15; Data Source=EDVSR19-05\\AGSQLSERVER; Integrated Security= SSPI; Database= DefaultSeqDb; ApplicationIntent= ReadWrite";

        //Connection zum Verbindungen (hat die Methode "Open")
        private SqlConnection myCon;

        public string Conn { get => conn; set => conn = value; }
        public string Conn1 { get => conn; set => conn = value; }
        public SqlConnection MyCon { get => myCon; set => myCon = value; }
        public string ConnectionString { get => connectionString; set => connectionString = value; }
        public string ApplicationIntent { get => applicationIntent; set => applicationIntent = value; }
        public string Database { get => database; set => database = value; }
        public string IntegratedSecurity { get => integratedSecurity; set => integratedSecurity = value; }
        public string Datasource { get => datasource; set => datasource = value; }


        public void setConnectionString()
        {
           // Datasource = "EDVSR19-05\\AGSQLSERVER";
           // IntegratedSecurity = "SSPI";
           // Database = "DefaultSeqDb";
            ConnectionString = "Connection Timeout=15; Data Source="+Datasource+"; Integrated Security= "+IntegratedSecurity+"; Database= "+CommonData.Database+"; ApplicationIntent= "+ ApplicationIntent + "";
        }

        public void establishConnection()
        {
            myCon = new SqlConnection(ConnectionString);
            //Console.WriteLine(Conn1);
            // myCon.Open();
        }
        public void openConnection()
        {
            MyCon.Open();
        }
        public void closeConnection()
        {
             MyCon.Close();
        }


    }


}
