using CSharp1.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace CSharp1
{
    public static class DbCommandExtensionMethods
    {

        public static void AddParameter(this IDbCommand command, string name, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }
    }

    class DBConnection
    {
       public  enum ConnectionType
        {
            DBLocal = 0,
            DBRemote = 1,
           
        }


        private string conn;

        //Der Connection String enthält alle wichtigen Infos für die Verbindung
        private string datasource = "EDVSR19-05\\AGSQLSERVER";
        private string integratedSecurity = "SSPI";
        private string database="DefaultSeqDb";
        private string applicationIntent= "ReadWrite"; 
        private string connectionString = "Connection Timeout=15; Data Source=EDVSR19-05\\AGSQLSERVER; Integrated Security= SSPI; Database= DefaultSeqDb; ApplicationIntent= ReadWrite";

        //Connection zum Verbindungen (hat die Methode "Open")
        private IDbConnection myCon;
        private IDbCommand myCommand;
        // private SqlConnection myCon;
        private SQLiteConnection myConLite;

        public string Conn { get => conn; set => conn = value; }
        public string Conn1 { get => conn; set => conn = value; }
        
        public IDbConnection MyCon { get => myCon; set => myCon = value; }
        public IDbCommand MyCommand { get => myCommand; set => myCommand = value; }
        // public SqlConnection MyCon { get => myCon; set => myCon = value; }
        public SQLiteConnection MyConLite { get => myConLite; set => myConLite = value; }

        public string ConnectionString { get => connectionString; set => connectionString = value; }
        public string ApplicationIntent { get => applicationIntent; set => applicationIntent = value; }
        public string Database { get => database; set => database = value; }
        public string IntegratedSecurity { get => integratedSecurity; set => integratedSecurity = value; }
        public string Datasource { get => datasource; set => datasource = value; }
       

        private const string DatabaseFile = "databaseFile.db";
        public static  string DBName = ApplicationData.Current.LocalFolder.Path + @"\data.sqlite";
        private static string DatabaseSource = "data source=" + DBName;
       
        private static void Initialize()

        {

            // SQLLITE
            // Recreate database if already exists
            if (File.Exists(DBName))
            {
              //  File.Delete(DBName);
               // SQLiteConnection.CreateFile(DBName);
                Debug.WriteLine("RE: " + DBName);
            }
            else
            {
                SQLiteConnection.CreateFile(DBName);
                Debug.WriteLine("CREATE: " + DBName);
            }
        }
            public void setConnectionString()
        {
           // Datasource = "EDVSR19-05\\AGSQLSERVER";
           // IntegratedSecurity = "SSPI";
           // Database = "DefaultSeqDb";
           // ConnectionString = "Connection Timeout=5; Data Source="+ CommonData.Datasource +"; Integrated Security= "+IntegratedSecurity+"; Database= "+CommonData.Database+"; ApplicationIntent= "+ ApplicationIntent + "";
            ConnectionString = "Connection Timeout=5; Data Source="+ CommonData.Datasource + "; Integrated Security= " + IntegratedSecurity + "; Database= " + CommonData.Database+"; ApplicationIntent= "+ ApplicationIntent + "";
        }

        public async Task establishConnectionAsync()
        {
           
            {

                if (myCon != null)
                {
                    myCon.Close();
                }
                try
                {
                    // myCon = new SqlConnection(ConnectionString);
                    
                    myCon = GetDatabaseConnection((ConnectionType)CommonData.theconnection);
               //     myCon = GetDatabaseConnection(ConnectionType.DBLocal);
                   // myCon = GetDatabaseConnection(ConnectionType.DBRemote);
                }

                catch (Exception ex)
                {
                    // MessageBox.Show(ex.Message);
                //    MessageDialog dialog = new MessageDialog("DBCONNECTION FAIL!!", "Information " + ex.Message);
                  //  await dialog.ShowAsync();
                }
            }
        }
                //Console.WriteLine(Conn1);
            // myCon.Open();
        
        public async Task openConnectionAsync()
        {
            try
            {
                               MyCon.Open();
            }

            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                MessageDialog dialog = new MessageDialog("DBCONNECTION FAIL!!", "Information " + ex.Message);
                await dialog.ShowAsync();
            }
          
        }
        public void closeConnection()
        {
             MyCon.Close();
            // MyCon.Dispose();

        }
        public IDbConnection GetDatabaseConnection(ConnectionType db)
        {
            switch (db)
            {
                case ConnectionType.DBLocal:
                    Debug.WriteLine("Local DB");
                    BlankPage1.MyCommand = new SQLiteCommand();
                    CreateAccountPage.MyCommand = new SQLiteCommand();
                    SettingsPage.MyCommand = new SQLiteCommand();
                    LoadFromDBPage.MyCommand   = new SQLiteCommand();
                    ShopPage.MyCommand  = new SQLiteCommand();
                    MyCommand = new SQLiteCommand();
                    Initialize();
                    //SQLiteConnectionStringBuilder lcb = new SQLiteConnectionStringBuilder();
                    
                    return new SQLiteConnection(DatabaseSource + " ;" + "Version = 3; journal mode = OFF; SyncMode = NORMAL");
                    break;
                case ConnectionType.DBRemote:
                    Debug.WriteLine("REMOTE DB");
                    BlankPage1.MyCommand= new SqlCommand();         
                    CreateAccountPage.MyCommand = new SqlCommand();
                    SettingsPage.MyCommand = new SqlCommand();
                    LoadFromDBPage.MyCommand = new SqlCommand();
                    ShopPage.MyCommand = new SqlCommand();
                    MyCommand = new SqlCommand();
                    
                    return new SqlConnection(ConnectionString);
                    break;
                default:
                    Debug.WriteLine("default DB");
                    MyCommand = new SqlCommand();
                    return new SqlConnection(ConnectionString);

                    break;
            }
        }

    }


}
