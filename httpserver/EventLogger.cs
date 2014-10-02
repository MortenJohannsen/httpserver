using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace httpserver
{
    public class EventLogger
    {
        private EventLog log;
        private const string Source = "AM HTTP Server";
        private const string sLog = "Application";
        private string[] EventLogArray;

        /// <summary>
        /// EventLogger constructor
        /// </summary>
        public EventLogger()
        {
            // Tjekker om sourcen allerede eksistere, og hvis ikke, oprettes en ny
            if (!EventLog.SourceExists(Source))
            {
                EventLog.CreateEventSource(Source, sLog);
            }
            
            // En ny log oprettes
            log = new EventLog("");

            log.Source = "AM HTTP Server";
            
            //Et array med Log beskederne oprettes
            EventLogArray = new string[5];
            //Metode kaldes som ligger log beskeder i EventLogArray
            CreateEventLogValues();
        }

        /// <summary>
        /// Metode som sammensætter og opretter en log entry med ønsket information
        /// </summary>
        /// <param name="logId">Det ID som er blevet tildelt til de forskellige typer Log beskeder</param>
        public void WriteLogEntry(int logId)
        {
            //Omformer EventLogArray fra typen objekt til en string inden den sendes
            string eventLog = EventLogArray.GetValue(logId).ToString();
            
            // Den sammensatte log sendes til Windows Event logger
            log.WriteEntry("EventID: " + logId + " Information: " + eventLog);

        }

        /// <summary>
        /// Indsætter værdier i EventLogArray som repræsenterer forskellige log beskeder
        /// </summary>
        private void CreateEventLogValues()
        {
           // Log beskederne samt et LogId placeres i EventLogArray
           EventLogArray.SetValue("Serveren er startet",0);
           EventLogArray.SetValue("Serveren modtog en forespørgsel", 1);
           EventLogArray.SetValue("Serveren sendte et svar", 2);
           EventLogArray.SetValue("Serveren er lukket", 3);
        }

    }
}
