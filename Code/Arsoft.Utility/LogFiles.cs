using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;

namespace Arsoft.Utility
{
    public class LogFiles
    {
        #region "Attributes"
        private static string logFileName;
        private static StreamWriter logFile;
        private static string folderFile = "";
        //private static string folderFile = ConfigurationManager.AppSettings["logFile"];
        #endregion

        #region "Methods"

        public static string GetApplicationPath()
        {
            string path = null;
            path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            if (path.Substring(path.Length - 1, 1) != "\\")
            {
                path += "\\";
            }
            return path;
        }

        private static string CreateLogFolder()
        {
            string folderName = GetApplicationPath() + "log";

            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            return folderName + "\\";
        }

        public static string SetLogFolder
        {
            get { return folderFile; }
            set { folderFile = value; }
        }

        public static void WriteToLog(string errorDescription)
        {
            if (folderFile == "" || folderFile == null)
            {
                folderFile = CreateLogFolder();
            }

            if (logFile == null)
            {
                logFileName = "error-" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                logFile = new StreamWriter(folderFile + logFileName, true);
                logFile.AutoFlush = true;
            }
            else if (!logFileName.Equals("error-" + DateTime.Now.ToString("yyyyMMdd") + ".log"))
            {
                logFile.Close();
                logFileName = "";

                logFileName = "error-" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                logFile = new StreamWriter(folderFile + logFileName, true);
                logFile.AutoFlush = true;
            }

            lock ((logFile))
            {
                logFile.WriteLine("Internal" + " | " + DateTime.Now + " | " + errorDescription);
            }
        }

        public static void WriteToLog(string sessionId, string errorDescription)
        {
            if (folderFile == "" || folderFile == null)
            {
                folderFile = CreateLogFolder();
            }

            if (logFile == null)
            {
                logFileName = "error-" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                logFile = new StreamWriter(folderFile + logFileName, true);
                logFile.AutoFlush = true;
            }
            else if (!logFileName.Equals("error-" + DateTime.Now.ToString("yyyyMMdd") + ".log"))
            {
                logFile.Close();
                logFileName = "";

                logFileName = "error-" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                logFile = new StreamWriter(folderFile + logFileName, true);
                logFile.AutoFlush = true;
            }

            lock ((logFile))
            {
                logFile.WriteLine(sessionId + " | " + DateTime.Now + " | " + errorDescription);
            }
        }

        public static void WriteToLog(string className, string functionName, string errorDescription)
        {
            if (folderFile == "" || folderFile == null)
            {
                folderFile = CreateLogFolder();
            }

            if (logFile == null)
            {
                logFileName = "error-" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                logFile = new StreamWriter(folderFile + logFileName, true);
                logFile.AutoFlush = true;
            }
            else if (!logFileName.Equals("error-" + DateTime.Now.ToString("yyyyMMdd") + ".log"))
            {
                logFile.Close();
                logFileName = "";

                logFileName = "error-" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                logFile = new StreamWriter(folderFile + logFileName, true);
                logFile.AutoFlush = true;
            }

            lock ((logFile))
            {
                logFile.WriteLine(DateTime.Now + " | " + className + " | " + functionName + " | " + errorDescription);
            }
        }

        #endregion
    }
}
