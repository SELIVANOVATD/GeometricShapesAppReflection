using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeometricShapesApp.Models;
using GeometricShapesApp.Views;

namespace GeometricShapesApp.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Shape> _shapes;

        public MainWindowViewModel()
        {
            Shapes = new ObservableCollection<Shape>
            {
                new Point(100, 100) { Name = "Точка" },
                new Line(150, 150, 300, 300) { Name = "Линия" },
                new Ellipse(400, 200, 50, 50) { Name = "Эллипс" },
                new Polygon(0, 0, new List<(double X, double Y)>
                {
                    (200, 200),
                    (250, 250),
                    (300, 200),
                    (250, 150)
                }) { Name = "Многоугольник" }
            };

            Shapes.CollectionChanged += Shapes_CollectionChanged;
        }

        private void Shapes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Shapes));
        }

        [RelayCommand]
        private void AddPoint()
        {
            var window = new AddPointWindow();
            window.DataContext = new AddPointViewModel(this, window);
            window.Show();
        }

        [RelayCommand]
        private void AddLine()
        {
            var window = new AddLineWindow();
            window.DataContext = new AddLineViewModel(this, window);
            window.Show();
        }

        [RelayCommand]
        private void AddEllipse()
        {
            var window = new AddEllipseWindow();
            window.DataContext = new AddEllipseViewModel(this, window);
            window.Show();
        }

        [RelayCommand]
        private void AddPolygon()
        {
            var window = new AddPolygonWindow();
            window.DataContext = new AddPolygonViewModel(this, window);
            window.Show();
        }

        public void AddShape(Shape shape)
        {
            Shapes.Add(shape);
        }
    }
}