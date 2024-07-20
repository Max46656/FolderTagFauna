using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FolderSortingHat.Models;

namespace FolderSortingHat.Repository
{
  public class FolderScanner
  {
    public List<Folder> ScanFolder(string folderPath, bool includeFormattedOnly = false)
    {
      //並行處理
      var folders = new ConcurrentBag<Folder>();
      //搜尋子目錄
      var searchOption = SearchOption.AllDirectories;

      // 只處理資料夾名稱僅符合 [xxx] 格式的資料夾，即只處理Tag資料夾
      var regex = new Regex(@"^\[(.*?)\]$", RegexOptions.Compiled);

      // 並行處理資料夾掃描
      Parallel.ForEach(Directory.GetDirectories(folderPath, "*", searchOption), directory =>
      {
        // 根據 includeFormattedOnly 參數過濾資料夾
        if (includeFormattedOnly && !regex.IsMatch(Path.GetFileName(directory)))
          return;
        folders.Add(new Folder(directory));
      });

      return folders.ToList(); // 將 ConcurrentBag 轉換回 List
    }
  }
}