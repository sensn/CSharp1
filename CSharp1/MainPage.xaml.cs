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

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace CSharp1
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
       

        public const int numchannels = 10;
        public bool vis = true;
        public Dictionary<ToggleButton, Tuple<int,int>> clientDict = new Dictionary<ToggleButton, Tuple<int,int>>();
        public Dictionary<Slider, int> sliderclientDict = new Dictionary<Slider,int>();
        // public Brush Background { get; set; }
        public UniformGrid my = new UniformGrid();
        public UniformGrid my1 = new UniformGrid();

        public static Room[] room = new Room[10];
        public ToggleButton[] channelSel = new ToggleButton[10];
        public ToggleButton[] saveSlot = new ToggleButton[10];
        public ToggleButton[] loadSlot = new ToggleButton[10];
        public Button[] prgButton = new Button[2];
        public Button[] bnkButton = new Button[2];
        public static Rectangle[] led = new Rectangle[16];

        public static Worker playsequence;


        MyMidiDeviceWatcher inputDeviceWatcher;
        MyMidiDeviceWatcher outputDeviceWatcher;

        static MidiInPort midiInPort;
        static IMidiOutPort midiOutPort;

        
        int tabentry = 0;
        int numentries =10;
        int activechannel=0;
        public MainPage()
        {

            //prgButton[0].Visibility = Visibility.Collapsed;
            this.InitializeComponent();
            


           Debug.WriteLine("Servas Wöd, I brauch a göd! CREATE TASK");
           // DefaultLaunch();    //Launch a app from asociated filetype
            //****MIDI
            inputDeviceWatcher =
              new MyMidiDeviceWatcher(MidiInPort.GetDeviceSelector(), midiInPortListBox, Dispatcher);

            inputDeviceWatcher.StartWatcher();

            outputDeviceWatcher =
                new MyMidiDeviceWatcher(MidiOutPort.GetDeviceSelector(), midiOutPortListBox, Dispatcher);

            outputDeviceWatcher.StartWatcher();

            //*****MIDI END



            //System.Threading.Tasks.Task task1 = new Task(DoSomething);
            //task1.Start();
            //task1.Factory.StartNew(DoSomething);

            // Creating object of ExThread class 
            playsequence = new Worker();

            Action<object> action = (object obj) =>
            {
                
                Console.WriteLine("Task={0}, obj={1}, Thread={2}",
                Task.CurrentId, obj,
                Thread.CurrentThread.ManagedThreadId);
                playsequence.mythread1();
            };
            // Creating thread 
            // Using thread class 

            // Thread thr = new Thread(new ThreadStart(playsequence.mythread1));
            // thr.Start();

            Task t1 = new Task (action, "alpha");
            t1.Start();
            Console.WriteLine("t1 has been launched. (Main Thread={0})",
                              Thread.CurrentThread.ManagedThreadId);
            // startPlaySequence();

            ButtonsUniformGrid.Visibility = Visibility.Visible;
            ButtonsUniformGrid_Copy.Orientation = Orientation.Horizontal;
            ButtonsUniformGrid_Copy.Columns = 16;
            ButtonsUniformGrid_Copy.Rows = 4;
            for (int i = 0; i < numchannels; i++)
            {
                room[i] = new Room();
                room[i].channel = i;
                thegrid.Children.Add(room[i].uniformGrid1);
                thegrid.Children.Add(room[i].uniformGrid2);
              
                Grid.SetColumn(room[i].uniformGrid1, 0);     //ToggleButtonMatrix
                   Grid.SetRow(room[i].uniformGrid1, 1);
                Grid.SetColumn(room[i].uniformGrid2, 1);     //Sliders
                   Grid.SetRow(room[i].uniformGrid2, 1);
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

            
                 for (int i = 0; i < 16; i++)
            {
                 led[i] = new Rectangle();
                led[i].Fill = new SolidColorBrush(Windows.UI.Colors.Black);  //SAPCER
                LedUniformGrid.Children.Add(led[i]);
            }
            led[3].Fill = new SolidColorBrush(Windows.UI.Colors.DarkRed);  //SAPCER


            for (int i = 0; i < 2; i++)
            {
                Border myspacer1 = new Border();
                myspacer1.Background = new SolidColorBrush(Windows.UI.Colors.Black);  //SAPCER
                ButtonsUniformGrid_Copy.Children.Add(myspacer1);
            }
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
         
            Border myspacer2 = new Border();
            myspacer2.Background = new SolidColorBrush(Windows.UI.Colors.Black);  //SAPCER
            ButtonsUniformGrid_Copy.Children.Add(myspacer2);

            ToggleButton playbutton = new ToggleButton();
            playbutton.HorizontalAlignment = HorizontalAlignment.Stretch;
            playbutton.VerticalAlignment = VerticalAlignment.Stretch;
            playbutton.Click += HandleplayButtonClicked;
            ButtonsUniformGrid_Copy.Children.Add(playbutton);
            for (int i = 0; i < 12; i++)
            {
                Border myspacer1 = new Border();
                myspacer1.Background = new SolidColorBrush(Windows.UI.Colors.Black);  //SAPCER
                ButtonsUniformGrid_Copy.Children.Add(myspacer1);
            }
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

            for (int i = 0; i < 3; i++)
            {
                Border myspacer1 = new Border();
                myspacer1.Background = new SolidColorBrush(Windows.UI.Colors.Black);  //SAPCER
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



            room[0].uniformGrid1.Visibility = Visibility.Visible;
            room[0].uniformGrid2.Visibility = Visibility.Visible;
          
        }  // public MAINPAGE

        private void HandleprgButtonClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int m = (int)button.Tag;
            room[0].bank += m > 0 ? 1 : -1;

            ////    throw new NotImplementedException();
            //MainPage.bankchangeme(channel,bank)
            // throw new NotImplementedException();
        }

        private void HandlesaveSlotChecked(object sender, RoutedEventArgs e)
        {
            ToggleButton toggle = sender as ToggleButton;
            int m = (int)toggle.Tag;
            tabentry = m;
            for (int i = 0; i < numchannels; i++)
            {
                if (i != m)
                {


                    saveSlot[i].IsChecked = false;
                }
              
            }
        }

     


        private void HandleloadFromDBButtonChecked(object sender, RoutedEventArgs e)
        {
          //  throw new NotImplementedException();
        }

        private void HandlesaveTODBButtonClick(object sender, RoutedEventArgs e)
        {
           // throw new NotImplementedException();
        }

        private void HandlebnkButtonClicked(object sender, RoutedEventArgs e)
        {
        //    throw new NotImplementedException();
        }

        private void HandleplayButtonClicked(object sender, RoutedEventArgs e)
        {
            //  throw new NotImplementedException();
            Debug.WriteLine("111111111111111");
           
            playsequence.isplaying = !playsequence.isplaying;
           
            //   DoSomething();
            Debug.WriteLine("2222222222222");

            //byte channel = 0;
            //byte note = 60;
            //byte velocity = 127;
            //IMidiMessage midiMessageToSend = new MidiNoteOnMessage(channel, note, velocity);
            
            //midiOutPort.SendMessage(midiMessageToSend);
        }

        private void HandleloadSlotChecked(object sender, RoutedEventArgs e)
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

            for (int x = 0; x < 10; x++)
            {
                room[x].pattern_load_struct(tabentry);
                //room[x].slider[1].SetValue(room[x].thepattern.int_vs[tabentry]));
                room[x].slider[1].Value = room[x].thepattern.int_vs[tabentry];
                room[x].slider[2].Value= room[x].thepattern.int_sl2[tabentry];
                //room[x]->vol_slider->setValue(room[x]->thepattern.int_vs[tabentry]);
               
            room[x].prg = room[x].thepattern.int_prg[tabentry];
           // room[x]->prg = room[x]->thepattern.int_prg[tabentry];
               
            room[x].bank = room[x].thepattern.int_bnk[tabentry];
                //room[x]->bank = room[x]->thepattern.int_bnk[tabentry];


                bankchangeme(x, room[x].bank);
                prgchangeme(x, room[x].prg);
                vol_value(x,room[x].thepattern.int_vs[tabentry]);
                //vol_value(x, room[x]->vol_slider->value());

                //  qDebug()<<"X:"<<x<<"tabentry:"<<tabentry;
            }



        }
        public static void bpm_value(int thevalue)
        {
            playsequence.thebpm = thevalue;
            playsequence.ms = ((60000.0 / (double)thevalue) / (double)4);
            playsequence.dur = playsequence.ms;


        }
        public static void vol_value(int x, int v)
        {
            IMidiMessage midiMessageToSend = new MidiControlChangeMessage((byte)x, 7, (byte) v);
            midiOutPort.SendMessage(midiMessageToSend);
        }

        public static void prgchangeme(int x, int prg)
        {
            IMidiMessage midiMessageToSend1 = new MidiProgramChangeMessage((byte)x, (byte) prg);
            midiOutPort.SendMessage(midiMessageToSend1);
        }

        public static void bankchangeme(int x, int bank)
        {
            byte channel = (byte) x;
            byte controller = 0;
            byte controlValue = (byte)bank;
            byte prg = (byte)room[x].prg;
            IMidiMessage midiMessageToSend = new MidiControlChangeMessage(channel,controller,controlValue);
            IMidiMessage midiMessageToSend1 = new MidiProgramChangeMessage(channel, prg);
          
            midiOutPort.SendMessage(midiMessageToSend);
            midiOutPort.SendMessage(midiMessageToSend1);
        }

        private void HandlesavePatternChecked(object sender, RoutedEventArgs e)
        {
           // ToggleButton toggle = sender as ToggleButton;
           // int m = (int)toggle.Tag;

            for (int x = 0; x < 10; x++)
            {
                room[x].pattern_save_struct(tabentry);
                room[x].thepattern.int_vs[tabentry] = (int)room[x].slider[1].Value;
                room[x].thepattern.int_sl2[tabentry] = (int)room[x].slider[2].Value;
               room[x].thepattern.int_prg[tabentry] = room[x].prg;
                room[x].thepattern.int_bnk[tabentry] = room[x].bank;
                // qDebug()<<"X:"<<x<<"tabentry:"<<tabentry;
            }

            //    throw new NotImplementedException();
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
                    //   throw new NotImplementedException();
                    channelSel[i].IsChecked = false;
                }
            }
            activechannel = m;
            //channelSel[m].IsChecked = true;
            room[m].uniformGrid1.Visibility = Visibility.Visible;
            room[m].uniformGrid2.Visibility = Visibility.Visible;
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            // throw new NotImplementedException();
            var client = sliderclientDict[sender as Slider];
          //  Debug.WriteLine(client);
        }

        private void HandleToggleButtonUnChecked(object sender, RoutedEventArgs e)
        {
          //  ToggleButton toggle = sender as ToggleButton;
            MyToggle toggle = sender as MyToggle;
            toggle.state = 0;
            // toggle.Background = new SolidColorBrush(Windows.UI.Colors.Green);
            //throw new NotImplementedException();
            Debug.WriteLine("isChecked:" + toggle.IsChecked);
            Debug.WriteLine("State:" + toggle.state);
        }

     

        private void HandleToggleButtonChecked(object sender, RoutedEventArgs e)
        {
            // ToggleButton toggle = sender as ToggleButton;
            MyToggle toggle = sender as MyToggle;
            //  toggle.Background = new SolidColorBrush(Windows.UI.Colors.Yellow);
            //  throw new NotImplementedException();
            toggle.state = 1;
            Debug.WriteLine(toggle.Resources.Values);
            Debug.WriteLine("isChecked:" + toggle.IsChecked);
            Debug.WriteLine("State:" + toggle.state);
        }

        private void HandleButtonClick(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            var client = clientDict[sender as ToggleButton];
            Debug.WriteLine(client.Item1 +" " + client.Item2);
           
        }

        //private async void Button_Click(object sender, RoutedEventArgs e)
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // MediaElement mediaElement = new MediaElement();
            // var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
            //  Windows.Media.SpeechSynthesis.SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync("Servas Wöd, I brauch a göd!");
            // mediaElement.SetSource(stream, stream.ContentType);
            //  mediaElement.Play();
            Debug.WriteLine("Servas Wöd, I brauch a göd!");

          

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
          
            vis = !vis;
            Console.WriteLine("Servas Wöd, I brauch a göd!");
           // if (vis != true) { ButtonsUniformGrid.Visibility = Visibility.Collapsed; } else { ButtonsUniformGrid.Visibility = Visibility.Visible; }
           // if (vis != true) { my.Visibility = Visibility.Visible; } else { my.Visibility = Visibility.Collapsed; }
            if (vis != true) { room[0].uniformGrid1.Visibility = Visibility.Collapsed; } else { room[0].uniformGrid1.Visibility = Visibility.Visible; }
        }

        public async  static void DoSomething(short step)
       // public static void DoSomething(short step)
        {



          //  var frame = (Frame)Window.Current.Content;
          //  var page = (MainPage)frame.Content;


            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
          //  Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
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
            Debug.WriteLine("DA SRED, DA SERD, Na dA TASTk DA TASK, ER RENNT ER RENNNNNT!");
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

        }

        ///*** LAUNCHER CLASS *** NON MIDI
        ///
      public  async void DefaultLaunch()
        {
            // Path to the file in the app package to launch
             string imageFile =@"images\\myscript.cmd";

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

    }
}
