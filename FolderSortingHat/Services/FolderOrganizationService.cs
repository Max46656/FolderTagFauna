using FolderSortingHat.Repository;
using FolderSortingHat.Models;
using FolderSortingHat.Utils;
using System.IO;

namespace FolderSortingHat.Services
{

  public class FolderOrganizationService : IFolderOrganizationService
  {
    private readonly FolderScanner _folderScanner;
    private readonly CsvManager _csvManager;
    private readonly FolderTagMatcher _folderTagMatcher;
    private readonly FolderHandler _folderHandler;
    public FolderOrganizationService(
        FolderScanner folderScanner,
        CsvManager csvManager,
        FolderTagMatcher folderTagMatcher,
        FolderHandler folderHandler)
    {
      _folderScanner = folderScanner;
      _csvManager = csvManager;
      _folderTagMatcher = folderTagMatcher;
      _folderHandler = folderHandler;
    }

    public async Task<List<Folder>> ScanFoldersAsync(string folderPath, bool includeFormattedOnly = false)
    {
      var folders = await Task.Run(() => _folderScanner.ScanFolder(folderPath, includeFormattedOnly));
      return folders;
    }

    public Task SaveToCsvAsync(List<Folder> folders, string filePath)
    {
      return Task.Run(() => _csvManager.SaveToCsv(folders, filePath));
    }

    public Task<List<Folder>> LoadFromCsvAsync(string filePath)
    {
      return Task.Run(() => _csvManager.LoadFromCsv(filePath));
    }

    public List<(Folder SourceFolder, Folder DestFolder)> MatchFolders(List<Folder> sourceFolders, List<Folder> destFolders)
    {
      return _folderTagMatcher.MatchFolders(sourceFolders, destFolders);
    }
    public async Task MoveFoldersAsync(List<(Folder SourceFolder, Folder DestFolder)> matches)
    {
      foreach (var match in matches)
      {
        await Task.Run(() =>
        {
          try
          {
            _folderHandler.MoveFolder(match.SourceFolder, match.DestFolder);
          }
          catch (Exception ex)
          {
            Logger.LogException(ex, $"移動資料夾失敗: 從 {match.SourceFolder.Path} 到 {match.DestFolder.Path}");
          }
        });
      }
    }
    public List<string> LoadOrCreateExcludeTags(string filePath)
    {
      return _csvManager.LoadExcludeTags(filePath);
    }
    public async Task CreateUnclassifiedFoldersAsync(string unclassifiedPath, List<Folder> unclassifiedFolders)
    {
      await Logger.LogAsync($"開始創建未分類資料夾: {unclassifiedPath}", LogLevel.Info);
      try
      {
        Directory.CreateDirectory(unclassifiedPath);

        foreach (var folder in unclassifiedFolders)
        {
          string tagFolderName = _folderHandler.CreateTagFolderName(folder.Tags);
          string tagFolderPath = Path.Combine(unclassifiedPath, tagFolderName);
          Directory.CreateDirectory(tagFolderPath);

          var destinationFolder = new Folder(tagFolderPath);
          _folderHandler.MoveFolder(folder, destinationFolder);

          await Logger.LogAsync($"移動資料夾 {folder.Path} 到 {tagFolderPath}", LogLevel.Info);
        }

        await Logger.LogAsync($"完成創建未分類資料夾: {unclassifiedPath}, 移動了 {unclassifiedFolders.Count} 個資料夾", LogLevel.Info);
      }
      catch (Exception ex)
      {
        await Logger.LogExceptionAsync(ex, $"創建未分類資料夾時發生錯誤: {unclassifiedPath}");
        throw;
      }
    }
  }
}