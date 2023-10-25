using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;

namespace JRBAWebApplication2.Classes
{
    /// <summary>
    /// This a portion of this code is based on a tutorial by [Coding Under Pressure]
    ///The original code can be found at[https://www.youtube.com/watch?v=DrXIWwBfUWI]
    ///Modifications were made to the code like the path to the log file and making the class statics so it can be accessed from anywhere
    /// </summary>
    public static class logger
    {
        //******************************************************************************//
        /// <summary>
        /// This method writes a message to the log file
        /// </summary>
        /// <param name="message"></param>
        //********************************************************************************//
        public static void WriteLog(string message)
        {
            string logPath = ConfigurationManager.AppSettings["LogPath"];

            using (StreamWriter file = new StreamWriter(logPath, true))
            {
                file.WriteLine($"{DateTime.Now} : {message}");
            }
        }
    }
}
//----------------------------------------...ooo000 END OF FILE 000ooo...----------------------------------------//