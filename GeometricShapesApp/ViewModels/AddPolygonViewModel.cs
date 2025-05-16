using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeometricShapesApp.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System;

namespace GeometricShapesApp.ViewModels
{
    public partial class AddPolygonViewModel : ObservableObject
    {
        private readonly MainWindowViewModel _mainVm;
        private readonly Window _window;

        [ObservableProperty]
        private string _name = "Новый многоугольник";

        [ObservableProperty]
        private double _newVertexX;

        [ObservableProperty]
        private double _newVertexY;

        [ObservableProperty]
        private ObservableCollection<Vertex> _vertices = new();

        public AddPolygonViewModel(MainWindowViewModel mainVm, Window window)
        {
            _mainVm = mainVm;
            _window = window;

            // Инициализация начальными вершинами
            Vertices.Add(new Vertex(200, 200));
            Vertices.Add(new Vertex(250, 250));
            Vertices.Add(new Vertex(300, 200));
        }

        [RelayCommand]
        private void AddVertex()
        {
            Vertices.Add(new Vertex(NewVertexX, NewVertexY));
            NewVertexX = NewVertexY = 0;
        }

        [RelayCommand]
        private void RemoveVertex()
        {
            if (Vertices.Count > 0)
                Vertices.RemoveAt(Vertices.Count - 1);
        }

        [RelayCommand]
        private void AddPolygon()
        {
            if (Vertices.Count < 3) return;

            var verticesList = Vertices.Select(v => (v.X, v.Y)).ToList();
            _mainVm.AddShape(new Polygon(0, 0, verticesList) { Name = Name });
            _window.Close();
        }
    }
}