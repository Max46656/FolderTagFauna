using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

namespace FolderSortingHat.Models
{
  public class Folder
  {
    public List<string> Tags { get; set; }
    public string Path { get; set; }
    public DateTime LastModified { get; set; }

    public Folder(string folderPath)
    {
      Path = folderPath;

      // 使用 UTF-8 解碼資料夾名稱
      var folderName = System.IO.Path.GetFileName(folderPath);
      var utf8FolderName = Encoding.UTF8.GetString(Encoding.Default.GetBytes(folderName));

      Tags = ExtractTags(utf8FolderName);
      LastModified = Directory.GetLastWriteTime(folderPath);
    }

    private List<string> ExtractTags(string folderName)
    {
      var tags = new List<string>();

      // 提取 [xxx] 形式的標籤
      var regex = new Regex(@"\[(.*?)\]");
      var matches = regex.Matches(folderName);
      foreach (Match match in matches)
      {
        var tag = match.Groups[1].Value.Trim();
        if (!string.IsNullOrWhiteSpace(tag))
        {
          // 處理 [xxx(aaa)（zzz）] 形式的標籤
          var innerTags = ExtractInnerTags(tag);
          tags.AddRange(innerTags);
        }
      }

      return tags.Distinct().ToList();
    }

    private List<string> ExtractInnerTags(string tag)
    {
      var innerTags = new List<string>();

      // 提取 (xxx) 和 （xxx） 形式的內部標籤
      var regex = new Regex(@"[\(（](.*?)[\)）]");
      var matches = regex.Matches(tag);
      foreach (Match match in matches)
      {
        var innerTag = match.Groups[1].Value.Trim();
        if (!string.IsNullOrWhiteSpace(innerTag))
          innerTags.Add(innerTag);
      }

      // 提取剩餘的外部標籤
      var outerTag = regex.Replace(tag, "").Trim('(', ')', '（', '）', ' ');
      if (!string.IsNullOrWhiteSpace(outerTag))
        innerTags.Add(outerTag);

      return innerTags;
    }
  }
}