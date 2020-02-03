using System;
using System.Collections.Generic;
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

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace CSharp1
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public bool vis = true;
        public MainPage()
        {
            this.InitializeComponent();
            Console.WriteLine("Servas Wöd, I brauch a göd!");
            ButtonsUniformGrid.Visibility = Visibility.Visible;

            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 16; j++)
                {
                    {
                        ToggleButton b = new ToggleButton(); ;
                        b.Content = "ToggleButton";
                        b.Click += Button_Click;

                        b.HorizontalAlignment = HorizontalAlignment.Stretch;
                        b.VerticalAlignment = VerticalAlignment.Stretch;
                        b.IsThreeState = true;
                         
                         ButtonsUniformGrid.Children.Add(b);
                    }
                }
        }

        //private async void Button_Click(object sender, RoutedEventArgs e)
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // MediaElement mediaElement = new MediaElement();
            // var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
            //  Windows.Media.SpeechSynthesis.SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync("Servas Wöd, I brauch a göd!");
            // mediaElement.SetSource(stream, stream.ContentType);
            //  mediaElement.Play();
            Console.WriteLine("Servas Wöd, I brauch a göd!");
            
              

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            vis = !vis;
            Console.WriteLine("Servas Wöd, I brauch a göd!");
            if (vis == true) { ButtonsUniformGrid.Visibility = Visibility.Collapsed; } else { ButtonsUniformGrid.Visibility = Visibility.Visible; }
        }
    }
}
