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
        public delegate void LogHandler2(short index);
        public LogHandler myLogger1;
        public LogHandler2 myLogger2;
        public Room[] theRooms = new Room[10];
        // public   IMidiOutPort midiOutPort1;

        // public Worker() {
        //    midiOutPort1 = midiOutPort;
        // }
        // Non-static method 
        //public Worker(Room[] room)
        public static IMidiOutPort midiOutPort;
        public Worker()
        {
            // myroom = room;
        }
        public void settherooms(Room[] trooms)
        {
            theRooms = trooms;
            for (int i = 0; i < 10; i++)
            {
                Debug.WriteLine("THEROOMS : "+theRooms[i].channel);
            }
        }

        public void setMidiout( IMidiOutPort midiOutPort1)
        {
            midiOutPort = midiOutPort1;
           
          
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
                               // Process(myLogger1,1,1,index);
                                Process2(myLogger2,index);

                        //    if (MainPage.room[i]->Field[worker->index - 1][j]->bstate > 0)
                        //  Debug.WriteLine("CHECK IT !!! : " + mp.room[1].bu[1, 1].IsChecked);


                        //Debug.WriteLine("TRIGGER " + index);
                        //  qDebug() << "BSTATE:"<< room[i]->Field[worker->index-1][j]->bstate;
                        //MainPage.
                        //sendMidiMessage(i, j,index);
                        //  midi.sendMsg(0x90 + i | (35 + j) << 8 | 64 << 16);
                        //  IMidiMessage midiMessageToSend = new MidiControlChangeMessage((byte)x, 7, (byte)v);
                        //   midiOutPort1.SendMessage(midiMessageToSend);
                        //*****
                        for (int x = 0; x < 15; x++)
                        {
                            // ALL NOTES OF
                            IMidiMessage midiMessageToSend = new MidiControlChangeMessage((byte)(x), (byte)123, (byte)0);
                            midiOutPort.SendMessage(midiMessageToSend);
                        }

                        for (int x = 0; x < 10; x++)
                        {
                            for (int y = 0; y < 5; y++)
                            {
                                if (theRooms[x].thepattern.vec_bs1[y, index] == 1 && theRooms[x].thepattern.vec_m_bs1[y] == 1)
                                {
                                    byte channel = (byte)x;
                                    byte note = (byte)(35 + y);
                                    byte velocity = 100;

                                    IMidiMessage midiMessageToSend = new MidiNoteOnMessage(channel, note, velocity);
                                    midiOutPort.SendMessage(midiMessageToSend);
                                }
                            }
                        }
                        //***
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
         
        }
        public void Process2(LogHandler2 logHandler ,short index)
        {
            if (logHandler != null)
            {
                logHandler(index);
            }

        }




    }
}
