using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using FolderSortingHat.Services;
using FolderSortingHat.Utils;
using FolderSortingHat.Repository;
using FolderSortingHat.Page;
using System.Reflection;
using System.Runtime.Versioning;
namespace FolderSortingHat
{
  public partial class App : Application
  {
    private ServiceProvider _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
      AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
      DispatcherUnhandledException += App_DispatcherUnhandledException;

      base.OnStartup(e);

      try
      {
        Logger.Log("應用程式啟動");

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        _serviceProvider = serviceCollection.BuildServiceProvider();
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        Logger.Log("主視窗初始化完成，顯示主視窗");
        mainWindow.Show();
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, "應用程式啟動過程中發生異常");
        throw;
      }
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      Logger.LogException(e.ExceptionObject as Exception, "未處理的應用程式域異常");
    }

    private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
      Logger.LogException(e.Exception, "未處理的調度程序異常");
      e.Handled = true;
    }

    private void ConfigureServices(IServiceCollection services)
    {
      try
      {
        Logger.Log("配置依賴服務");

        services.AddSingleton<IFolderOrganizationService, FolderOrganizationService>();
        services.AddSingleton<FolderScanner>();
        services.AddSingleton<CsvManager>();
        services.AddSingleton<FolderTagMatcher>();
        services.AddSingleton<FolderHandler>();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<MainWindow>();

        Logger.Log("依賴服務配置完成");
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, "配置依賴服務過程中發生異常");
        throw;
      }
    }
  }

}