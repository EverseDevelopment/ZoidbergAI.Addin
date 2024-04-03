using Common.Application;
using System;
using System.IO;

namespace Common.Utils
{
    public class AppLogger
    {
        #region Singleton

        private static AppLogger instance;
        public static AppLogger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppLogger();
                }
                return instance;
            }
        }

        #endregion

        static string logPath = AppConstants.LogsFolder + $"\\logs\\logs_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}.txt";

        private AppLogger()
        {
            if (!Directory.Exists(Path.Combine(AppConstants.LogsFolder, "logs")))
            {
                Directory.CreateDirectory(Path.Combine(AppConstants.LogsFolder, "logs"));
            }
        }

        public void AddInformation(string message)
        {
            File.AppendAllText(Path.Combine(AppConstants.LogsFolder, logPath), BuildMessage(message));
        }

        public void AddEx(Exception ex, string message)
        {
            File.AppendAllText(Path.Combine(AppConstants.LogsFolder, logPath), BuildMessage(message + " - " + ex.Message + " - " + ex.StackTrace, "EXCEPTION"));
        }

        private string BuildMessage(string msg, string type = "INFO")
        {
            return Environment.NewLine + $"LOG.{type} [{DateTime.Now.ToString()}]" + Environment.NewLine + msg;
        }
    }
}
