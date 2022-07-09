using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

    public class Util
    {
        private static StringBuilder strB = new StringBuilder();
        public static void WriteObj(Object o,string Path)
        {
            

            //serialize
            using (Stream stream = File.Open(Path, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bformatter.Serialize(stream, o);
            }
        }
        public static Object  ReadObj(string Path)
        {

            Object o=null;
            using (Stream stream = File.Open(Path, FileMode.Open))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                o = bformatter.Deserialize(stream);
            }
            return o;

        }
        public static void WriteErrorLog(Exception ex)
        {
            WriteLog("!!!ExceptionOccured:" + ex.ToString());

        }
        public static void WriteLog(string str)
        {
            System.IO.StreamWriter SW = new System.IO.StreamWriter(LogPath,true);
            SW.WriteLine(str);
            SW.Close();
            SW.Dispose();

        }
        public static string CurrentPath
        {
            get
            {
                return System.AppDomain.CurrentDomain.BaseDirectory;
            }
        }
        public static string LogPath
        {
            get
            {
            return CurrentPath + @"\SwapGem.log";
            }
        }
        public static void ShowLog()
        {
           // frmShowLog f = new frmShowLog();
           // f.ShowDialog();
        }

        public static string GetLog()
        {
            return strB.ToString();
        }
        private static void ClearLog()
        {
            strB = new StringBuilder();
        }
        private static Random R = new Random();
        public static  int GetRandom(int Min, int Max)
        {
            return  R.Next (Min,Max);
        }

        public static string  ReadFile( string Path)
        {
            string strTemp = "";
            try
            {
            
                System.IO.StreamReader SR = new System.IO.StreamReader(Path);
                
                strTemp = SR.ReadToEnd();
                SR.Close();
                SR.Dispose();
            }
            catch (Exception ex)
            {

            }
            return strTemp;
        }
        public static void WriteFile(string Message,string Path)
        {
            try
            {
                System.IO.StreamWriter SW = new System.IO.StreamWriter(Path,false );
                SW.WriteLine(Message);

                SW.Close();
                SW.Dispose();
            }
            catch (Exception ex)
            {

            }

        }
    }

