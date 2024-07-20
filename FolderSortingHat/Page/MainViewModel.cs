using FolderSortingHat.Services;
using FolderSortingHat.Utils;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FolderSortingHat.Page
{
  public class MainViewModel : INotifyPropertyChanged
  {
    private readonly IFolderOrganizationService _folderOrganizationService;

    private string _sourceFolderPath;
    private string _destFolderPath;
    private string _tagCSVPath;
    private bool _isOrganizing;
    private string _statusMessage;
    private bool _rescanSourceFolder;
    private List<string> _excludeTags;

    public string SourceFolderPath
    {
      get => _sourceFolderPath;
      set
      {
        _sourceFolderPath = value;
        OnPropertyChanged();
      }
    }

    public string DestFolderPath
    {
      get => _destFolderPath;
      set
      {
        _destFolderPath = value;
        OnPropertyChanged();
        UpdateTagCSVPath();
      }
    }

    public string TagCSVPath
    {
      get => _tagCSVPath;
      set { _tagCSVPath = value; OnPropertyChanged(); }
    }

    public bool IsOrganizing
    {
      get => _isOrganizing;
      set { _isOrganizing = value; OnPropertyChanged(); }
    }

    public string StatusMessage
    {
      get => _statusMessage;
      set { _statusMessage = value; OnPropertyChanged(); }
    }

    public bool IsRescanDestFolder
    {
      get => _rescanSourceFolder;
      set { _rescanSourceFolder = value; OnPropertyChanged(); }
    }

    public ICommand OrganizeFoldersCommand { get; }
    public ICommand BrowseSourceFolderCommand { get; }
    public ICommand BrowseDestFolderCommand { get; }
    public ICommand BrowseTagCSVCommand { get; }
    public ICommand ScanDestFolderPath { get; }

    public MainViewModel(IFolderOrganizationService folderOrganizationService)
    {
      try
      {
        _folderOrganizationService = folderOrganizationService;
        OrganizeFoldersCommand = new RelayCommand(async () => await OrganizeFoldersAsync(), () => !IsOrganizing);
        BrowseSourceFolderCommand = new RelayCommand(() => BrowseFolder(nameof(SourceFolderPath)));
        BrowseDestFolderCommand = new RelayCommand(() => BrowseFolder(nameof(DestFolderPath)));
        BrowseTagCSVCommand = new RelayCommand(() => BrowseTagCSV());
        ScanDestFolderPath = new RelayCommand(async () => await ScanDestFolderPathAsync(), () => !IsOrganizing);


        Logger.Log("MainViewModel 初始化");
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, "MainViewModel 初始化過程中發生異常");
        throw;
      }
    }

    private async Task ScanDestFolderPathAsync()
    {
      try
      {
        IsOrganizing = true;
        await Logger.LogAsync("開始製作地圖...", LogLevel.Info);

        StatusMessage = "獲取排除標籤...";
        _excludeTags = _folderOrganizationService.LoadOrCreateExcludeTags(Path.Combine(DestFolderPath, "excludeTag.csv"));

        if (File.Exists(TagCSVPath))
          File.Delete(TagCSVPath);
        StatusMessage = "掃描棲息地資料夾...";
        var destFolders = await _folderOrganizationService.ScanFoldersAsync(DestFolderPath, true);
        StatusMessage = "根據排除標籤排除棲息地資料夾...";
        foreach (var folder in destFolders)
        {
          folder.Tags = folder.Tags
              .Where(tag => !_excludeTags.Any(excludeTag => tag.ToLower().EndsWith(excludeTag.ToLower())))
              .ToList();
        }
        await _folderOrganizationService.SaveToCsvAsync(destFolders, TagCSVPath);

        StatusMessage = "棲息地地圖製作完成!";
        await Logger.LogAsync("棲息地資料夾位置與標籤CSV檔製作完成!", LogLevel.Info);
      }
      catch (Exception ex)
      {
        await Logger.LogExceptionAsync(ex, "資料夾歸檔時發生錯誤");
        StatusMessage = $"製作地圖錯誤: {ex.Message}";
      }
      finally
      {
        IsOrganizing = false;
      }
    }

    private async Task OrganizeFoldersAsync()
    {
      try
      {
        IsOrganizing = true;
        await Logger.LogAsync("動物遷徙開始...", LogLevel.Info);

        StatusMessage = "獲取排除標籤...";
        _excludeTags = _folderOrganizationService.LoadOrCreateExcludeTags(Path.Combine(DestFolderPath, "excludeTag.csv"));

        if (IsRescanDestFolder || !File.Exists(TagCSVPath))
        {
          if (File.Exists(TagCSVPath))
            File.Delete(TagCSVPath);
          StatusMessage = "掃描棲息地資料夾...";
          var destFolders = await _folderOrganizationService.ScanFoldersAsync(DestFolderPath, true);
          StatusMessage = "根據排除標籤排除棲息地資料夾...";
          foreach (var folder in destFolders)
          {
            folder.Tags = folder.Tags
                .Where(tag => !_excludeTags.Any(excludeTag => tag.ToLower().EndsWith(excludeTag.ToLower())))
                .ToList();
          }
          await _folderOrganizationService.SaveToCsvAsync(destFolders, TagCSVPath);
        }
        StatusMessage = "從CSV讀取資料夾資訊...";
        var allFolders = await _folderOrganizationService.LoadFromCsvAsync(TagCSVPath);

        StatusMessage = "掃描動物資料夾...";
        var sourceFolders = await _folderOrganizationService.ScanFoldersAsync(SourceFolderPath, false);

        StatusMessage = "根據排除標籤排除動物資料夾...";
        foreach (var folder in sourceFolders)
        {
          folder.Tags = folder.Tags
              .Where(tag => !_excludeTags.Any(excludeTag => tag.ToLower().EndsWith(excludeTag.ToLower())))
              .ToList();
        }

        StatusMessage = "匹配資料夾...";
        var matches = _folderOrganizationService.MatchFolders(sourceFolders, allFolders);

        StatusMessage = "移動匹配的資料夾...";
        await _folderOrganizationService.MoveFoldersAsync(matches);

        var unclassifiedFolders = sourceFolders
                 .Where(f => f.Tags.Any() && !matches.Any(m => m.SourceFolder.Path == f.Path))
                 .ToList();

        if (unclassifiedFolders.Any())
        {
          StatusMessage = "處理未分類的資料夾...";
          string unclassifiedPath = Path.Combine(DestFolderPath, "未完整分類資料夾");
          await _folderOrganizationService.CreateUnclassifiedFoldersAsync(unclassifiedPath, unclassifiedFolders);
        }

        StatusMessage = "動物回歸棲息地!";
        await Logger.LogAsync("資料夾歸檔完成!", LogLevel.Info);
      }
      catch (Exception ex)
      {
        await Logger.LogExceptionAsync(ex, "資料夾歸檔時發生錯誤");
        StatusMessage = $"動物遷徙錯誤: {ex.Message}";
      }
      finally
      {
        IsOrganizing = false;
      }
    }
    private void BrowseFolder(string propertyName)
    {
      try
      {
        var dialog = new Microsoft.Win32.OpenFolderDialog();
        bool? result = dialog.ShowDialog();
        if (result == true)
        {
          this.GetType().GetProperty(propertyName).SetValue(this, dialog.FolderName);
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, "開啟資料夾選取框失敗");
      }
    }

    private void BrowseTagCSV()
    {
      var dialog = new OpenFileDialog
      {
        Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
        InitialDirectory = Path.GetDirectoryName(TagCSVPath)
      };
      if (dialog.ShowDialog() == true)
      {
        TagCSVPath = dialog.FileName;
      }
    }

    private void UpdateTagCSVPath()
    {
      if (!string.IsNullOrEmpty(DestFolderPath))
      {
        string folderName = new DirectoryInfo(DestFolderPath).Name;
        TagCSVPath = Path.Combine(DestFolderPath, $"{folderName}.csv");
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }

  public class RelayCommand : ICommand
  {
    private readonly Action _execute;
    private readonly Func<bool> _canExecute;

    public RelayCommand(Action execute, Func<bool> canExecute = null)
    {
      _execute = execute ?? throw new ArgumentNullException(nameof(execute));
      _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

    public void Execute(object parameter) => _execute();
  }
}