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
using CSharp1.Views;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace CSharp1
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public partial class MainPage : Page
    {


        public BlankPage1 mypage;
      //  public static Worker playsequence;
        public MainPage()
        {

            //prgButton[0].Visibility = Visibility.Collapsed;
            InitializeComponent();

        
            //   playsequence = new Worker();
            //   mypage = new BlankPage1();
            //   //Frame theframe = new Frame();
            //   //theframe = Window.Current.Content as Frame;
            //   // theframe.Navigate(typeof(BlankPage1));
            //   //Window.Current.Content = theframe;


            //   //playsequence = new Worker(mypage.room);


            ////   Worker.LogHandler myLogger = new Worker.LogHandler(BlankPage1.sendMidiMessage);
            //   Worker.LogHandler myLogger = new Worker.LogHandler(mypage.sendMidiMessage);
            //  // Worker.LogHandler myLogger = new Worker.LogHandler(BlankPage1.sendMidiMessage);
            //   playsequence.myLogger1 = myLogger;

            //   Action<object> action = (object obj) =>
            //   {

            //       Console.WriteLine("Task={0}, obj={1}, Thread={2}",
            //       Task.CurrentId, obj,
            //       Thread.CurrentThread.ManagedThreadId);
            //       playsequence.mythread1();
            //   };


            // Creating thread
            //  Using thread class

            //Thread thr = new Thread(new ThreadStart(playsequence.mythread1));
            //thr.Start();

            //Task t1 = new Task(action, "alpha");
            //t1.Start();
            //Console.WriteLine("t1 has been launched. (Main Thread={0})",
            //                  Thread.CurrentThread.ManagedThreadId);
            // startPlaySequence();


            ////
            // FileLogger fl = new FileLogger("process.log");

            // MyClass myClass = new MyClass(); WORKER

            // Crate an instance of the delegate, pointing to the Logger()
            // function on the fl instance of a FileLogger.

            // playsequence.Process(myLogger);
            //fl.Close();




        }


        #region NavigationView event handlers
        private void nvTopLevelNav_Loaded(object sender, RoutedEventArgs e)
        {
            // set the initial SelectedItem
            foreach (NavigationViewItemBase item in nvTopLevelNav.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "Home_Page")
                {
                    nvTopLevelNav.SelectedItem = item;
                    break;
                }
            }
            contentFrame.Navigate(typeof(BlankPage1));

            nvTopLevelNav.IsPaneOpen = false;
        }

        private void nvTopLevelNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
        }

        private void nvTopLevelNav_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            TextBlock ItemContent = args.InvokedItem as TextBlock;
            if (ItemContent != null)
            {
                switch (ItemContent.Tag)
                {
                    case "Nav_Home":
                        contentFrame.Navigate(typeof(BlankPage1));
                        break;

                    case "Nav_Shop":
                        contentFrame.Navigate(typeof(ShopPage));
                        break;

                    //case "Nav_ShopCart":
                    //    contentFrame.Navigate(typeof(CartPage));
                    //    break;

                    //case "Nav_Message":
                    //    contentFrame.Navigate(typeof(MessagePage));
                    //    break;

                    //case "Nav_Print":
                    //    contentFrame.Navigate(typeof(PrintPage));
                    //    break;
                }
            }
        }
        #endregion


    }
}