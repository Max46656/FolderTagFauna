using FolderSortingHat.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FolderSortingHat.Repository
{
  public class FolderTagMatcher
  {
    public List<(Folder SourceFolder, Folder DestFolder)> MatchFolders(List<Folder> sourceFolders, List<Folder> destFolders)
    {
      var matches = new List<(Folder SourceFolder, Folder DestFolder)>();

      foreach (var sourceFolder in sourceFolders)
      {
        var lowerSourceTags = sourceFolder.Tags.Select(tag => tag.ToLower()).ToList();

        foreach (var destFolder in destFolders)
        {
          var lowerDestTags = destFolder.Tags.Select(tag => tag.ToLower()).ToList();

          if (lowerSourceTags.Intersect(lowerDestTags).Any())
          {
            matches.Add((sourceFolder, destFolder));
            break;
          }
        }
      }

      return matches;
    }
  }
}