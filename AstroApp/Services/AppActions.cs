using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace AstroApp.Services
{
    public class AppActions
    {
        //public async Task SaveAstroEventsAsync(List<AstroEvent> astroEventsList)
        //{
        //    try
        //    {
        //        var localFolderPath = FileSystem.AppDataDirectory; // Gets the local app data folder
        //        var filePath = Path.Combine(localFolderPath, "astroEvents.xml");

        //        XmlSerializer serializer = new XmlSerializer(typeof(List<AstroEvent>));
        //        using (StreamWriter writer = new StreamWriter(filePath, false))
        //        {
        //            serializer.Serialize(writer, astroEventsList);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle or log the exception as needed
        //    }
        //}

        //public async Task<List<AstroEvent>> LoadAstroEventsAsync()
        //{
        //    var localFolderPath = FileSystem.AppDataDirectory;
        //    var filePath = Path.Combine(localFolderPath, "astroEvents.xml");

        //    if (!File.Exists(filePath))
        //        return new List<AstroEvent>();

        //    XmlSerializer serializer = new XmlSerializer(typeof(List<AstroEvent>));
        //    using (StreamReader reader = new StreamReader(filePath))
        //    {
        //        return (List<AstroEvent>)serializer.Deserialize(reader);
        //    }
        //}
        public async Task<AppDB> LoadDBAsync()
        {
            try
            {
                var localFolderPath = FileSystem.AppDataDirectory;
                var filePath = Path.Combine(localFolderPath, "astrodb.json");

                if (!File.Exists(filePath))
                    return new AppDB();

                var jsonString = await File.ReadAllTextAsync(filePath);

                var appDB = JsonSerializer.Deserialize<AppDB>(jsonString);
                return appDB;
            }
            catch (Exception ex)
            {
                return new AppDB();
            }
        }

        public async Task SaveAstroEventsDBAsync(AppDB astroDB)
        {
            try
            {
                var localFolderPath = FileSystem.AppDataDirectory; // Gets the local app data folder
                var filePath = Path.Combine(localFolderPath, "astrodb.json");

                // Read the existing data
                AppDB existingData = null;
                if (File.Exists(filePath))
                {
                    string existingJson = await File.ReadAllTextAsync(filePath);
                    existingData = JsonSerializer.Deserialize<AppDB>(existingJson);
                }

                // Update AstroEvents only
                if (existingData != null)
                {
                    existingData.AstroEventsDB = astroDB.AstroEventsDB;
                }
                else
                {
                    existingData = astroDB; // In case there was no existing data
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                var jsonString = JsonSerializer.Serialize(existingData, options);

                await File.WriteAllTextAsync(filePath, jsonString);
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
            }
        }        

        public async Task SavePlanetinZodiacsAsync(ObservableCollection<PlanetInZodiac> planetInZodiacs)
        {
            try
            {
                var localFolderPath = FileSystem.AppDataDirectory; // Gets the local app data folder
                var filePath = Path.Combine(localFolderPath, "astrodb.json");

                // Read the existing data
                AppDB existingData = null;
                if (File.Exists(filePath))
                {
                    string existingJson = await File.ReadAllTextAsync(filePath);
                    existingData = JsonSerializer.Deserialize<AppDB>(existingJson);
                }

                // Update PlanetInZodiac only
                if (existingData != null)
                {
                    existingData.PlanetInZodiacsDB = planetInZodiacs;
                }
                else
                {
                    existingData = new AppDB();
                    existingData.PlanetInZodiacsDB = planetInZodiacs; // In case there was no existing data
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                var jsonString = JsonSerializer.Serialize(existingData, options);

                await File.WriteAllTextAsync(filePath, jsonString);
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
            }
        }        
    }    
}
