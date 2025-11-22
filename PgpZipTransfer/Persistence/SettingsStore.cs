using System.Text.Json;
using PgpZipTransfer.Models;

namespace PgpZipTransfer.Persistence;

public class SettingsStore
{
    private readonly string _settingsPath;

    public SettingsStore()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dir = Path.Combine(appData, "PgpZipTransfer");
        Directory.CreateDirectory(dir);
        _settingsPath = Path.Combine(dir, "settings.json");
    }

    public AppSettings Load()
    {
        try
        {
            if (File.Exists(_settingsPath))
            {
                var json = File.ReadAllText(_settingsPath);
                var s = JsonSerializer.Deserialize<AppSettings>(json);
                return s ?? new AppSettings();
            }
        }
        catch { }
        return new AppSettings();
    }

    public void Save(AppSettings settings)
    {
        try
        {
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_settingsPath, json);
        }
        catch { }
    }
}
