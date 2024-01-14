using AstroApp.Data.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AstroApp.Services
{
    public class AppActions
    {
        public async Task SaveAstroEventsAsync(List<AstroEvent> astroEventsList)
        {
            try
            {
                var localFolderPath = FileSystem.AppDataDirectory; // Gets the local app data folder
                var filePath = Path.Combine(localFolderPath, "astroEvents.xml");

                XmlSerializer serializer = new XmlSerializer(typeof(List<AstroEvent>));
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    serializer.Serialize(writer, astroEventsList);
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
            }
        }

        public async Task<List<AstroEvent>> LoadAstroEventsAsync()
        {
            var localFolderPath = FileSystem.AppDataDirectory;
            var filePath = Path.Combine(localFolderPath, "astroEvents.xml");

            if (!File.Exists(filePath))
                return new List<AstroEvent>();

            XmlSerializer serializer = new XmlSerializer(typeof(List<AstroEvent>));
            using (StreamReader reader = new StreamReader(filePath))
            {
                return (List<AstroEvent>)serializer.Deserialize(reader);
            }
        }
    }    
}
