namespace PgpZipTransfer.Services;

public class LoggingService
{
    private readonly string _logFile;
    private readonly object _lock = new();

    public LoggingService()
    {
        var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PgpZipTransfer");
        Directory.CreateDirectory(dir);
        _logFile = Path.Combine(dir, "app.log");
    }

    public void LogInfo(string message) => Write("INFO", message);
    public void LogError(string message) => Write("ERROR", message);

    private void Write(string level, string message)
    {
        var line = $"{DateTime.UtcNow:O} [{level}] {message}";
        lock (_lock)
        {
            File.AppendAllText(_logFile, line + Environment.NewLine);
        }
    }
}
