using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp1
{
    public class Worker
    {
       public int oddeven;
       public bool isplaying = false;
       public long timeinterval;
     
       public int thebpm = 150;
      
       public double ms;
       public double dur;
       public double swing = 0;
       public double theswing = 0.33;
       public short index = 0;



        // Non-static method 
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

                        Debug.WriteLine("First Thread");
                        MainPage.DoSomething(index);
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


    }
}
