using AstroApp.Data.Enums;
using AstroApp.Data.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace AstroApp.Services
{
    public class AppActions
    {
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

        public async Task SaveDataAsync<T>(T data, string propertyKey)
        {
            try
            {
                var localFolderPath = FileSystem.AppDataDirectory;
                var filePath = Path.Combine(localFolderPath, "astrodb.json");
                AppDB existingData = null;

                if (File.Exists(filePath))
                {
                    string existingJson = await File.ReadAllTextAsync(filePath);
                    existingData = JsonSerializer.Deserialize<AppDB>(existingJson);
                }
                else
                {
                    existingData = new AppDB(); // Initialize if not exist
                }

                // Using reflection to set the property dynamically
                var propertyInfo = existingData.GetType().GetProperty(propertyKey);
                propertyInfo?.SetValue(existingData, data);

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
