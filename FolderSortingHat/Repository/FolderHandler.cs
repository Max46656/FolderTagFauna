using FolderSortingHat.Models;
using FolderSortingHat.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace FolderSortingHat.Repository
{
  public class FolderHandler
  {
    public void MoveFolder(Folder sourceFolder, Folder destFolder)
    {
      Logger.Log($"開始移動資料夾: 從 {sourceFolder.Path} 到 {destFolder.Path}", LogLevel.Info);
      try
      {
        if (!Directory.Exists(sourceFolder.Path))
        {
          throw new DirectoryNotFoundException($"來源資料夾不存在: {sourceFolder.Path}");
        }

        if (!Directory.Exists(destFolder.Path))
        {
          throw new DirectoryNotFoundException($"目標資料夾不存在: {destFolder.Path}");
        }

        string newPath = Path.Combine(destFolder.Path, Path.GetFileName(sourceFolder.Path));

        // 如果目標路徑已存在，則重命名
        if (Directory.Exists(newPath))
        {
          int counter = 1;
          string newName;
          do
          {
            newName = $"{Path.GetFileName(sourceFolder.Path)}_重複{counter}";
            newPath = Path.Combine(destFolder.Path, newName);
            counter++;
          } while (Directory.Exists(newPath));
        }

        Logger.Log($"移動資料夾: 從 {sourceFolder.Path} 到 {newPath}", LogLevel.Info);
        Directory.Move(sourceFolder.Path, newPath);
        Logger.Log($"完成移動資料夾: 從 {sourceFolder.Path} 到 {newPath}", LogLevel.Info);
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, $"移動資料夾時發生錯誤: 從 {sourceFolder.Path} 到 {destFolder.Path}");
        throw;
      }
    }
    public string CreateTagFolderName(List<string> tags)
    {
      if (tags == null || tags.Count == 0)
        return string.Empty;

      string mainTag = tags[0];
      IEnumerable<string> additionalTags = tags.Skip(1).Select(tag => $"({tag})");

      return $"[{mainTag}{string.Join("", additionalTags)}]";
    }
  }
}