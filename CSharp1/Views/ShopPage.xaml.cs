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
            List<string> thelist = GetDatabaseList();
            String username = txtbox_username.Text;
            String password = pwdbox_username.Password;
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
               
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "dbo.getuseraccounts";

               
                // myCommand.CommandText = "SELECT Username=Username, Passw=Passw FROM USERAccounts";
               // myCommand.CommandType = CommandType.Text;
              //  myCommand.CommandText = "SELECT * FROM USERData";
                //Falls nötig Parameter hinzufuegen
                //  myCommand.Parameters.Add("@Username", SqlDbType.VarChar).Direction = ParameterDirection.Output;
                // myCommand.Parameters.Add("@Passw", SqlDbType.VarChar).Direction = ParameterDirection.Output;
                //Parameter hinzufuegen!!



                CommonData.MyCon.openConnection();


                //ExecuteNonQuery --> fuehrt unsere Prozedur aus
                // myCommand.ExecuteNonQuery();
                myReader = myCommand.ExecuteReader();
                bool pass = false;
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
                        // ListWindow window = new ListWindow();
                        // window.Show();
                        MessageDialog dialog = new MessageDialog("LOGIN?", "LOGIN OK ");
                        await dialog.ShowAsync();
                       

                        // GET ALL DATABASES ON SERVER
                      
                        foreach (var db in thelist)
                        {
                            Debug.WriteLine(db);
                            if (username == db) dbexisted = true;
                        }
                        if (dbexisted)
                        {
                            CommonData.Database = username;
                            CommonData.MyCon.setConnectionString();
                            CommonData.MyCon.establishConnection();
                        }
                        //



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
            //Debug.WriteLine(GetDatabaseList());
         
        }


        public List<string> GetDatabaseList()
        {
            List<string> list = new List<string>();

            SqlCommand myCommand = new SqlCommand();
            SqlDataReader myReader = null;

            bool tableexisted = false;
          
                //Commandtype --> Stored Procedure + Name der Prozedur angeben
                //  myCommand.CommandText = "SELECT * FROM [TestDB].[dbo].[USERData]";
                // myCommand.CommandText = "SELECT Username=Username, Passw=Passw FROM USERAccounts";
                myCommand.Connection = CommonData.MyCon.MyCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "SELECT name from sys.databases"; //GET TABLES IN DATABASE
                CommonData.MyCon.openConnection();
              //  myReader = myCommand.ExecuteReader();


            // Set up a command with the given query and associate
            // this with the current connection.

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
