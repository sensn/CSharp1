using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp1
{
    class CommonData
    {
        public static List<String> thesongs=  new List<String>(2);

        public static int BPM;
        public static bool SQLite = true;

       public static String theColumms = "";
       public static String theColummsRaw = "";
       public static String theColummsVal = "";
       public static String theColummsUpd = "";
       public static String theColummsUpdSingle = "";
       public static String theColummsSingleRaw = "";
        public static String theColummsUpdMute = "";
       public static String theColummsSingleMute = "";
       private static String mytablename = "";
       public static int numchannels = 10;

        public static DBConnection MyCon;

        public static string Mytablename { get => mytablename; set => mytablename = value; }
        public static string Database { get; internal set; }
        public static string Datasource { get; internal set; }

        public static int theconnection = 1; //0 Local 1 Remote, 

        public static void setTheSongs(String thesong)
        {
            thesongs.Add(thesong);
        }
        public static async Task SetConnection()
        {
            
            MyCon = new DBConnection();
            MyCon.setConnectionString();
            MyCon.establishConnectionAsync();
        }

        public static void create_SQL_Strings()
        {
            theColumms = "";
            theColummsRaw = "";
            theColummsVal = "";
            theColummsUpd = "";

            for (int i = 0; i < numchannels; i++)
            {
                theColumms += "Channel" + i + " TINYINT ,";
                theColummsRaw += "Channel" + i + ", ";
                theColummsVal += "Channel" + i + ", ";
                theColummsUpd += "Channel" + i + "=" + "@Channel" + i + " ";
                if (i != numchannels - 1)
                {
                  //  theColumms += ",";
                   // theColummsRaw += ",";
                   // theColummsVal += ",";
                    theColummsUpd += ",";
                }
            }

            for (int i = 0; i < numchannels; i++)
            {
                theColumms += "Mute" + i + " TINYINT ,";
                theColummsRaw += "Mute" + i + ", ";
                theColummsVal += "@Mute" + i + ", ";
                // theColummsUpd += "Mute" + i + "=" + "@Mute" + i + ", ";
            }

            for (int i = 0; i < numchannels; i++)
            {
                theColumms += "Volume" + i + " TINYINT ,";
                theColummsRaw += "Volume" + i + ", ";
                theColummsVal += "@Volume" + i + ", ";
               // theColummsUpd += "Volume" + i + "=" + "@Volume" + i + ", ";
            }
            for (int i = 0; i < numchannels; i++)
            {
                theColumms += "Bank" + i + " TINYINT ,";
                theColummsRaw += "Bank" + i + ", ";
                theColummsVal += "@Bank" + i + ", ";
               // theColummsUpd += "Bank" + i + "=" + "@Bank" + i + ", ";
            }

          

            for (int i = 0; i < numchannels; i++)
            {
                theColumms += "Prg" + i + " TINYINT ";
                theColummsRaw += "Prg" + i + " ";
                theColummsVal += "@Prg" + i + " ";
               // theColummsUpd += "Prg" + i + "=" + "@Prg" + i + " ";

                if (i != numchannels - 1)
                {
                    theColumms += ",";
                    theColummsRaw += ",";
                    theColummsVal += ",";
                    //theColummsUpd += ",";
                }

            }

            theColummsSingleRaw = "";
            theColummsUpdSingle = "";
            //******   Single ***
            for (int i = 0; i < numchannels; i++)
            {
                //         theColumms+= "Volume" + i + " TINYINT ,";
                theColummsSingleRaw += "Volume" + i + ", ";
                //  theColummsVal+= "@Volume" + i + ", ";
                theColummsUpdSingle += "Volume" + i + "=" + "@Volume" + i + ", ";
            }
            for (int i = 0; i < numchannels; i++)
            {
                //         theColumms+= "Bank" + i + " TINYINT ,";
                theColummsSingleRaw += "Bank" + i + ", ";
                //         theColummsVal+= "@Bank" + i + ", ";
                theColummsUpdSingle += "Bank" + i + "=" + "@Bank" + i + ", ";
            }

            for (int i = 0; i < numchannels; i++)
            {
                //         theColumms+= "Prg" + i + " TINYINT ";
                theColummsSingleRaw += "Prg" + i + " ";
                //         theColummsVal+= "@Prg" + i + " ";
                theColummsUpdSingle += "Prg" + i + "=" + "@Prg" + i + " ";

                if (i != numchannels - 1)
                {
                    //                  theColumms+=  ",";
                    theColummsSingleRaw += ",";
                    //                  theColummsVal+= ",";
                    theColummsUpdSingle += ",";
                }
            }


            theColummsSingleMute = "";
            theColummsUpdMute = "";
            for (int i = 0; i < numchannels; i++)
            {
                //         theColumms+= "Prg" + i + " TINYINT ";
                theColummsSingleMute += "Mute" + i + " ";
                //         theColummsVal+= "@Prg" + i + " ";
                theColummsUpdMute += "Mute" + i + "=" + "@Mute" + i + " ";

                if (i != numchannels - 1)
                {
                    //                  theColumms+=  ",";
                    theColummsSingleMute += ",";
                    //                  theColummsVal+= ",";
                    theColummsUpdMute += ",";
                }
            }
            //*****
        }


    }
}
