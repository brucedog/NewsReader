using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using NewsReader.DataStorage.Interfaces.Configuration;
using NewsReader.DataStorage.Interfaces.UnitOfWork;
using NewsReader.DataStorage.SqlLite;
using NewsReader.Models;
using NewsReader.ViewModels;
using NewsReader.Views;
using Splat;
namespace NewsReader;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        RegisterServicesDependency(Locator.CurrentMutable, Locator.Current);
        var mainViewModel = new MainViewModel(Locator.Current.GetService<IUnitOfWorkFactory>());

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
            
            SetupDesktop(desktop, Locator.Current.GetService<IUnitOfWorkFactory>());
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = mainViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void SetupDesktop(IClassicDesktopStyleApplicationLifetime desktop, IUnitOfWorkFactory unitOfWorkFactory)
    {
        try
        {
            var uow = unitOfWorkFactory.Create();
            var repo = uow.GetRepository<Settings>();
            var settings = repo.GetById(Settings.AppSettings);
            var settingSetup = settings ?? new Settings();

            desktop.Startup += delegate
            {
                if (desktop.MainWindow != null)
                {
                    desktop.MainWindow.Position = new PixelPoint(settingSetup.MainWindowInfo.TopX,
                        settingSetup.MainWindowInfo.TopY);
                    desktop.MainWindow.Height = settingSetup.MainWindowInfo.Height;
                    desktop.MainWindow.Width = settingSetup.MainWindowInfo.Width;

                    desktop.MainWindow.Resized += delegate(object? sender, WindowResizedEventArgs args)
                    {
                        settingSetup.MainWindowInfo.Height = (int)args.ClientSize.Height;
                        settingSetup.MainWindowInfo.Width = (int)args.ClientSize.Width;
                    };
                    desktop.MainWindow.PositionChanged += delegate(object? sender, PixelPointEventArgs args)
                    {
                        settingSetup.MainWindowInfo.TopX = args.Point.X;
                        settingSetup.MainWindowInfo.TopY = args.Point.Y;
                    };
                }
            };
            desktop.ShutdownRequested += delegate
            {
                repo.Upsert(Settings.AppSettings, settingSetup);
            };
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private void RegisterServicesDependency(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        var assemblyLocation = Assembly.GetEntryAssembly()?.Location;
        var dbDirectory = Path.GetDirectoryName(assemblyLocation);


        if (!Directory.Exists(dbDirectory))
        {
            Directory.CreateDirectory(dbDirectory);
        }

        var contect = Path.Combine(dbDirectory, "TestingDb");
        var config = new DatabaseConfiguration { ConnectionString = $"FileName={contect};Connection=Shared" };

        services.RegisterLazySingleton<IUnitOfWorkFactory>(() => new LiteDbUnitOfWorkFactory(config));
        //services
    }
}
