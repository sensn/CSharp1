using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace CSharp1.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private bool DefaultDbExisted;
        private bool AccDbExisted;
        private IEnumerable<object> theDBlist;

        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private async void btn_connectToSQLServer_Click(object sender, RoutedEventArgs e)
        {
            if (txtbox_servername.Text != "")
            {
                CommonData.Datasource = txtbox_servername.Text;

                CommonData.Database = "master";
                CommonData.SetConnection();   //CONNECT TO MS SQL SERVER
                theDBlist = GetDatabaseList();
                foreach (var db in theDBlist)
                {
                    // Debug.WriteLine(db);
                    if ("myDBSeqAccounts" == db) AccDbExisted = true;
                    if ("DefaultSeqDb" == db) DefaultDbExisted = true;
                }
                if (!AccDbExisted)
                {
                    create_dbAsync("myDBSeqAccounts");
                    CommonData.Database = "myDBSeqAccounts";
                    create_dbTableAsync();
                }

                if (DefaultDbExisted)
                {
                    CommonData.Database = "DefaultSeqDb";
                }
                else
                {
                    create_dbAsync("DefaultSeqDb");
                    CommonData.Database = "DefaultSeqDb";

                }


                Debug.WriteLine("Servas Wöd, I brauch a göd! CREATE TASK");
                CommonData.Database = "DefaultSeqDb";   //Default Database
                CommonData.SetConnection();   //CONNECT TO MS SQL SERVER

            }
        }
        private async Task create_dbTableAsync()
        {
            SqlCommand myCommand = new SqlCommand();
            SqlDataReader myReader = null;

            bool tableexisted = false;
            try
            {
                CommonData.SetConnection();
                // Debug.WriteLine("DATABASE C: " + CommonData.Database);
                //Commandtype --> Stored Procedure + Name der Prozedur angeben
                //  myCommand.CommandText = "SELECT * FROM [DefaultSeqDb].[dbo].[USERData]";
                // myCommand.CommandText = "SELECT Username=Username, Passw=Passw FROM USERAccounts";
                myCommand.Connection = CommonData.MyCon.MyCon;
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = " SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG = '" + CommonData.Database + "'"; //GET TABLES IN DATABASE
                CommonData.MyCon.openConnection();
                myReader = myCommand.ExecuteReader();
                // if (CommonData.thesongs.Count() > 0) CommonData.thesongs.Clear();    //Clear the songs list to fill it again
                while (myReader.Read())
                {
                    Debug.WriteLine("HOHO" + myReader[0].ToString());

                    // CommonData.setTheSongs(myReader[0].ToString());
                    if (CommonData.Mytablename == myReader[0].ToString()) { tableexisted = true; }
                }
                Console.WriteLine("HOHO" + myReader[0].ToString());
                //Console.WriteLine(myReader[0].ToString() + "" + myReader[1].ToString());                       
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                //  MessageDialog dialog = new MessageDialog("FAIL!!", "Information " + ex.Message);
                // await dialog.ShowAsync();
            }
            finally
            {
                if (CommonData.MyCon.MyCon != null)
                    CommonData.MyCon.closeConnection();
            }
            if (!tableexisted)
            {
                try
                {
                    myCommand.Connection = CommonData.MyCon.MyCon;
                    myCommand.CommandType = CommandType.Text;
                    myCommand.CommandText = "CREATE TABLE " + "USERAccounts" + " (id INTEGER , Username varchar(50),Passw VARCHAR(50))"; //GET TABLES IN DATABASE
                    //myCommand.CommandText = "CREATE TABLE " + CommonData.Mytablename + " (id INTEGER , " + CommonData.theColumms + ")"; //GET TABLES IN DATABASE
                    CommonData.MyCon.openConnection();
                    myReader = myCommand.ExecuteReader();
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
        }


    }
} 
