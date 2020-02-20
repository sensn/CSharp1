using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CSharp1.Views
{
    public sealed partial class CreateAccountPage : Page
    {
        private bool dbexisted = false;
        String username = "";
        String password = "";
        public CreateAccountPage()
        {
            this.InitializeComponent();
        }
        private async void btn_username_Click(object sender, RoutedEventArgs e)
        {
            CommonData.Database = "myDBSeqAccounts";
            CommonData.SetConnection();   //CONNECT TO MS SQL SERVER
            List<string> thelist = GetDatabaseList();     //// GET ALL DATABASES ON SERVER
            username = txtbox_username.Text;
            password = pwdbox_username.Password;
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

                //Zeile für Zeile wird in ein Listviewitem geschrieben und ausgegeben
                while (myReader.Read() && pass == false)
                {
                    //ListViewItem item = new ListViewItem(myReader[1].ToString());
                    // item.SubItems.Add(myReader[2].ToString());
                    // item.SubItems.Add(myReader[3].ToString());

                    //this.listView_person.Items.Add(item);
                    Console.WriteLine(myReader[0].ToString() + "" + myReader[1].ToString());
                    if (username == myReader[0].ToString())    //USERNAME already Existes?
                    {
                        pass = true;
                       // MessageDialog dialog = new MessageDialog("LOGIN!", "LOGIN OK ");
                      //  await dialog.ShowAsync();

                       
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
            ////
            if (pass)
            {
                MessageDialog dialog = new MessageDialog("User already exists! Choose a different username!");
                await dialog.ShowAsync();
            }
            if (!pass && username != "")
            {
                {
                    create_dbAsync(username);
                    CommonData.Database = username;
                    CommonData.MyCon.setConnectionString();
                    CommonData.MyCon.establishConnectionAsync();
                    MessageDialog dialog = new MessageDialog("Please Login!", "User Account created! Hello " + username + "!");
                    await dialog.ShowAsync();
                    Frame navigationFrame = this.Frame as Frame;
                    navigationFrame.Navigate(typeof(ShopPage));
                    Frame appFrame = Window.Current.Content as Frame;
                    MainPage mainPage = appFrame.Content as MainPage;
                    mainPage.SetSelectedNavigationItem(1);
                }
            }
        }
       private async void CreateAccountLinkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame navigationFrame = this.Frame as Frame;
            navigationFrame.Navigate(typeof(BlankPage1));
            Frame appFrame = Window.Current.Content as Frame;
            MainPage mainPage = appFrame.Content as MainPage;
            mainPage.SetSelectedNavigationItem(0);
        }
       private async System.Threading.Tasks.Task create_dbAsync(string username)
        {
             SqlCommand myCommand = new SqlCommand();
            // SqlDataReader myReader = null;
            try
            {
                CommonData.Database = "myDBSeqAccounts";
                CommonData.SetConnection();   //CONNECT TO MS SQL SERVER
                Debug.WriteLine("DATABASE C: " + CommonData.Database);
                SqlCommand mycommand = new SqlCommand();
                myCommand.Connection = CommonData.MyCon.MyCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "INSERT INTO USERAccounts(Username, Passw) VALUES(@Username, @Passw)";
                Debug.WriteLine("Command TEXT: " + myCommand.CommandText);
                //myCommand.CommandText = "INSERT INTO " + CommonData.mytablename + "(id)" + " VALUES (@id)";
                // myCommand.CommandText = "INSERT INTO " + CommonData.mytablename + "(id," + CommonData.theColummsRaw + ")" + " VALUES (@id," + CommonData.theColummsVal + ")";
                CommonData.MyCon.openConnection();

                myCommand.Parameters.Clear();
                myCommand.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;
                myCommand.Parameters.Add("@Passw", SqlDbType.NVarChar).Value = password;
                //myReader = myCommand.ExecuteReader();
                myCommand.ExecuteNonQuery();
                // CommonData.MyCon.closeConnection();
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                    MessageDialog dialog = new MessageDialog("FAIL!!", "Information " + ex.Message);
                    await dialog.ShowAsync();
                Debug.WriteLine("FAIL!!", "Information " + ex.Message);
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

            //Commandtype --> Stored Procedure + Name der Prozedur angeben
            //  myCommand.CommandText = "SELECT * FROM [myDBSeqAccounts].[dbo].[USERData]";
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


