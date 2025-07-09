using System.ComponentModel;
using System.Diagnostics;
namespace SignatorDocSolution.Utils
{
    /// <summary>
    /// CLASS_ID=13;
    /// </summary>
    public static class RegisterWinEventLogViewer
    {
        private static string sSource = "SignatorDoc";
        private static string sLog = "Application";
        private static string sEvent;
        private static EventLogEntryType eventLogEntryType = EventLogEntryType.Information;

        public static void RegisterLog(string evtSourceName, string evtMessage, int evtID, int eventType)
        {

            sEvent = evtSourceName + evtMessage;
            if (!EventLog.SourceExists(sSource))
            {
                //EventSourceCreationData eventFileData = new EventSourceCreationData(sSource, sLog);
                //eventFileData.MessageResourceFile = new System.Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)).LocalPath + @"SignatorDoc_Log.log";
                EventLog.CreateEventSource(sSource, sLog);
            }

            
            switch (eventType)
            {
                case 2: {
                    eventLogEntryType = EventLogEntryType.Error;
                    break; 
                }
                case 3: {
                    eventLogEntryType = EventLogEntryType.FailureAudit;
                    break; 
                }
                case 4: {
                    eventLogEntryType = EventLogEntryType.SuccessAudit;
                    break; 
                }
                case 5: {
                    eventLogEntryType = EventLogEntryType.Warning;
                    break; 
                }
            }

            EventLog.WriteEntry(sSource, sEvent);
            EventLog.WriteEntry(sSource, sEvent, eventLogEntryType, eventType);
        
        }
    }
}
