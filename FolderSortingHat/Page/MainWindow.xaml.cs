using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FolderSortingHat.Page;
using FolderSortingHat.Utils;

namespace FolderSortingHat
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window

  {
    public MainWindow(MainViewModel mainViewModel)
    {
      try
      {
        InitializeComponent();
        DataContext = mainViewModel;
        Logger.Log("主視窗初始化");
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, "主視窗初始化過程中發生異常");
        throw;
      }

    }

  }
}