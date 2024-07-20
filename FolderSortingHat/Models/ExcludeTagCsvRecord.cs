using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace FolderSortingHat.Models
{
  public class ExcludeTagCsvRecord
  {
    [Name("excludeTag")]
    public required string ExcludeTag { get; set; }
  }
}