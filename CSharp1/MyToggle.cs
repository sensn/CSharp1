using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace CSharp1
{
    public class MyToggle : ToggleButton
    {
        public const int LEDWidth = 20;
        public const int LEDHeight = 20;

        public short state = 0;

        public MyToggle()
        {
            
            Background = new SolidColorBrush(Windows.UI.Colors.DarkGray);
          //  Foreground = new SolidColorBrush(Windows.UI.Colors.DarkRed);
           // FlatStyle = FlatStyle.Flat;
           // Size = new Size(LEDWidth, LEDHeight);
           // UseVisualStyleBackColor = false;
        }

        public void update()
        {
            if (state == 1)
            {
              
                IsChecked = true;
                
            }

        }

    }
}
