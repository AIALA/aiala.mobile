using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using xappido.Mobile.Core.Models;
using xappido.Mobile.Core.Services;

namespace aiala.mobile
{
    public class AppLoggerService : ILoggerService
    {
        public AppLoggerService()
        {
        }

        public void LoadLogLevel()
        {
        }

        private void Log(LogType type, string message, Exception exception = null)
        {
            if (type >= LogType.Warning)
            {
                Analytics.TrackEvent("Log", new Dictionary<string, string> {
                    { "LogType", type.ToString() },
                    { "Message", message}
                });
            }

            if (exception != null)
            {
                Crashes.TrackError(exception, new Dictionary<string, string> { { "message", message } });
            }

            Debug.WriteLine($"{type.ToString().ToUpper()} | {message}");

        }

        public void LogWarning(string message)
        {
            Log(LogType.Warning, message);
        }

        public void LogInfo(string message)
        {
            Log(LogType.Info, message);
        }

        public void LogDebug(string message)
        {
            Log(LogType.Debug, message);
        }

        public void LogError(string message, Exception ex)
        {
            Log(LogType.Error, message, ex);
        }

        public void PurgeLogs()
        {
        }

        public ObservableCollection<LogItem> GetAllLogs()
        {
            var logs = new ObservableCollection<LogItem>();
            return logs;
        }
    }
}