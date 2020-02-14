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
        private string connectionString = "Connection Timeout=15; Data Source=EDVSR19-05\\AGSQLSERVER; Integrated Security= SSPI; Database= TestDB; ApplicationIntent= ReadWrite";

        //Connection zum Verbindungen (hat die Methode "Open")
        private SqlConnection myCon;

        public string Conn { get => conn; set => conn = value; }
        public string Conn1 { get => conn; set => conn = value; }
        public SqlConnection MyCon { get => myCon; set => myCon = value; }

        public void establishConnection()
        {
            myCon = new SqlConnection(connectionString);
            Console.WriteLine(Conn1);
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
