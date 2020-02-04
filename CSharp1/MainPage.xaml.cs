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

        public Room[] room = new Room[10];
        public ToggleButton[] channelSel = new ToggleButton[10];
        public MainPage()
        {
             
            this.InitializeComponent();
            
           Debug.WriteLine("Servas Wöd, I brauch a göd!");
            ButtonsUniformGrid.Visibility = Visibility.Visible;
            ButtonsUniformGrid_Copy.Orientation = Orientation.Horizontal;
            ButtonsUniformGrid_Copy.Columns = 16;
            ButtonsUniformGrid_Copy.Rows = 4;
            for (int i = 0; i < numchannels; i++)
            {
                room[i] = new Room();
                thegrid.Children.Add(room[i].uniformGrid1);
                thegrid.Children.Add(room[i].uniformGrid2);
              
                Grid.SetColumn(room[i].uniformGrid1, 0);     //ToggleButtonMatrix
                   Grid.SetRow(room[i].uniformGrid1, 0);
                Grid.SetColumn(room[i].uniformGrid2, 1);     //Sliders
                   Grid.SetRow(room[i].uniformGrid2, 0);
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
            room[0].uniformGrid1.Visibility = Visibility.Visible;
            room[0].uniformGrid2.Visibility = Visibility.Visible;
          
        }  // public MAINPAGE

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
    }
}
