using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FolderSortingHat.Models;

namespace FolderSortingHat.Services
{
  public interface IFolderOrganizationService
  {
    Task<List<Folder>> ScanFoldersAsync(string folderPath, bool includeFormattedOnly = false);

    Task SaveToCsvAsync(List<Folder> folders, string filePath);
    Task<List<Folder>> LoadFromCsvAsync(string filePath);
    List<(Folder SourceFolder, Folder DestFolder)> MatchFolders(List<Folder> sourceFolders, List<Folder> destFolders);
    Task MoveFoldersAsync(List<(Folder SourceFolder, Folder DestFolder)> matches);
    public List<string> LoadOrCreateExcludeTags(string filePath);
    Task CreateUnclassifiedFoldersAsync(string basePath, List<Folder> unclassifiedFolders);
  }
}