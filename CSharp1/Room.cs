using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace CSharp1
{
  public class Room
    {
        public UniformGrid uniformGrid1 = new UniformGrid();
        public UniformGrid uniformGrid2 = new UniformGrid();
        public UniformGrid uniformGrid3 = new UniformGrid();
        public bool vis = true;

        public Dictionary<MyToggle, Tuple<int,int>> clientDict = new Dictionary<MyToggle, Tuple<int,int>>();
        public Dictionary<Slider, int> sliderclientDict = new Dictionary<Slider,int>();

        public  MyToggle[,] bu = new MyToggle[5, 16];
        public  MyToggle[] mute_bu = new MyToggle[5];
        public Slider[] slider = new Slider[3];

        // public Button[] prgButton = new Button[2];
        // public Button[] bnkButton = new Button[2];

        //
       public  int channel;
       public  int bank = 0;
        public int prg = 0;
        int tempo = 150;

        public int[,] vec_bs2;
        public struct pattern
        {
           
            // public List<int> int_tempo;

            public int[,] vec_bs1 ;
            public int[] vec_m_bs1 ;
            //public List<int> vec_bs1;
            public List<int> vec_bs;
            public List<int> vec_m_bs;
            public List<int> int_vs;
            public List<int> int_bnk;
            public List<int> int_prg;
            public List<int> int_sl2;
        };
        public pattern thepattern = new pattern();
        //  List<int> vec_bs;

        public Room()
        {
            vec_bs2 = new int[5, 16];
            //  thepattern = new pattern();
            // thepattern.vec_bs = new List<int>(10000);
            // thepattern.vec_bs = new List<int>(5*16*10);
            thepattern.vec_bs1 = new int[5, 16];
            thepattern.vec_m_bs1 = new int[5];
            //thepattern.vec_bs1 = new List<int>(5 * 16 * 10);
            thepattern.vec_bs = new List<int>(5 * 16 * 10);
            thepattern.vec_m_bs = new List<int>(5*10);
            thepattern.int_bnk = new List<int>(10);
            thepattern.int_prg = new List<int>(10);

            thepattern.int_vs = new List<int>(10);
            thepattern.int_sl2 = new List<int>(10);

            for (int i = 0; i < 10; i++)
            {
                thepattern.int_vs.Add(0);
                thepattern.int_sl2.Add(0);
                thepattern.int_bnk.Add(0);
                thepattern.int_prg.Add(0);

            }
            for (int i = 0; i < 5*10; i++)
            {
                thepattern.vec_m_bs.Add(0);
            }

            for (int i = 0; i < (5 * 16 * 10); i++)
            {
                // Debug.WriteLine("LOOP" + i);
                thepattern.vec_bs.Add(0);
                //  thepattern.vec_bs.Add(0);
            }

            uniformGrid1.Columns = 16;
            uniformGrid1.Rows = 5;
            uniformGrid1.ColumnSpacing = 4;
            uniformGrid1.RowSpacing = 4;
            uniformGrid1.Orientation = Orientation.Horizontal;

            uniformGrid2.Columns = 3;
            uniformGrid2.Rows = 1;

            uniformGrid2.ColumnSpacing = 4;
            uniformGrid2.RowSpacing = 4;
            uniformGrid2.Orientation = Orientation.Horizontal;

            uniformGrid3.Columns = 1;
            uniformGrid3.Rows = 5; 
            uniformGrid3.Margin = new Thickness(15, 0, 15, 0);
            //uniformGrid3.Margin = new Thickness(15, 15, 15, 15);

            uniformGrid3.ColumnSpacing = 4;
            uniformGrid3.RowSpacing = 4;
            uniformGrid3.Orientation = Orientation.Vertical;


            uniformGrid1.Visibility = Visibility.Collapsed;
            uniformGrid2.Visibility = Visibility.Collapsed;
            uniformGrid3.Visibility = Visibility.Collapsed;


            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 16; j++)
                {
                    {
                        bu[i, j] = new MyToggle();
                        //bu[i, j].Background = new SolidColorBrush(Windows.UI.Colors.DarkBlue);
                        // bu[i, j].Content = "T";
                        //   bu[i, j].Click += HandleButtonClick;
                        bu[i, j].Checked += HandleToggleButtonChecked;
                        bu[i, j].Unchecked += HandleToggleButtonUnChecked;
                        // bu[i, j].Tag = i;
                        //bool isOver = bu[i, j].IsPointerOver;

                        clientDict.Add(bu[i, j], new Tuple<int, int>(i, j));
                        bu[i, j].HorizontalAlignment = HorizontalAlignment.Stretch;
                        bu[i, j].VerticalAlignment = VerticalAlignment.Stretch;
                        bu[i, j].IsThreeState = false;
                        // b.IsThreeState = true;
                        uniformGrid1.Children.Add(bu[i, j]);
                    }
                }
            //bu[1, 1].state = 1;
            //bu[1, 1].update();

            for (int i = 0; i < 5; i++) { 
                mute_bu[i] = new MyToggle();
                mute_bu[i].Checked += HandleMuteButtonChecked;
                mute_bu[i].Unchecked += HandleMuteButtonUnChecked;
                mute_bu[i].HorizontalAlignment = HorizontalAlignment.Stretch;
                mute_bu[i].VerticalAlignment = VerticalAlignment.Stretch;
                mute_bu[i].Tag = i;
               //mute_bu[i].IsThreeState = false;
               // b.IsThreeState = true;
               uniformGrid3.Children.Add(mute_bu[i]);
            }

            for (int i = 0; i < 3; i++)
            {
                slider[i] = new Slider();
                slider[i].Header = "Volume";
                slider[i].Width = 600;
                // slider[i].Height = 1000;
                sliderclientDict.Add(slider[i], i);
                slider[i].Orientation = Orientation.Vertical;
                slider[i].HorizontalAlignment = HorizontalAlignment.Stretch;
                slider[i].VerticalAlignment = VerticalAlignment.Stretch;

                slider[i].ValueChanged += Slider_ValueChanged;
                uniformGrid2.Children.Add(slider[i]);
            }
            slider[0].Maximum = 600;
            slider[0].Minimum = 30;
            slider[1].Maximum = 127;
            slider[1].Minimum = 0;

            //for (int i = 0; i < 2; i++) { 
            //    prgButton[i] = new Button();
            //prgButton[i].HorizontalAlignment = HorizontalAlignment.Stretch;
            //prgButton[i].VerticalAlignment = VerticalAlignment.Stretch;
            //prgButton[i].Click += HandleprgButtonClicked;
            ////   saveSlot[i].Unchecked += HandleChannelSelUnChecked;
            //prgButton[i].Tag = i;
            //    //
            //    bnkButton[i] = new Button();
            //    bnkButton[i].HorizontalAlignment = HorizontalAlignment.Stretch;
            //    bnkButton[i].VerticalAlignment = VerticalAlignment.Stretch;
            //    bnkButton[i].Click += HandlebnkButtonClicked;
            //    //   saveSlot[i].Unchecked += HandleChannelSelUnChecked;
            //    bnkButton[i].Tag = i;

            //}

        }

        private void HandleMuteButtonUnChecked(object sender, RoutedEventArgs e)
        {
            ToggleButton toggle = sender as ToggleButton;
            int m = (int)toggle.Tag;
            thepattern.vec_m_bs1[m] = 0;
        }

        private void HandleMuteButtonChecked(object sender, RoutedEventArgs e)
        {
            ToggleButton toggle = sender as ToggleButton;
            int m = (int)toggle.Tag;
            thepattern.vec_m_bs1[m] = 1;
        }

        public void pattern_load_struct(int tabentry)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    thepattern.vec_bs1[i, j] = thepattern.vec_bs[(j) + (i * 16) + ((80) * tabentry)];
                    bu[i, j].IsChecked = thepattern.vec_bs1[i, j] != 0;  // INT TO BOOL        
                    //bu[i,j].IsChecked = thepattern.vec_bs[(j) + (i * 16) + ((80) * tabentry)] != 0 ;  // INT TO  
                }
            }
            for (int i = 0; i < 5; i++)
            {
                thepattern.vec_m_bs1[i] = thepattern.vec_m_bs[(i) + ((5) * tabentry)];
                mute_bu[i].IsChecked = thepattern.vec_m_bs1[i] != 0;
            }
        }
        public void pattern_save_struct(int tabentry)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                              
                    thepattern.vec_bs[(j) + (i * 16) + ((80) * tabentry)] = thepattern.vec_bs1[i,j];
            
                }
            }
            for (int i = 0; i < 5; i++)
            {
               thepattern.vec_m_bs[(i) + ((5) * tabentry)] = thepattern.vec_m_bs1[i];
            }


        }
        private void HandlebnkButtonClicked(object sender, RoutedEventArgs e)
        {
            //Button button = sender as Button;
            //int m = (int)button.Tag;
            //bank += m > 0 ? 1 : -1;
           
            ////    throw new NotImplementedException();
            //MainPage.bankchangeme(channel,bank)
        }
        private void HandleprgButtonClicked(object sender, RoutedEventArgs e)
        {
            // throw new NotImplementedException();
        }

        public  void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
           Slider sl = sender as Slider;
            
            //// throw new NotImplementedException();
            var client = sliderclientDict[sender as Slider];
            //Debug.WriteLine(client+ " " + sl.Value);
            ////  Debug.WriteLine(client);
            if (client == 0)
            BlankPage1.bpm_value((int)sl.Value);
            if (client == 1)
                BlankPage1.vol_value(channel, (int)sl.Value);

            //throw new NotImplementedException();
        }

        private void HandleToggleButtonUnChecked(object sender, RoutedEventArgs e)
        {
            //  ToggleButton toggle = sender as ToggleButton;
            MyToggle toggle = sender as MyToggle;
            toggle.state = 0;
            // toggle.Background = new SolidColorBrush(Windows.UI.Colors.Green);
            //throw new NotImplementedException();
          //  Debug.WriteLine("isChecked:" + toggle.IsChecked);
          //  Debug.WriteLine("State:" + toggle.state);

            var client = clientDict[sender as MyToggle];
          // Debug.WriteLine(client.Item1 + " " + client.Item2);
            thepattern.vec_bs1[client.Item1, client.Item2] = 0;
            //throw new NotImplementedException();
        }

        public void HandleToggleButtonChecked(object sender, RoutedEventArgs e)
        {
            // ToggleButton toggle = sender as ToggleButton;
            //MyToggle toggle = sender as MyToggle;
            MyToggle toggle = sender as MyToggle;
            //  toggle.Background = new SolidColorBrush(Windows.UI.Colors.Yellow);
            //  throw new NotImplementedException();
            toggle.state = 1;
           // toggle.update();
           // Debug.WriteLine(toggle.Resources.Values);
           // Debug.WriteLine("isChecked:" + toggle.IsChecked);
          //  Debug.WriteLine("State:" + toggle.state);

            var client = clientDict[sender as MyToggle];
           // Debug.WriteLine(client.Item1 + " " + client.Item2);
            //  throw new NotImplementedException();


         //   thepattern.vec_bs1[0, 15] = 1;

           this.thepattern.vec_bs1[client.Item1, client.Item2] = 1;
          // this.vec_bs2[client.Item1, client.Item2] = 1;
            //bu[client.Item1, client.Item2].state = 1;
            //bu[client.Item1, client.Item2].update();
           // Debug.WriteLine("CHANNEL:" + channel + " VECBS! " + thepattern.vec_bs1[(int)client.Item1, (int) client.Item2]);

            //  bu[client.Item1, client.Item2].update();

         
        }
    } //Class
}
