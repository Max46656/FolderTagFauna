using System;
using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;

namespace FolderSortingHat.Models
{
  public class TagCsvRecord
  {
    public string Tag { get; set; }
    public string FolderPath { get; set; }

    public TagCsvRecord(string tag, string folderPath)
    {
      Tag = tag;
      FolderPath = folderPath;
    }
  }
}