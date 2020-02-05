using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public bool vis = true;

        public Dictionary<ToggleButton, Tuple<int,int>> clientDict = new Dictionary<ToggleButton, Tuple<int,int>>();
        public Dictionary<Slider, int> sliderclientDict = new Dictionary<Slider,int>();

        public MyToggle[,] bu = new MyToggle[5, 16];
        public Slider[] slider = new Slider[3];
       
        
        
        public Room()
        {

            uniformGrid1.Columns=16;
            uniformGrid1.Rows = 5;
            uniformGrid1.ColumnSpacing = 4;
            uniformGrid1.RowSpacing = 4;
            uniformGrid1.Orientation = Orientation.Horizontal;

            uniformGrid2.Columns = 3;
            uniformGrid2.Rows = 1;
         
            uniformGrid2.ColumnSpacing = 4;
            uniformGrid2.RowSpacing = 4;
            uniformGrid2.Orientation = Orientation.Horizontal;

            uniformGrid1.Visibility = Visibility.Collapsed;
            uniformGrid2.Visibility = Visibility.Collapsed;


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
            bu[1, 1].state = 1;
            bu[1, 1].update();

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

        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
           Slider sl = sender as Slider;
            
            // throw new NotImplementedException();
            var client = sliderclientDict[sender as Slider];
            Debug.WriteLine(client+ " " + sl.Value);
            //  Debug.WriteLine(client);

            //throw new NotImplementedException();
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

            var client = clientDict[sender as ToggleButton];
            Debug.WriteLine(client.Item1 + " " + client.Item2);

            //throw new NotImplementedException();
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

            var client = clientDict[sender as ToggleButton];
            Debug.WriteLine(client.Item1 + " " + client.Item2);
            //  throw new NotImplementedException();
        }
    } //Class
}
