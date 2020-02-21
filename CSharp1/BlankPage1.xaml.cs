using System;
using System.Collections.Generic;
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
using System.Windows;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.ApplicationModel.Background;
using System;
using System.Threading;
using Windows.UI.Core;

//***FÜR MIDI
using Windows.Devices.Enumeration;
using Windows.Devices.Midi;
using System.Threading.Tasks;
using Windows.UI.Popups;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Data.Common;


// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace CSharp1
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {

        public string sers = "Hello   OIDA  ";

        public const int numchannels = 10;
        public bool vis = true;
        public Dictionary<ToggleButton, Tuple<int, int>> clientDict = new Dictionary<ToggleButton, Tuple<int, int>>();
        public Dictionary<Slider, int> sliderclientDict = new Dictionary<Slider, int>();
        // public Brush Background { get; set; }
        public UniformGrid my = new UniformGrid();
        public UniformGrid my1 = new UniformGrid();

        public Room[] room = new Room[10];

        private static IDbCommand myCommand = null;
        public static IDbCommand MyCommand { get => myCommand; set => myCommand = value; }
       // public static IDbCommand MyKommand { get; internal set; }

        public ToggleButton[] channelSel = new ToggleButton[10];
        public ToggleButton[] saveSlot = new ToggleButton[10];
        public ToggleButton[] loadSlot = new ToggleButton[10];
        public Button[] prgButton = new Button[2];
        public Button[] bnkButton = new Button[2];
        public static Rectangle[] led = new Rectangle[16];

        TextBlock prgText;
        TextBlock bnkText;

        MyMidiDeviceWatcher inputDeviceWatcher;
        MyMidiDeviceWatcher outputDeviceWatcher;

        public static MidiInPort midiInPort;
        public static IMidiOutPort midiOutPort;

        int tabentry = 0;
        int numentries = 10;
        int activechannel = 0;

        //public Worker playsequence;
        public static Worker playsequence;
        public static bool midiset = false;
        private int tabentry_save;
        public ListView songListView;
        public ListView songListView1;
        public Flyout myflyout;
        public Flyout myflyout1;

        List<string> theDBlist;

        bool AccDbExisted = false;
        bool DefaultDbExisted = false;
        TextBox enter_songname;
        public BlankPage1()
        {
            InitializeComponent();
            // Add the following line of code.
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            bool connect = 1 == 1;
            if (connect) { 
            CommonData.Datasource = "EDVSR19-05\\AGSQLSERVER";
            CommonData.Database = "master";
            CommonData.SetConnection();   //CONNECT TO MS SQL SERVER
            //CommonData.SetConnection();   //CONNECT TO MS SQL SERVER
            theDBlist = GetDatabaseList();
            foreach (var db in theDBlist)
            {
                Debug.WriteLine(db);
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

            }
            Debug.WriteLine("Servas Wöd, I brauch a göd! CREATE TASK");
            CommonData.Database = "DefaultSeqDb";   //Default Database
            CommonData.SetConnection();   //CONNECT TO MS SQL SERVER
            
            CommonData.create_SQL_Strings();
         //   create_table();
            playsequence = new Worker();
            Worker.LogHandler myLogger = new Worker.LogHandler(sendMidiMessage);
            Worker.LogHandler2 myLogger2 = new Worker.LogHandler2(DoSomething);
            // Worker.LogHandler myLogger = new Worker.LogHandler(BlankPage1.sendMidiMessage);
            playsequence.myLogger1 = myLogger;
            playsequence.myLogger2 = myLogger2;

            //Action<object> action = (object obj) =>
            //{

            //    Console.WriteLine("Task={0}, obj={1}, Thread={2}",
            //    Task.CurrentId, obj,
            //    Thread.CurrentThread.ManagedThreadId);
            //    playsequence.mythread1();
            //};


            //Task t1 = new Task(action, "alpha");

            //t1.Start();
            //Console.WriteLine("t1 has been launched. (Main Thread={0})",
            //                  Thread.CurrentThread.ManagedThreadId);

            //Creating thread
            // Using thread class

            Thread thr = new Thread(new ThreadStart(playsequence.mythread1));
            thr.Priority = ThreadPriority.Highest;
            thr.Start();

            // DefaultLaunch();    //Launch a app from asociated filetype
            //****MIDI
            inputDeviceWatcher =
                  new MyMidiDeviceWatcher(MidiInPort.GetDeviceSelector(), midiInPortListBox, Dispatcher);

            inputDeviceWatcher.StartWatcher();

            outputDeviceWatcher =
                new MyMidiDeviceWatcher(MidiOutPort.GetDeviceSelector(), midiOutPortListBox, Dispatcher);

            outputDeviceWatcher.StartWatcher();
            //midiOutPortListBox.SelectedIndex = 0;
         
            //*****MIDI END

            // midiOutPortListBox.SelectedIndex = 0;

            //System.Threading.Tasks.Task task1 = new Task(DoSomething);
            //task1.Start();
            //task1.Factory.StartNew(DoSomething);

            // Creating object of ExThread class 


            ButtonsUniformGrid.Visibility = Visibility.Visible;
            ButtonsUniformGrid_Copy.Orientation = Orientation.Horizontal;
            ButtonsUniformGrid_Copy.Columns = 16;
            ButtonsUniformGrid_Copy.Rows = 4;
            Slider thebpmSlider = new Slider();
            thebpmSlider.Orientation = Orientation.Vertical;
            thebpmSlider.HorizontalAlignment = HorizontalAlignment.Stretch;
            thebpmSlider.VerticalAlignment = VerticalAlignment.Stretch;
            thebpmSlider.Header = "BPM";
            thebpmSlider.ValueChanged += thebpmSlider_ValueChanged;
            for (int i = 0; i < numchannels; i++)
            {
                room[i] = new Room();
                room[i].channel = i;
            

                thegrid.Children.Add(room[i].uniformGrid1);
                thegrid.Children.Add(room[i].uniformGrid2);
                thegrid.Children.Add(room[i].uniformGrid3);

                Grid.SetColumn(room[i].uniformGrid1, 1);     //ToggleButtonMatrix
                Grid.SetRow(room[i].uniformGrid1, 1);
                Grid.SetColumn(room[i].uniformGrid2, 2);     //Sliders
                Grid.SetRow(room[i].uniformGrid2, 1);  
                Grid.SetColumn(room[i].uniformGrid3, 0);     //Sliders
                Grid.SetRow(room[i].uniformGrid3, 1);
                //  Grid.SetRowSpan(room[i].uniformGrid2, 2);    //Slider Stretch Vertically

                //****
                channelSel[i] = new ToggleButton();
                channelSel[i].HorizontalAlignment = HorizontalAlignment.Stretch;
                channelSel[i].VerticalAlignment = VerticalAlignment.Stretch;
                channelSel[i].Checked += HandleChannelSelChecked;
                channelSel[i].Unchecked += HandleChannelSelUnChecked;
                channelSel[i].Tag = i;


                ButtonsUniformGrid_Copy.Children.Add(channelSel[i]);
            }
            //channelSel[0].IsChecked = true;
            playsequence.settherooms(room);
            for (int i = 0; i < 16; i++)
            {
                led[i] = new Rectangle();
                led[i].Fill = new SolidColorBrush(Windows.UI.Colors.Black);  //SAPCER
                LedUniformGrid.Children.Add(led[i]);
            }
            led[3].Fill = new SolidColorBrush(Windows.UI.Colors.DarkRed);  //SAPCER


            for (int i = 0; i < 1; i++)   //
            {
                Border myspacer1 = new Border();
                myspacer1.Background = new SolidColorBrush(Windows.UI.Colors.Black);  //SAPCER
                ButtonsUniformGrid_Copy.Children.Add(myspacer1);
            }
            prgText = new TextBlock();
            prgText.Text = "0";
            prgText.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            // prgText.Width = 240;
            prgText.IsTextSelectionEnabled = true;
            // prgText.TextWrapping = TextWrapping.Wrap;
            prgText.HorizontalAlignment = HorizontalAlignment.Center;
            prgText.VerticalAlignment = VerticalAlignment.Center;
           
            ButtonsUniformGrid_Copy.Children.Add(prgText);
            for (int i = 0; i < 2; i++)
            {
                prgButton[i] = new Button();
                prgButton[i].HorizontalAlignment = HorizontalAlignment.Stretch;
                prgButton[i].VerticalAlignment = VerticalAlignment.Stretch;
                prgButton[i].Click += HandleprgButtonClicked;
                //   saveSlot[i].Unchecked += HandleChannelSelUnChecked;
                prgButton[i].Tag = i;
                ButtonsUniformGrid_Copy.Children.Add(prgButton[i]);
            }
            prgButton[0].Content = "prg+";
            prgButton[1].Content = "prg-";

            Border myspacer2 = new Border();
            myspacer2.Background = new SolidColorBrush(Windows.UI.Colors.Black);  //SAPCER
            ButtonsUniformGrid_Copy.Children.Add(myspacer2);

            ToggleButton playbutton = new ToggleButton();
            playbutton.HorizontalAlignment = HorizontalAlignment.Stretch;
            playbutton.VerticalAlignment = VerticalAlignment.Stretch;
            playbutton.Click += HandleplayButtonClicked;
            ButtonsUniformGrid_Copy.Children.Add(playbutton);
            for (int i = 0; i < 11; i++)
            {
                Border myspacer1 = new Border();
                myspacer1.Background = new SolidColorBrush(Windows.UI.Colors.Black);  //SAPCER
                ButtonsUniformGrid_Copy.Children.Add(myspacer1);
            }
            bnkText = new TextBlock();
            bnkText.Text = "0";
            bnkText.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            bnkText.HorizontalAlignment = HorizontalAlignment.Center;
            bnkText.VerticalAlignment = VerticalAlignment.Center;
            // bnkText.Width = 240;
            bnkText.IsTextSelectionEnabled = true;
            bnkText.TextWrapping = TextWrapping.Wrap;
            ButtonsUniformGrid_Copy.Children.Add(bnkText);
            for (int i = 0; i < 2; i++)
            {
                bnkButton[i] = new Button();
                bnkButton[i].HorizontalAlignment = HorizontalAlignment.Stretch;
                bnkButton[i].VerticalAlignment = VerticalAlignment.Stretch;
                bnkButton[i].Click += HandlebnkButtonClicked;
                //   saveSlot[i].Unchecked += HandleChannelSelUnChecked;
                bnkButton[i].Tag = i;
                ButtonsUniformGrid_Copy.Children.Add(bnkButton[i]);
            }
            bnkButton[0].Content = "bnk+";
            bnkButton[1].Content = "bnk-";

            Button savePattern = new Button();
            savePattern.HorizontalAlignment = HorizontalAlignment.Stretch;
            savePattern.VerticalAlignment = VerticalAlignment.Stretch;
            savePattern.Click += HandlesavePatternChecked;
            //savePattern.Unchecked += HandleChannelSelUnChecked;
            ButtonsUniformGrid_Copy1.Children.Add(savePattern);

            Border myspacer = new Border();
            myspacer.Background = new SolidColorBrush(Windows.UI.Colors.Black);  //SAPCER
            ButtonsUniformGrid_Copy1.Children.Add(myspacer);

            for (int i = 0; i < numchannels; i++)
            {
                saveSlot[i] = new ToggleButton();
                saveSlot[i].HorizontalAlignment = HorizontalAlignment.Stretch;
                saveSlot[i].VerticalAlignment = VerticalAlignment.Stretch;
                saveSlot[i].Checked += HandlesaveSlotChecked;
                //   saveSlot[i].Unchecked += HandleChannelSelUnChecked;
                saveSlot[i].Tag = i;
                ButtonsUniformGrid_Copy1.Children.Add(saveSlot[i]);
            }
           // saveSlot[0].IsChecked = true;
            for (int i = 0; i < 3; i++)
            {
                Border myspacer1 = new Border();
                myspacer1.Background = new SolidColorBrush(Windows.UI.Colors.Black);  //SPACER
                ButtonsUniformGrid_Copy1.Children.Add(myspacer1);
            }
            Button saveTODBButton = new Button();
            saveTODBButton.HorizontalAlignment = HorizontalAlignment.Stretch;
            saveTODBButton.VerticalAlignment = VerticalAlignment.Stretch;
            saveTODBButton.Click += HandlesaveTODBButtonClick;
            //saveTODBButton.Unchecked += HandleChannelSelUnChecked;
            ButtonsUniformGrid_Copy1.Children.Add(saveTODBButton);

            for (int i = 0; i < 2; i++)
            {
                Border myspacer1 = new Border();
                myspacer1.Background = new SolidColorBrush(Windows.UI.Colors.Black);  //SAPCER
                ButtonsUniformGrid_Copy1.Children.Add(myspacer1);
            }

            for (int i = 0; i < numchannels; i++)
            {
                loadSlot[i] = new ToggleButton();
                loadSlot[i].HorizontalAlignment = HorizontalAlignment.Stretch;
                loadSlot[i].VerticalAlignment = VerticalAlignment.Stretch;
                loadSlot[i].Checked += HandleloadSlotChecked;
                //   loadSlot[i].Unchecked += HandleChannelSelUnChecked;
                loadSlot[i].Tag = i;
                ButtonsUniformGrid_Copy1.Children.Add(loadSlot[i]);
            }
            //loadSlot[0].IsChecked = true;
            for (int i = 0; i < 3; i++)
            {
                Border myspacer1 = new Border();
                myspacer1.Background = new SolidColorBrush(Windows.UI.Colors.Black);  //SAPCER
                ButtonsUniformGrid_Copy1.Children.Add(myspacer1);
            }
            Button loadFromDBButton = new Button();
            loadFromDBButton.HorizontalAlignment = HorizontalAlignment.Stretch;
            loadFromDBButton.VerticalAlignment = VerticalAlignment.Stretch;
            loadFromDBButton.Click += HandleloadFromDBButtonChecked;
            //loadFromDBButton.Unchecked += HandleChannelSelUnChecked;
            ButtonsUniformGrid_Copy1.Children.Add(loadFromDBButton);

            myflyout =new Flyout();
            songListView = new ListView();
            myflyout.Content = songListView;
            songListView.IsItemClickEnabled = true;
            songListView.ItemClick += SongListView_ItemClick;
            loadFromDBButton.Flyout  = myflyout;


            songListView1 = new ListView();
            
            songListView1.IsItemClickEnabled = true;
            songListView1.ItemClick += SongListView_ItemClick1;

            //loadFromDBButton.Flyout  = "{StaticResource TravelFlyout}";
            myflyout1 = new Flyout();
            enter_songname = new TextBox();
           
            Button btn_confirm_songname = new Button();
            btn_confirm_songname.Content = "Save Song";
            btn_confirm_songname.Click += btn_confirm_songname_Click;
            enter_songname.KeyDown += Enter_songname_KeyDown;
            Grid mygrid = new Grid();
            RowDefinition rd1 = new RowDefinition();
            RowDefinition rd2 = new RowDefinition();
            RowDefinition rd3 = new RowDefinition();
            rd1.Height= new GridLength(1,GridUnitType.Star);
            rd2.Height= new GridLength(1,GridUnitType.Star);
            rd2.Height= new GridLength(2,GridUnitType.Star);
            mygrid.RowDefinitions.Add(rd1);
            mygrid.RowDefinitions.Add(rd2);
            mygrid.RowDefinitions.Add(rd3);
            mygrid.Children.Add(enter_songname);
            mygrid.Children.Add(btn_confirm_songname);
            mygrid.Children.Add(songListView1);
            Grid.SetRow(enter_songname, 0);
            Grid.SetRow(btn_confirm_songname, 1);
            Grid.SetRow(songListView1, 2);
            myflyout1.Content = mygrid;
            saveTODBButton.Flyout = myflyout1;

            room[0].uniformGrid1.Visibility = Visibility.Visible;
            room[0].uniformGrid2.Visibility = Visibility.Visible;
            room[0].uniformGrid3.Visibility = Visibility.Visible;

         
            // midiOut1();
            //  checkit();
          //  fill_table();
        }  // public MAINPAGE

        private void btn_confirm_songname_Click(object sender, RoutedEventArgs e)
        {
            if (enter_songname.Text != "")
            {
                CommonData.Mytablename = enter_songname.Text;
                Debug.WriteLine("TABLENAME S: " + CommonData.Mytablename);
                myflyout1.Hide();
                create_table();
                fill_table();
            }
        }

        private async Task create_dbTableAsync()
        {
            //SqlCommand myCommand = new SqlCommand();      
           // IDbCommand myCommand = new SqlCommand();
            IDataReader myReader = null;
            //SqlDataReader myReader = null;

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
                CommonData.MyCon.openConnectionAsync();
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
                    CommonData.MyCon.openConnectionAsync();
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
            private void thebpmSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
            {
                throw new NotImplementedException();
            }

            private void Enter_songname_KeyDown(object sender, KeyRoutedEventArgs e)
            {
                if (e.Key == Windows.System.VirtualKey.Enter)
                {
                if (enter_songname.Text != "")
                {
                    TextBox tbox = sender as TextBox;
                    CommonData.Mytablename = tbox.Text;
                    Debug.WriteLine("TABLENAME S: " + CommonData.Mytablename);
                    myflyout1.Hide();
                    create_table();
                    fill_table();
                }
                }


            }
            private void SongListView_ItemClick1(object sender, ItemClickEventArgs e)
            {
                Debug.WriteLine("TABLENAME S: " + e.ClickedItem.ToString());
                CommonData.Mytablename = e.ClickedItem.ToString();
                myflyout1.Hide();
                create_table();
                fill_table();
            }

            private void SongListView_ItemClick(object sender, ItemClickEventArgs e)
            {
                Debug.WriteLine("TABLENAME S: " + e.ClickedItem.ToString());
                CommonData.Mytablename = e.ClickedItem.ToString();
                myflyout.Hide();
                load_table();

            }

            private void HandlesaveSlotChecked(object sender, RoutedEventArgs e)
            {
                ToggleButton toggle = sender as ToggleButton;
                int m = (int)toggle.Tag;
                tabentry_save = m;
                for (int i = 0; i < numchannels; i++)
                {
                    if (i != m)
                    {
                        saveSlot[i].IsChecked = false;
                    }
                }
            }




            private async void HandleloadFromDBButtonChecked(object sender, RoutedEventArgs e)
            {
                songListView.Items.Clear();
                get_tables();
                // if (CommonData.thesongs.Count()>0) CommonData.thesongs.Clear();

                for (var i = 0; i < CommonData.thesongs.Count; i++)
                {

                    songListView.Items.Add(CommonData.thesongs[i]);
                }
                //  await load_table();
            }

            private async void HandlesaveTODBButtonClick(object sender, RoutedEventArgs e)
            {
                songListView1.Items.Clear();
                get_tables();
                // if (CommonData.thesongs.Count() > 0) CommonData.thesongs.Clear();
                for (var i = 0; i < CommonData.thesongs.Count; i++)
                {

                    songListView1.Items.Add(CommonData.thesongs[i]);
                }
            }

            private void HandlebnkButtonClicked(object sender, RoutedEventArgs e)
            {
                //    throw new NotImplementedException();
                Button button = sender as Button;
                int m = (int)button.Tag;
                room[activechannel].bank += m > 0 ? (room[activechannel].bank < 1) ? 0 : -1 : 1;
                bankchangeme(activechannel, room[activechannel].bank);
                prgchangeme(activechannel, room[activechannel].prg);
                bnkText.Text = room[activechannel].bank.ToString();

            }
            private void HandleprgButtonClicked(object sender, RoutedEventArgs e)
            {
                Button button = sender as Button;
                int m = (int)button.Tag;
                room[activechannel].prg += m > 0 ? (room[activechannel].prg < 1) ? 0 : -1 : 1;
                prgchangeme(activechannel, room[activechannel].prg);
                prgText.Text = room[activechannel].prg.ToString();
                ////    throw new NotImplementedException();
                //  bankchangeme(channel,bank)
                // throw new NotImplementedException();
            }

            private void HandleplayButtonClicked(object sender, RoutedEventArgs e)
            {
                //  this.Frame.Navigate(typeof(BlankPage1));
                //  Debug.WriteLine(" SELECTED: "+ midiOutPortListBox.SelectedIndex);
                if (!midiset && midiOutPortListBox.SelectedIndex == -1) {
                    midiOutPortListBox.SelectedIndex = 0;
                    midiOut1();
                    playsequence.setMidiout(midiOutPort);
                    midiset = true; }

                playsequence.isplaying = !playsequence.isplaying;
                Debug.WriteLine("PLAY");
                Debug.WriteLine("MIDI ITEMS: " + midiOutPortListBox.Items.Count);
            }

            private async void HandleloadSlotChecked(object sender, RoutedEventArgs e)
            {
                //  throw new NotImplementedException();
                ToggleButton toggle = sender as ToggleButton;
                int m = (int)toggle.Tag;
                tabentry = m;
                for (int i = 0; i < numchannels; i++)
                {
                    if (i != m)
                    {
                        loadSlot[i].IsChecked = false;
                    }
                }
                //
                for (int x = 0; x < numchannels; x++)
                {
                    room[x].pattern_load_struct(tabentry);
                    //room[x].slider[1].SetValue(room[x].thepattern.int_vs[tabentry]));
                    room[x].slider[1].Value = room[x].thepattern.int_vs[tabentry];
                    room[x].slider[2].Value = room[x].thepattern.int_sl2[tabentry];

                    room[x].prg = room[x].thepattern.int_prg[tabentry];

                    room[x].bank = room[x].thepattern.int_bnk[tabentry];

                    bankchangeme(x, room[x].bank);
                    prgchangeme(x, room[x].prg);
                    vol_value(x, room[x].thepattern.int_vs[tabentry]);
                    bnkText.Text = room[activechannel].bank.ToString();
                    prgText.Text = room[activechannel].prg.ToString();
                }
            }
            public static void bpm_value(int thevalue)
            {
                playsequence.thebpm = thevalue;
                playsequence.ms = ((60000.0 / (double)thevalue) / (double)4);
                playsequence.dur = playsequence.ms;
                //  Debug.WriteLine("MS: " + playsequence.ms);

            }
            public void setbpmslidershack(int thevalue)
            {
                for (int i = 0; i < numchannels; i++)
                {
                    room[i].slider[0].Value = thevalue;
                }
            }
            public static void vol_value(int x, int v)
            {
                IMidiMessage midiMessageToSend = new MidiControlChangeMessage((byte)x, 7, (byte)v);
                midiOutPort.SendMessage(midiMessageToSend);
            }

            public static void prgchangeme(int x, int prg)
            {
                IMidiMessage midiMessageToSend1 = new MidiProgramChangeMessage((byte)x, (byte)prg);
                midiOutPort.SendMessage(midiMessageToSend1);
            }

            public static void bankchangeme(int x, int bank)
            {
                byte channel = (byte)x;
                byte controller = 0;
                byte controlValue = (byte)bank;
                //  byte prg = (byte)room[x].prg;
                IMidiMessage midiMessageToSend = new MidiControlChangeMessage(channel, controller, controlValue);
                // IMidiMessage midiMessageToSend1 = new MidiProgramChangeMessage(channel, prg);
                midiOutPort.SendMessage(midiMessageToSend);
                //  midiOutPort.SendMessage(midiMessageToSend1);
            }
            public void checkit()
            {
                // Debug.WriteLine("CHECK IT !!!!!!!!!!!!!!!!!! : " );
                //  Debug.WriteLine(room[0].thepattern.vec_bs1[0, 0]);
            }
            public void sendMidiMessage(int i, int j, int index)
            {

                //for (int x = 0; x < 15; x++)
                //{
                //   // ALL NOTES OF
                //    IMidiMessage midiMessageToSend = new MidiControlChangeMessage((byte)(x), (byte)123, (byte)0);
                //    midiOutPort.SendMessage(midiMessageToSend);
                //}

                //for (int x = 0; x < 10; x++)
                //{
                //    for (int y = 0; y < 5; y++)
                //    {                 
                //        if (room[x].thepattern.vec_bs1[y,index] == 1 && room[x].thepattern.vec_m_bs1[y]== 1)
                //        {                   
                //            byte channel = (byte)x;
                //            byte note = (byte)(35 + y);
                //            byte velocity = 100;

                //            IMidiMessage midiMessageToSend = new MidiNoteOnMessage(channel, note, velocity);
                //            midiOutPort.SendMessage(midiMessageToSend);
                //        }
                //    }
                //}
                DoSomething((short)index);
            }
            private void HandlesavePatternChecked(object sender, RoutedEventArgs e)
            {
                // ToggleButton toggle = sender as ToggleButton;
                // int m = (int)toggle.Tag;
                for (int x = 0; x < 10; x++)
                {
                    room[x].pattern_save_struct(tabentry_save);
                    room[x].thepattern.int_vs[tabentry_save] = (int)room[x].slider[1].Value;
                    room[x].thepattern.int_sl2[tabentry_save] = (int)room[x].slider[2].Value;
                    room[x].thepattern.int_prg[tabentry_save] = room[x].prg;
                    room[x].thepattern.int_bnk[tabentry_save] = room[x].bank;

                }
            }
            private void HandleChannelSelUnChecked(object sender, RoutedEventArgs e)
            {
                // throw new NotImplementedException();
            }
            private void HandleChannelSelChecked(object sender, RoutedEventArgs e)
            {
                ToggleButton toggle = sender as ToggleButton;
                int m = (int)toggle.Tag;
                for (int i = 0; i < numchannels; i++)
                {
                    if (i != m)
                    {
                        room[i].uniformGrid1.Visibility = Visibility.Collapsed;
                        room[i].uniformGrid2.Visibility = Visibility.Collapsed;
                        room[i].uniformGrid3.Visibility = Visibility.Collapsed;
                        channelSel[i].IsChecked = false;
                    }
                    room[i].slider[0].Value = CommonData.BPM;

                }
                // Debug.WriteLine("ACTIVECHANNEL:" + activechannel);
                activechannel = m;
                //channelSel[m].IsChecked = true;
                room[m].uniformGrid1.Visibility = Visibility.Visible;
                room[m].uniformGrid2.Visibility = Visibility.Visible;
                room[m].uniformGrid3.Visibility = Visibility.Visible;
                bnkText.Text = room[activechannel].bank.ToString();
                prgText.Text = room[activechannel].prg.ToString();
            }

            private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
            {
                var client = sliderclientDict[sender as Slider];

            }

            //private async void Button_Click(object sender, RoutedEventArgs e)
            private void Button_Click(object sender, RoutedEventArgs e)
            {
                // MediaElement mediaElement = new MediaElement();
                // var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
                //  Windows.Media.SpeechSynthesis.SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync("Servas Wöd, I brauch a göd!");
                // mediaElement.SetSource(stream, stream.ContentType);
                //  mediaElement.Play();
                //  Debug.WriteLine("Servas Wöd, I brauch a göd!");
            }
            private void Button_Click_1(object sender, RoutedEventArgs e)
            {
                vis = !vis;
                Console.WriteLine("Servas Wöd, I brauch a göd!");
                if (vis != true) { room[0].uniformGrid1.Visibility = Visibility.Collapsed; } else { room[0].uniformGrid1.Visibility = Visibility.Visible; }
            }

            //public async void DoSomething(short step)
            public void DoSomething(short step)
            // public static void DoSomething(short step)
            {
                // await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
              {
                  //var frame = new Frame();
                  //frame.Navigate(typeof(Frame2));
                  //  Window.Current.Content = frame;

                  for (short i = 0; i < 16; i++)
                  {
                      led[i].Fill = new SolidColorBrush(Windows.UI.Colors.Black);
                  }

                  led[step].Fill = new SolidColorBrush(Windows.UI.Colors.Red);

                  // System.Threading.Thread.Sleep(1000);
                  // Debug.WriteLine("DA SRED, DA SERD, Na dA TASTk DA TASK, ER RENNT ER RENNNNNT!");
              });
            }

            //***************UWP MIDI
            private async Task EnumerateMidiInputDevices()
            {
                // Find all input MIDI devices
                string midiInputQueryString = MidiInPort.GetDeviceSelector();
                DeviceInformationCollection midiInputDevices = await DeviceInformation.FindAllAsync(midiInputQueryString);

                midiInPortListBox.Items.Clear();

                // Return if no external devices are connected
                if (midiInputDevices.Count == 0)
                {
                    this.midiInPortListBox.Items.Add("No MIDI input devices found!");
                    this.midiInPortListBox.IsEnabled = false;
                    return;
                }

                // Else, add each connected input device to the list
                foreach (DeviceInformation deviceInfo in midiInputDevices)
                {
                    this.midiInPortListBox.Items.Add(deviceInfo.Name);
                }
                this.midiInPortListBox.IsEnabled = true;
            }
            private async Task EnumerateMidiOutputDevices()
            {

                // Find all output MIDI devices
                string midiOutportQueryString = MidiOutPort.GetDeviceSelector();
                DeviceInformationCollection midiOutputDevices = await DeviceInformation.FindAllAsync(midiOutportQueryString);

                midiOutPortListBox.Items.Clear();

                // Return if no external devices are connected
                if (midiOutputDevices.Count == 0)
                {
                    this.midiOutPortListBox.Items.Add("No MIDI output devices found!");
                    this.midiOutPortListBox.IsEnabled = false;
                    return;
                }

                // Else, add each connected input device to the list
                foreach (DeviceInformation deviceInfo in midiOutputDevices)
                {
                    this.midiOutPortListBox.Items.Add(deviceInfo.Name);
                }
                this.midiOutPortListBox.IsEnabled = true;
            }
            private async void midiInPortListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                var deviceInformationCollection = inputDeviceWatcher.DeviceInformationCollection;

                if (deviceInformationCollection == null)
                {
                    return;
                }

                DeviceInformation devInfo = deviceInformationCollection[midiInPortListBox.SelectedIndex];

                if (devInfo == null)
                {
                    return;
                }

                midiInPort = await MidiInPort.FromIdAsync(devInfo.Id);

                if (midiInPort == null)
                {
                    System.Diagnostics.Debug.WriteLine("Unable to create MidiInPort from input device");
                    return;
                }
                midiInPort.MessageReceived += MidiInPort_MessageReceived;

            }
            private void MidiInPort_MessageReceived(MidiInPort sender, MidiMessageReceivedEventArgs args)
            {
                IMidiMessage receivedMidiMessage = args.Message;

                System.Diagnostics.Debug.WriteLine(receivedMidiMessage.Timestamp.ToString());

                if (receivedMidiMessage.Type == MidiMessageType.NoteOn)
                {
                    System.Diagnostics.Debug.WriteLine(((MidiNoteOnMessage)receivedMidiMessage).Channel);
                    System.Diagnostics.Debug.WriteLine(((MidiNoteOnMessage)receivedMidiMessage).Note);
                    System.Diagnostics.Debug.WriteLine(((MidiNoteOnMessage)receivedMidiMessage).Velocity);
                }
            }
            private async void midiOutPortListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                var deviceInformationCollection = outputDeviceWatcher.DeviceInformationCollection;

                if (deviceInformationCollection == null)
                {
                    return;
                }

                DeviceInformation devInfo = deviceInformationCollection[midiOutPortListBox.SelectedIndex];

                if (devInfo == null)
                {
                    return;
                }

                midiOutPort = await MidiOutPort.FromIdAsync(devInfo.Id);

                if (midiOutPort == null)
                {
                    System.Diagnostics.Debug.WriteLine("Unable to create MidiOutPort from output device");
                    return;
                }
                playsequence.setMidiout(midiOutPort);
            }

            private async void midiOut1()
            {
                var deviceInformationCollection = outputDeviceWatcher.DeviceInformationCollection;

                if (deviceInformationCollection == null)
                {
                    return;
                }

                DeviceInformation devInfo = deviceInformationCollection[0];

                if (devInfo == null)
                {
                    return;
                }

                midiOutPort = await MidiOutPort.FromIdAsync(devInfo.Id);

                if (midiOutPort == null)
                {
                    System.Diagnostics.Debug.WriteLine("Unable to create MidiOutPort from output device");
                    return;
                }

            }

            ///*** LAUNCHER CLASS *** NON MIDI
            ///
            public async void DefaultLaunch()
            {
                // Path to the file in the app package to launch
                string imageFile = @"images\\myscript.cmd";

                var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(imageFile);

                if (file != null)
                {
                    // Launch the retrieved file
                    var success = await Windows.System.Launcher.LaunchFileAsync(file);

                    if (success)
                    {
                        // File launched
                    }
                    else
                    {
                        // File launch failed
                    }
                }
                else
                {
                    Debug.WriteLine("NIXI FILE LAUNCH");
                    // Could not find file
                }
            }

            private void Button_Click_2(object sender, RoutedEventArgs e)
            {
                //Debug.WriteLine("X: " + x + " Y: " + y + "Index:" + index + " Value:" + room[x].thepattern.vec_bs1[y, index]);
                Debug.WriteLine(room[0].thepattern.vec_bs1[0, 0]);
            }
            ////   NAVIGATION MENU
            ///
            #region NavigationView event handlers
            private void nvTopLevelNav_Loaded(object sender, RoutedEventArgs e)
            {
            }

            private void nvTopLevelNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
            {
            }

            private void nvTopLevelNav_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
            {
            }
            #endregion
            /////SQLLLL
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
                    myCommand.CommandText = " SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG = '" + CommonData.Database + "'"; //GET TABLES IN DATABASE
                    CommonData.MyCon.openConnectionAsync();
                    myReader = myCommand.ExecuteReader();
                    if (CommonData.thesongs.Count() > 0) CommonData.thesongs.Clear();    //Clear the songs list to fill it again
                    while (myReader.Read())
                    {
                        Debug.WriteLine("HOHO" + myReader[0].ToString());

                        CommonData.setTheSongs(myReader[0].ToString());
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

            }
            async public Task fill_table() {
              //  SqlCommand myCommand = new SqlCommand();
               // SqlDataReader myReader = null;
          //  DbCommand myCommand = new SqlCommand();
            IDataReader myReader = null;
            {
                    try
                    {
                        CommonData.SetConnection();
                        Debug.WriteLine("DATABASE C: " + CommonData.Database);
                      //  SqlCommand mycommand = new SqlCommand();
                        myCommand.Connection = CommonData.MyCon.MyCon;
                        myCommand.CommandType = CommandType.Text;
                        myCommand.CommandText = "UPDATE " + CommonData.Mytablename + " SET " + CommonData.theColummsUpd + "WHERE id=@id";
                        Debug.WriteLine("Command TEXT: " + myCommand.CommandText);
                        //myCommand.CommandText = "INSERT INTO " + CommonData.mytablename + "(id)" + " VALUES (@id)";
                        // myCommand.CommandText = "INSERT INTO " + CommonData.mytablename + "(id," + CommonData.theColummsRaw + ")" + " VALUES (@id," + CommonData.theColummsVal + ")";
                        CommonData.MyCon.openConnectionAsync();
                        for (int i = 0; i < (16 * 5) * numentries; i++)
                        {
                            myCommand.Parameters.Clear();
                            for (int x = 0; x < numchannels; x++)
                            {
                           
                            String thestring = "@Channel" + x;
                            var parameter = myCommand.CreateParameter();
                            parameter.ParameterName = "@SomeName";
                            //query.bindValue(thestring, room[x]->thepattern.vec_bs[i]);

                            // myCommand.Parameters.Add(thestring, SqlDbType.TinyInt).Value = room[x].thepattern.vec_bs[i];
                            myCommand.AddParameter(thestring, room[x].thepattern.vec_bs[i]);
                            }
                            //myCommand.Parameters.Add("@id", SqlDbType.Int).Value = i;
                            myCommand.AddParameter("@id",i);
                            //myReader = myCommand.ExecuteReader();
                            myCommand.ExecuteNonQuery();
                        }
                        CommonData.MyCon.closeConnection();
                    }
                    catch (Exception ex)
                    {
                        // MessageBox.Show(ex.Message);
                        //    MessageDialog dialog = new MessageDialog("FAIL!!", "Information " + ex.Message);
                        //    await dialog.ShowAsync();
                        Debug.WriteLine("FAIL!!", "Information " + ex.Message);
                    }
                    finally
                    {
                        if (CommonData.MyCon.MyCon != null)
                            CommonData.MyCon.closeConnection();
                    }
                }

                {
                    {
                        try
                        {
                          // SqlCommand mycommand = new SqlCommand();
                            myCommand.Connection = CommonData.MyCon.MyCon;
                            myCommand.CommandType = CommandType.Text;
                            myCommand.CommandText = "UPDATE " + CommonData.Mytablename + " SET " + CommonData.theColummsUpdSingle + "WHERE id=@id";
                            Debug.WriteLine("Command TEXT: " + myCommand.CommandText);
                            //myCommand.CommandText = "INSERT INTO " + CommonData.mytablename + "(id)" + " VALUES (@id)";
                            // myCommand.CommandText = "INSERT INTO " + CommonData.mytablename + "(id," + CommonData.theColummsRaw + ")" + " VALUES (@id," + CommonData.theColummsVal + ")";
                            CommonData.MyCon.openConnectionAsync();
                            for (int x = 0; x < numchannels; x++)
                            {
                                myCommand.Parameters.Clear();
                                for (int y = 0; y < numchannels; y++)
                                {
                                    String thestring = "@Volume" + y;
                                    myCommand.Parameters.Add(thestring, SqlDbType.TinyInt).Value = room[y].thepattern.int_vs[x];
                                    thestring = "@Bank" + y;
                                    myCommand.Parameters.Add(thestring, SqlDbType.TinyInt).Value = room[y].thepattern.int_bnk[x];
                                    thestring = "@Prg" + y;
                                    myCommand.Parameters.Add(thestring, SqlDbType.TinyInt).Value = room[y].thepattern.int_prg[x];
                                }
                                myCommand.Parameters.Add("@id", SqlDbType.Int).Value = (x);
                                //myReader = myCommand.ExecuteReader();
                                myCommand.ExecuteNonQuery();
                            }
                            CommonData.MyCon.closeConnection();
                        }
                        catch (Exception ex)
                        {
                            // MessageBox.Show(ex.Message);
                            //    MessageDialog dialog = new MessageDialog("FAIL!!", "Information " + ex.Message);
                            //    await dialog.ShowAsync();
                            Debug.WriteLine("FAIL!!", "Information " + ex.Message);
                        }
                        finally
                        {
                            if (CommonData.MyCon.MyCon != null)
                                CommonData.MyCon.closeConnection();
                        }
                    }


                }



            }

            public async Task load_table()
            {
                {
                   // SqlCommand myCommand = new SqlCommand();
                   // SqlDataReader myReader = null;

               // IDbCommand myCommand = new SqlCommand();
                IDataReader myReader = null;
                // bool tableexisted = false;
                try
                    {
                        CommonData.SetConnection();
                        Debug.WriteLine("DATABASE L: " + CommonData.Database);
                        //Commandtype --> Stored Procedure + Name der Prozedur angeben
                        //  myCommand.CommandText = "SELECT * FROM [myDBSeqAccounts].[dbo].[USERData]";
                        // myCommand.CommandText = "SELECT Username=Username, Passw=Passw FROM USERAccounts";
                        myCommand.Connection = CommonData.MyCon.MyCon;
                        myCommand.CommandType = CommandType.Text;
                        myCommand.CommandText = "SELECT* FROM " + CommonData.Mytablename + "";
                        CommonData.MyCon.openConnectionAsync();
                        myReader = myCommand.ExecuteReader();

                        while (myReader.Read())
                        {
                            for (int i = 0; i < numchannels; i++)
                            {
                                //reader.GetInt32(reader.GetOrdinal(columnName));
                                room[i].thepattern.vec_bs[myReader.GetInt32(0)] = myReader.GetByte(i + 1);
                                //Debug.WriteLine("READ BSTATE" + myReader[0].ToString());
                            }
                        }

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
                }
                {
                  //  SqlCommand myCommand = new SqlCommand();
                  //  SqlDataReader myReader = null;
              //  IDbCommand myCommand = new SqlCommand();
                IDataReader myReader = null;

                // bool tableexisted = false;
                try
                    {
                        //Commandtype --> Stored Procedure + Name der Prozedur angeben
                        //  myCommand.CommandText = "SELECT * FROM [myDBSeqAccounts].[dbo].[USERData]";
                        // myCommand.CommandText = "SELECT Username=Username, Passw=Passw FROM USERAccounts";
                        myCommand.Connection = CommonData.MyCon.MyCon;
                        myCommand.CommandType = CommandType.Text;
                        myCommand.CommandText = "SELECT id, " + CommonData.theColummsSingleRaw + " FROM " + CommonData.Mytablename + " WHERE id=@id";
                        CommonData.MyCon.openConnectionAsync();
                        for (int i = 0; i < numchannels; i++)
                        {
                            myCommand.Parameters.Clear();
                            myCommand.Parameters.Add("@id", SqlDbType.Int).Value = i;
                            //  query.bindValue(":rowid", (i + 1));
                            myReader = myCommand.ExecuteReader();

                            while (myReader.Read())
                            {
                                for (int j = 0; j < numchannels; j++)
                                {
                                    room[j].thepattern.int_vs[myReader.GetInt32(0)] = myReader.GetByte(j + 1);
                                    // Debug.WriteLine("VOL:"+myReader.GetByte(j + 1));
                                    room[j].thepattern.int_bnk[myReader.GetInt32(0)] = myReader.GetByte(((j + 1) + (numchannels)));

                                    room[j].thepattern.int_prg[myReader.GetInt32(0)] = myReader.GetByte(((j + 1) + (numchannels * 2)));
                                    //  Debug.WriteLine("DONE");
                                }

                            }
                            myReader.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        // MessageBox.Show(ex.Message);
                        //  MessageDialog dialog = new MessageDialog("FAIL!!", "Information " + ex.Message);
                        // await dialog.ShowAsync();
                        Debug.WriteLine("FAIL FAIL " + ex.Message);
                    }
                    finally
                    {
                        if (CommonData.MyCon.MyCon != null)
                            CommonData.MyCon.closeConnection();
                    }
                }
            }
            public async Task create_table()
            {
               // SqlCommand myCommand = new SqlCommand();
               // SqlDataReader myReader = null;

          //  IDbCommand myCommand = new SqlCommand();
            IDataReader myReader = null;

            bool tableexisted = false;
                try
                {
                    CommonData.SetConnection();
                    // Debug.WriteLine("DATABASE C: " + CommonData.Database);
                    //Commandtype --> Stored Procedure + Name der Prozedur angeben
                    //  myCommand.CommandText = "SELECT * FROM [myDBSeqAccounts].[dbo].[USERData]";
                    // myCommand.CommandText = "SELECT Username=Username, Passw=Passw FROM USERAccounts";
                    myCommand.Connection = CommonData.MyCon.MyCon;
                    myCommand.CommandType = CommandType.Text;
                    myCommand.CommandText = " SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG = '" + CommonData.Database + "'"; //GET TABLES IN DATABASE
                    CommonData.MyCon.openConnectionAsync();
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
                        myCommand.CommandText = "CREATE TABLE " + CommonData.Mytablename + " (id INTEGER , " + CommonData.theColumms + ")"; //GET TABLES IN DATABASE
                        CommonData.MyCon.openConnectionAsync();
                        myReader = myCommand.ExecuteReader();
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
                    //


                    {
                        try
                        {
                            //SqlCommand mycommand = new SqlCommand();
                     //   IDbCommand myCommand = new SqlCommand();
                        IDataReader myReader = null;
                        myCommand.Connection = CommonData.MyCon.MyCon;
                            myCommand.CommandType = CommandType.Text;
                            myCommand.CommandText = "INSERT INTO " + CommonData.Mytablename + "(id)" + " VALUES (@id)";
                            // myCommand.CommandText = "INSERT INTO " + CommonData.mytablename + "(id," + CommonData.theColummsRaw + ")" + " VALUES (@id," + CommonData.theColummsVal + ")";
                            CommonData.MyCon.openConnectionAsync();
                            for (int i = 0; i < (16 * 5) * numentries; i++)
                            {
                                myCommand.Parameters.Clear();
                                //Debug.WriteLine("INSERT INTO" + i);
                                //myCommand.Parameters.AddWithValue("parentId", 1);
                                myCommand.Parameters.Add("@id", SqlDbType.Int).Value = i;
                                // myCommand.Parameters.AddWithValue("id", i);


                                //myReader = myCommand.ExecuteReader();
                                myCommand.ExecuteNonQuery();

                            }
                            CommonData.MyCon.closeConnection();
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(ex.Message);
                            // MessageDialog dialog = new MessageDialog("FAIL!!", "Information " + ex.Message);
                            //await dialog.ShowAsync();
                            Debug.WriteLine("FAIL!!", "Information " + ex.Message);
                        }
                        finally
                        {
                            if (CommonData.MyCon.MyCon != null)
                                CommonData.MyCon.closeConnection();
                        }
                    }

                }
                //
            }
       
            /////DATABASE CREATE GET DB
            ///
            private async System.Threading.Tasks.Task create_dbAsync(string username)
            {
              //  SqlCommand myCommand = new SqlCommand();
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

            //SqlCommand myCommand = new SqlCommand();
           // SqlDataReader myReader = null;
           // IDbCommand myCommand = new SqlCommand();
            IDataReader myReader = null;
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




///
