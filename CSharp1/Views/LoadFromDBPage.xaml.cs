using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace CSharp1.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class LoadFromDBPage : Page
    {
        private static IDbCommand myCommand = null;
        public static IDbCommand MyCommand { get => myCommand; set => myCommand = value; }
        public LoadFromDBPage()
        {
            this.InitializeComponent();
            get_tables();
            SetsongListView();
        }

        public void SetsongListView()
        {
            songListView.Items.Clear();
            // foreach (string songs in CommonData.thesongs)
            for (var i = 0; i < CommonData.thesongs.Count; i++)
            {
              songListView.Items.Add(CommonData.thesongs[i]);
            }
            
        }
       private void btn_loadSong_Click(object sender, RoutedEventArgs e)
        {
            String my = songListView.SelectedItem.ToString();
            Debug.WriteLine(my);
            CommonData.SetConnection();
            delete_tableAsync(my);
            get_tables();
            SetsongListView();
            // CommonData.Mytablename = my;
            //BlankPage1.load_table();
        }
    


    private async System.Threading.Tasks.Task delete_tableAsync(string thetablename)
    {
          
       // SqlCommand myCommand = new SqlCommand();
       // SqlDataReader myReader = null;
            IDataReader myReader = null;
            try
        {
            myCommand.Connection = CommonData.MyCon.MyCon;
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "DROP TABLE " + thetablename +"";  //DROP TABLE
            CommonData.MyCon.openConnectionAsync();
            myReader = myCommand.ExecuteReader();
        }
        catch (Exception ex)
        {
           //  MessageBox.Show(ex.Message);
            MessageDialog dialog = new MessageDialog("FAIL!!", "Information " + ex.Message);
             await dialog.ShowAsync();
        }
        finally
        {
            if (CommonData.MyCon.MyCon != null)
                CommonData.MyCon.closeConnection();
        }
        //
    }

        public async Task get_tables()
        {
            // SqlCommand myCommand = new SqlCommand();
            //  IDbCommand myCommand = new SqlCommand();
            IDataReader myReader = null;
            // SqlDataReader myReader = null;

            bool tableexisted = false;
            try
            {
                CommonData.SetConnection();
                Debug.WriteLine("DATABASE C: " + CommonData.Database);
                //Commandtype --> Stored Procedure + Name der Prozedur angeben
                //  myCommand.CommandText = "SELECT * FROM [myDBSeqAccounts].[dbo].[USERData]";
                // myCommand.CommandText = "SELECT Username=Username, Passw=Passw FROM USERAccounts";
                myCommand.Connection = CommonData.MyCon.MyCon;
                myCommand.CommandType = CommandType.Text;
                if (CommonData.theconnection == 1)
                {
                    myCommand.CommandText = " SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG = '" + CommonData.Database + "'"; //GET TABLES IN DATABASE
                }
                else
                {
                    myCommand.CommandText = "SELECT name FROM sqlite_master WHERE type = 'table' AND name NOT LIKE 'sqlite_%';"; //GET TABLES IN  SQLITE DATABASE
                }
                CommonData.MyCon.openConnectionAsync();
                myReader = myCommand.ExecuteReader();
                if (CommonData.thesongs.Count() > 0) CommonData.thesongs.Clear();    //Clear the songs list to fill it again
                while (myReader.Read())
                {
                    Debug.WriteLine("HOHO" + myReader[0].ToString());

                    CommonData.setTheSongs(myReader[0].ToString());
                    if (CommonData.Mytablename == myReader[0].ToString()) { tableexisted = true; }
                }
                //Console.WriteLine("HOHO" + myReader[0].ToString());
                //Console.WriteLine(myReader[0].ToString() + "" + myReader[1].ToString());                       
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
                CommonData.MyCon.MyCon.Dispose();
                myReader.Dispose();
                GC.Collect();
            }

        }

    }

}
