using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Collections.Generic;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace CSharp1.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class ShopPage : Page
    {
        private bool dbexisted= false;

        public ShopPage()
        {
            this.InitializeComponent();
        }
        private async void btn_username_Click(object sender, RoutedEventArgs e)
        {
            CommonData.Database = "myDBSeqAccounts";
            CommonData.SetConnection();   //CONNECT TO MS SQL SERVER
            List<string> thelist = GetDatabaseList();     //// GET ALL DATABASES ON SERVER
            String username = txtbox_username.Text;
            String password = pwdbox_username.Password;
            bool pass = false;
            Debug.WriteLine(username);
            //DBConnection myCon = new DBConnection();
            //myCon.establishConnection();
            //CommonData.SetConnection();  // Connect TO SERVER   
            SqlCommand myCommand = new SqlCommand();
            SqlDataReader myReader = null;

            try
            {
                //Commandtype --> Stored Procedure + Name der Prozedur angeben
                myCommand.Connection = CommonData.MyCon.MyCon;
              //  myCommand.CommandType = CommandType.StoredProcedure;
             //   myCommand.CommandText = "dbo.getuseraccounts";   //name of stored Procedure
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "SELECT Username=Username, Passw=Passw FROM USERAccounts";
               // myCommand.CommandType = CommandType.Text;
              //  myCommand.CommandText = "SELECT * FROM USERData";
                //Falls nötig Parameter hinzufuegen
                //  myCommand.Parameters.Add("@Username", SqlDbType.VarChar).Direction = ParameterDirection.Output;
                // myCommand.Parameters.Add("@Passw", SqlDbType.VarChar).Direction = ParameterDirection.Output;
                //Parameter hinzufuegen!!
                CommonData.MyCon.openConnectionAsync();
                //ExecuteNonQuery --> fuehrt unsere Prozedur aus
                // myCommand.ExecuteNonQuery();
                myReader = myCommand.ExecuteReader();
                
                //Zeile für Zeile wird in ein Listviewitem geschrieben und ausgegeben
                while (myReader.Read() && pass == false)
                {
                    //ListViewItem item = new ListViewItem(myReader[1].ToString());
                    // item.SubItems.Add(myReader[2].ToString());
                    // item.SubItems.Add(myReader[3].ToString());

                    //this.listView_person.Items.Add(item);
                    Console.WriteLine(myReader[0].ToString() + "" + myReader[1].ToString());
                    if (username == myReader[0].ToString() && password == myReader[1].ToString())
                    {
                        pass = true;
                        MessageDialog dialog = new MessageDialog("You are logged in as " + username +".", "LOGIN SUCCESS ");
                        await dialog.ShowAsync();

                        Frame navigationFrame = this.Frame as Frame;
                        navigationFrame.Navigate(typeof(BlankPage1));
                        Frame appFrame = Window.Current.Content as Frame;
                        MainPage mainPage = appFrame.Content as MainPage;   
                        mainPage.SetSelectedNavigationItem(0);
                    }
                }
                //  string uname = Convert.ToString(myCommand.Parameters["@Username"].Value);
                // string passw = Convert.ToString(myCommand.Parameters["@Passw"].Value);
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message);
                MessageDialog dialog = new MessageDialog("FAIL!!", "Information " + ex.Message);
                await dialog.ShowAsync();
            }
            finally
            {
                if (CommonData.MyCon.MyCon != null)
                    CommonData.MyCon.closeConnection();
            }
            if (pass)
            {
                foreach (var db in thelist)
                {
                    Debug.WriteLine(db);
                    if (username == db) dbexisted = true;
                }
                if (dbexisted)
                {
                    CommonData.Database = username;
                    CommonData.MyCon.setConnectionString();
                    CommonData.MyCon.establishConnectionAsync();
                }
                if (!dbexisted)
                {
                    create_dbAsync(username);
                    CommonData.Database = username;
                    CommonData.MyCon.setConnectionString();
                    CommonData.MyCon.establishConnectionAsync();
                }
                //
         }
            if (!pass)
            {
                CommonData.Database = "myDBSeqAccounts";
                CommonData.MyCon.setConnectionString();
                CommonData.MyCon.establishConnectionAsync();
            }
        }
        private async void CreateAccountLinkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame navigationFrame = this.Frame as Frame;
            navigationFrame.Navigate(typeof(CreateAccountPage));
            Frame appFrame = Window.Current.Content as Frame;
            MainPage mainPage = appFrame.Content as MainPage;
            mainPage.SetSelectedNavigationItem(3);
        }
        private async System.Threading.Tasks.Task create_dbAsync(string username)
        {
            SqlCommand myCommand = new SqlCommand();
            // SqlDataReader myReader = null;
            try
            {
                myCommand.Connection = CommonData.MyCon.MyCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "CREATE DATABASE " + username; //GET TABLES IN DATABASE
                CommonData.MyCon.openConnectionAsync();
                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                MessageDialog dialog = new MessageDialog("FAIL!!", "Information " + ex.Message);
                await dialog.ShowAsync();
            }
            finally
            {
                if (CommonData.MyCon.MyCon != null)
                    CommonData.MyCon.closeConnection();
            }
        }
            public List<string> GetDatabaseList()
        {
            List<string> list = new List<string>();

            SqlCommand myCommand = new SqlCommand();
            SqlDataReader myReader = null;

            bool tableexisted = false;                        
                myCommand.Connection = CommonData.MyCon.MyCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "SELECT name from sys.databases"; //GET TABLES IN DATABASE
                CommonData.MyCon.openConnectionAsync();
            {
                using (IDataReader dr = myCommand.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(dr[0].ToString());
                    }
                }
            }
            if (CommonData.MyCon.MyCon != null)
                CommonData.MyCon.closeConnection();
            return list;
        }
    }
}
