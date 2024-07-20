using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace FolderSortingHat.Utils
{
  public enum LogLevel
  {
    Info,
    Warning,
    Error
  }

  public static class Logger
  {
    private static readonly string LogFileName = Assembly.GetExecutingAssembly().GetName().Name;
    private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", $"{LogFileName}.log");
    private static readonly object LogLock = new object();

    static Logger()
    {
      var logDirectory = Path.GetDirectoryName(LogFilePath);
      if (!Directory.Exists(logDirectory))
      {
        Directory.CreateDirectory(logDirectory);
      }
    }

    public static void Log(string message, LogLevel level = LogLevel.Info)
    {
      var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";

      lock (LogLock)
      {
        File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
      }

      Console.WriteLine(logMessage);
    }

    public static async Task LogAsync(string message, LogLevel level = LogLevel.Info)
    {
      var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";

      await Task.Run(() =>
      {
        lock (LogLock)
        {
          File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
        }
      });

      Console.WriteLine(logMessage);
    }

    public static void LogException(Exception ex, string additionalInfo = null)
    {
      var message = $"Exception: {ex.Message}";
      if (!string.IsNullOrEmpty(additionalInfo))
      {
        message += $" | Additional Info: {additionalInfo}";
      }
      message += $"\nStackTrace: {ex.StackTrace}";

      Log(message, LogLevel.Error);
    }

    public static async Task LogExceptionAsync(Exception ex, string additionalInfo = null)
    {
      var message = $"Exception: {ex.Message}";
      if (!string.IsNullOrEmpty(additionalInfo))
      {
        message += $" | Additional Info: {additionalInfo}";
      }
      message += $"\nStackTrace: {ex.StackTrace}";

      await LogAsync(message, LogLevel.Error);
    }
  }
}