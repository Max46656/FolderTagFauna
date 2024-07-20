using FolderSortingHat.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using FolderSortingHat.Utils;
using System.Text;

namespace FolderSortingHat.Repository
{
  public class CsvManager
  {
    public void SaveToCsv(List<Folder> folders, string filePath)
    {
      Dictionary<string, string>? tagPathDictionary = folders
          .SelectMany(folder => folder.Tags.Select(tag => new { Tag = tag, Path = folder.Path }))
          .GroupBy(item => item.Tag)
          .OrderBy(group => group.Key, StringComparer.OrdinalIgnoreCase)
          .ToDictionary(
              group => group.Key,
              group => group.OrderBy(item => item.Path.Length).First().Path
          );

      var records = tagPathDictionary.Select(kvp => new TagCsvRecord(kvp.Key, kvp.Value));

      try
      {
        using (var writer = new StreamWriter(filePath, false, Encoding.Unicode))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
          csv.WriteRecords(records);
          Logger.Log($"成功將資料保存到 CSV 文件：{filePath}");
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, $"保存資料到 CSV 文件時發生異常：{filePath}");
        throw;
      }
    }


    public List<Folder> LoadFromCsv(string filePath)
    {
      var records = new List<TagCsvRecord>();

      try
      {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
          PrepareHeaderForMatch = args => args.Header.ToLower(),
        };

        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, config))
        {
          records = csv.GetRecords<TagCsvRecord>().ToList();
          Logger.Log($"從 CSV 文件成功加載了 {records.Count} 條記錄：{filePath}");
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, $"從 CSV 文件加載記錄時發生異常：{filePath}");
        throw;
      }

      var folderDict = new Dictionary<string, Folder>();

      foreach (var record in records)
      {
        if (!folderDict.ContainsKey(record.FolderPath))
        {
          folderDict[record.FolderPath] = new Folder(record.FolderPath)
          {
            Tags = new List<string>()
          };
        }

        folderDict[record.FolderPath].Tags.Add(record.Tag);
      }

      return folderDict.Values.ToList();
    }
    public List<string> LoadExcludeTags(string filePath)
    {
      if (!File.Exists(filePath))
      {
        using (var writer = new StreamWriter(filePath, false, Encoding.Unicode))
        {
          writer.WriteLine("excludeTag");
        }
      }
      var records = new List<ExcludeTagCsvRecord>();
      var config = new CsvConfiguration(CultureInfo.InvariantCulture)
      {
        PrepareHeaderForMatch = args => args.Header.ToLower(),
      };

      using (var reader = new StreamReader(filePath))
      using (var csv = new CsvReader(reader, config))
      {
        records = csv.GetRecords<ExcludeTagCsvRecord>().ToList();
      }

      return records.Select(r => r.ExcludeTag).ToList();
    }
  }
}

