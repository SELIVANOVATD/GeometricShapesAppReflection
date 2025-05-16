using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeometricShapesApp.Models;

namespace GeometricShapesApp.ViewModels
{
    public partial class AddEllipseViewModel : ObservableObject
    {
        private readonly MainWindowViewModel _mainVm;
        private Window? _window;

        [ObservableProperty]
        private double _centerX = 200;

        [ObservableProperty]
        private double _centerY = 200;

        [ObservableProperty]
        private double _radiusX = 50;

        [ObservableProperty]
        private double _radiusY = 30;

        [ObservableProperty]
        private string _name = "Новый эллипс";

        public AddEllipseViewModel(MainWindowViewModel mainVm, Window window)
        {
            _mainVm = mainVm;
            _window = window;
        }

        [RelayCommand]
        private void AddEllipse()
        {
            if (string.IsNullOrWhiteSpace(Name))
                Name = "Эллипс";

            _mainVm.AddShape(new Ellipse(CenterX, CenterY, RadiusX, RadiusY) { Name = Name });
            _window?.Close();
        }
    }
}