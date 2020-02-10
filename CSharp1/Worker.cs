using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Midi;
using Windows.UI.Xaml;

namespace CSharp1
{
    public class Worker
    {
       // public pattern thepattern;
      // public Room[] myroom;
        public int oddeven;
        public bool isplaying = false;
        public long timeinterval;

        public int thebpm = 150;

        public double ms;
        public double dur;
        public double swing = 0;
        public double theswing = 0.33;
        public short index = 0;

        public delegate void LogHandler(int i, int j, int index);
        public LogHandler myLogger1;
        // public   IMidiOutPort midiOutPort1;

        // public Worker() {
        //    midiOutPort1 = midiOutPort;
        // }
        // Non-static method 
        //public Worker(Room[] room)
        public Worker()
        {
           // myroom = room;
        }

        public void mythread1()
        {
            long begin = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            //double begin = clock();
            // double theend = clock();
            double theend = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            ms = ((60000.0 / (double)thebpm) / (double)4);  //Milliseconds per quarternote
                                                            //ms = 125;  //Millisecond per quarternote
                                                            //dur = (ms / 1000) * CLOCKS_PER_SEC;
            dur = ms;
           // MainPage mp;
            //mp = new MainPage();

            //***
            while (true)
            {
                while (isplaying)
                {
                    // begin = clock();
                    begin = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                    if (begin > theend)
                    {
                        // theend = (double)clock() + dur + (dur * (swing));
                        theend = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond + dur + (dur * (swing));

                       //  Debug.WriteLine("First Thread");
                       
                       // BlankPage1.DoSomething(index);
                        // mypage.DoSomething(index);
                       
                        //for (int j = 0; j < 15; j++)
                        //{
                        //    // midi.sendMsg(176 + j | 123 << 8 | 0 << 16);  // ALL NOTES OFF
                        //}
                        //Debug.WriteLine("INDEX: " + index);
                       
                        //for (int i = 0; i < 10; i++)
                        //{
                        //    for (int j = 0; j < 5; j++)
                        //    {
                               // Debug.WriteLine("MYROOM: " + myroom[0].bu[i, j].IsChecked);
                               
                               // Debug.WriteLine("MYROOM: " + myroom[0].thepattern.vec_bs[0]);
                                
                               // Process(myLogger1,i,j,index);
                                Process(myLogger1,1,1,index);

                                //    if (MainPage.room[i]->Field[worker->index - 1][j]->bstate > 0)
                                //  Debug.WriteLine("CHECK IT !!! : " + mp.room[1].bu[1, 1].IsChecked);


                                //Debug.WriteLine("TRIGGER " + index);
                                //  qDebug() << "BSTATE:"<< room[i]->Field[worker->index-1][j]->bstate;
                                //MainPage.
                                //sendMidiMessage(i, j,index);
                                //  midi.sendMsg(0x90 + i | (35 + j) << 8 | 64 << 16);
                                //  IMidiMessage midiMessageToSend = new MidiControlChangeMessage((byte)x, 7, (byte)v);
                                //   midiOutPort1.SendMessage(midiMessageToSend);


                        //    }
                        //}
                                index++;
                                if (index == 16)
                                { //reset things
                                  //tickindex = 0;
                                    index = 0;
                                }


                            
                        





                    }
                }


            }
        }

        public void Process(LogHandler logHandler,int i,int j, int index)
        {
            if (logHandler != null)
            {
                logHandler(i,j,index);
            }
           // Debug.WriteLine("LOGLOGLOGLOGLOG");
            //if (logHandler != null)
            //{
            //    logHandler(i,j,3);
            //}
        }

    }

}
