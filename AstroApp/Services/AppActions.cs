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

        public async Task SaveAstroEventsAsync(List<AstroEvent> astroEventsList)
        {
            try
            {
                var localFolderPath = FileSystem.AppDataDirectory; // Gets the local app data folder
                var filePath = Path.Combine(localFolderPath, "astroEvents.json");

                var options = new JsonSerializerOptions { WriteIndented = true };
                var jsonString = JsonSerializer.Serialize(astroEventsList, options);

                await File.WriteAllTextAsync(filePath, jsonString);
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
            }
        }

        public async Task<List<AstroEvent>> LoadAstroEventsAsync()
        {
            var localFolderPath = FileSystem.AppDataDirectory;
            var filePath = Path.Combine(localFolderPath, "astroEvents.json");

            if (!File.Exists(filePath))
                return new List<AstroEvent>();

            var jsonString = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<AstroEvent>>(jsonString);
        }

        public async Task SavePlanetinZodiacsAsync(ObservableCollection<PlanetInZodiac> astroEventsList)
        {
            try
            {
                var localFolderPath = FileSystem.AppDataDirectory; // Gets the local app data folder
                var filePath = Path.Combine(localFolderPath, "planetinzodiacs.json");

                var options = new JsonSerializerOptions { WriteIndented = true };
                var jsonString = JsonSerializer.Serialize(astroEventsList, options);

                await File.WriteAllTextAsync(filePath, jsonString);
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
            }
        }

        public async Task<ObservableCollection<PlanetInZodiac>> LoadPlanetInZodiacsDetailsAsync()
        {
            var localFolderPath = FileSystem.AppDataDirectory;
            var filePath = Path.Combine(localFolderPath, "planetinzodiacs.json");

            if (!File.Exists(filePath))
            {
                var planetInZodiacsDetails = new ObservableCollection<PlanetInZodiac>();

                // Iterate over each value in the Planet enum
                foreach (Planet planet in Enum.GetValues(typeof(Planet)))
                {
                    // Iterate over each value in the ZodiacSign enum
                    foreach (ZodiacSign zodiacSign in Enum.GetValues(typeof(ZodiacSign)))
                    {
                        // Create a new PlanetInZodiac and add it to the collection
                        planetInZodiacsDetails.Add(new PlanetInZodiac
                        {
                            Planet = planet,
                            ZodiacSign = zodiacSign,
                            PlanetInZodiacInfo = "",                            
                        });
                    }
                }
                return planetInZodiacsDetails;
            }

            else 
            {
            var jsonString = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<ObservableCollection<PlanetInZodiac>>(jsonString);
            }
        }

        internal async Task<ObservableCollection<MoonDay>> LoadMoonDaysAsync()
        {
            var localFolderPath = FileSystem.AppDataDirectory;
            var filePath = Path.Combine(localFolderPath, "moondays.json");

            if (!File.Exists(filePath))
                return new ObservableCollection<MoonDay>();

            var jsonString = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<ObservableCollection<MoonDay>>(jsonString);
        }
    }    
}
