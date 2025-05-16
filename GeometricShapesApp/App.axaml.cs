using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GeometricShapesApp.ViewModels;
using GeometricShapesApp.Views;
using System;

namespace GeometricShapesApp
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {

                var viewModel = new MainWindowViewModel();

                var mainWindow = new MainWindow
                {
                    DataContext = viewModel
                };

                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}