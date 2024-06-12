using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;

namespace AstroApp.Services
{
    public class AppActions
    {
        private static readonly HttpClient httpClient = new HttpClient();        

        //public async Task<AppDB> LoadDBAsync()
        //{
        //    try
        //    {
        //        // Open the embedded astrodb.json file in the app package
        //        using var stream = await FileSystem.OpenAppPackageFileAsync("astrodb.json");
        //        using var reader = new StreamReader(stream);

        //        // Asynchronously read the contents of the file
        //        var contents = await reader.ReadToEndAsync();

        //        // Deserialize the JSON string into an AppDB object
        //        var appDB = JsonSerializer.Deserialize<AppDB>(contents);

        //        // Return the deserialized AppDB object
        //        return appDB;
        //    }
        //    catch (Exception ex)
        //    {
        //        // If any exception occurs, return a new instance of AppDB
        //        return new AppDB();
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

                // Sort the AstroEvents by Date
                astroDB.AstroEventsDB = new ObservableCollection<AstroEvent>(
                    astroDB.AstroEventsDB.OrderBy(e => e.Date));

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

        public async Task SavePlanetinZodiacsAsync(ObservableCollection<PlanetInZodiacDetails> planetInZodiacs)
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

        public async Task SaveMoonDaysAsync(ObservableCollection<MoonDayDetails> moonDays)
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

                // Update Moondays only
                if (existingData != null)
                {
                    existingData.MoonDayDetailsDB = moonDays;
                }
                else
                {
                    existingData = new AppDB();
                    existingData.MoonDayDetailsDB = moonDays; // In case there was no existing data
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

        public async Task SavePlanetInRetrogradeAsync(ObservableCollection<PlanetInRetrogradeDetails> planetInRetrogradeDetails)
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

                // Update PlanetInRetrograde only
                if (existingData != null)
                {
                    existingData.PlanetInRetrogradeDetailsDB = planetInRetrogradeDetails;
                }
                else
                {
                    existingData = new AppDB();
                    existingData.PlanetInRetrogradeDetailsDB = planetInRetrogradeDetails; // In case there was no existing data
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

        public async Task<AppDB> LoadDBFromUrlAsync(string url)
        {
            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var appDB = JsonSerializer.Deserialize<AppDB>(jsonString);
                return appDB;
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                return new AppDB();
            }
        }

    }
}
