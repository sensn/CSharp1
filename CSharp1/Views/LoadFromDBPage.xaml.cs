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

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace CSharp1.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class LoadFromDBPage : Page
    {
        public LoadFromDBPage()
        {
            this.InitializeComponent();
            SetsongListView();
        }

        public void SetsongListView()
        {
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
        }
    }
}
