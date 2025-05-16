using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeometricShapesApp.Models;
using Avalonia.Controls;
using System;

namespace GeometricShapesApp.ViewModels
{
    public partial class AddPointViewModel : ObservableObject
    {
        private readonly MainWindowViewModel _mainVm;
        private Window? _window;

        [ObservableProperty]
        private double _x = 100;

        [ObservableProperty]
        private double _y = 100;

        [ObservableProperty]
        private string _name = "Новая точка";

        public AddPointViewModel(MainWindowViewModel mainVm, Window window)
        {
            _mainVm = mainVm;
            _window = window;
        }

        [RelayCommand]
        private void AddPoint()
        {
            if (string.IsNullOrWhiteSpace(Name))
                Name = "Точка";

            _mainVm.AddShape(new Point(X, Y) { Name = Name });
            _window?.Close();
        }
    }
}