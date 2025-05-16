using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeometricShapesApp.Models;

namespace GeometricShapesApp.ViewModels
{
    public partial class AddLineViewModel : ObservableObject
    {
        private readonly MainWindowViewModel _mainVm;
        private Window? _window;

        [ObservableProperty]
        private double _x1 = 100;

        [ObservableProperty]
        private double _y1 = 100;

        [ObservableProperty]
        private double _x2 = 200;

        [ObservableProperty]
        private double _y2 = 200;

        [ObservableProperty]
        private string _name = "Новая линия";

        public AddLineViewModel(MainWindowViewModel mainVm, Window window)
        {
            _mainVm = mainVm;
            _window = window;
        }

        [RelayCommand]
        private void AddLine()
        {
            if (string.IsNullOrWhiteSpace(Name))
                Name = "Линия";

            _mainVm.AddShape(new Line(X1, Y1, X2, Y2) { Name = Name });
            _window?.Close();
        }
    }
}