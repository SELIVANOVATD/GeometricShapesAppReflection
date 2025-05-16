using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeometricShapesApp.Models;
using ShapeReflectionApp.Views;

namespace ShapeReflectionApp.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? _assemblyPath;

        [ObservableProperty]
        private ObservableCollection<string> _availableClasses = new();

        [ObservableProperty]
        private string? _selectedClass;

        [ObservableProperty]
        private ObservableCollection<MethodInfo> _availableMethods = new();

        [ObservableProperty]
        private MethodInfo? _selectedMethod;

        [ObservableProperty]
        private ObservableCollection<ParameterViewModel> _methodParameters = new();

        [ObservableProperty]
        private object? _methodResult;

        private Assembly? _loadedAssembly;
        private Type[]? _shapeTypes;
        

    public MainWindowViewModel()
        {
            // Убрали автоматическую загрузку
            AssemblyPath = string.Empty;
        }
        [RelayCommand]
        private void LoadAssembly()
        {
            if (string.IsNullOrWhiteSpace(AssemblyPath)) return;

            try
            {
                _loadedAssembly = Assembly.LoadFrom(AssemblyPath);
                _shapeTypes = _loadedAssembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && typeof(Shape).IsAssignableFrom(t))
                    .ToArray();

                AvailableClasses.Clear();
                foreach (var type in _shapeTypes)
                {
                    AvailableClasses.Add(type.Name);
                }

                MethodResult = "Библиотека успешно загружена. Доступные классы загружены.";
            }
            catch (Exception ex)
            {
                MethodResult = $"Ошибка загрузки библиотеки: {ex.Message}";
            }
        }

[RelayCommand]
private void SelectClass()
{
    if (_shapeTypes == null || string.IsNullOrWhiteSpace(SelectedClass)) return;

    var selectedType = _shapeTypes?.FirstOrDefault(t => t.Name == SelectedClass);
    if (selectedType == null) return;

    // Создаём экземпляр только если он ещё не создан или если тип изменился
    if (_currentInstance == null || _currentInstance.GetType() != selectedType)
    {
        _currentInstance = CreateShapeInstance(selectedType);
        
        if (_currentInstance is Shape shape)
        {
            shape.Name = $"Динамически созданный {selectedType.Name}";
        }
    }

    AvailableMethods.Clear();
    var methods = selectedType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
        .Where(m => !m.IsSpecialName && m.DeclaringType != typeof(object));

    foreach (var method in methods)
    {
        AvailableMethods.Add(method);
    }

    MethodResult = $"Выбран класс {SelectedClass}. Доступные методы загружены.";
}

        [RelayCommand]
        private void SelectMethod()
        {
            if (SelectedMethod == null) return;

            MethodParameters.Clear();
            foreach (var parameter in SelectedMethod.GetParameters())
            {
                MethodParameters.Add(new ParameterViewModel
                {
                    Name = parameter.Name ?? "unnamed",
                    Type = parameter.ParameterType,
                    Value = ParameterViewModel.GetDefaultValue(parameter.ParameterType),
                    IsReadOnly = parameter.ParameterType == typeof(bool)
                });
            }

            MethodResult = $"Выбран метод {SelectedMethod.Name}. Введите параметры.";
        }

[RelayCommand]
private void ExecuteMethod()
{
    if (SelectedMethod == null || _currentInstance == null) return;

    try
    {
        var methodParameters = MethodParameters
            .Select(p => Convert.ChangeType(p.Value, p.Type))
            .ToArray();

        var result = SelectedMethod.Invoke(_currentInstance, methodParameters);
        MethodResult = result?.ToString() ?? "Метод выполнен успешно (возвращаемое значение null)";
    }
    catch (Exception ex)
    {
        MethodResult = $"Ошибка выполнения метода: {ex.Message}";
    }
}

         private object CreateShapeInstance(Type shapeType)
        {
            if (shapeType == typeof(GeometricShapesApp.Models.Point))
                return new GeometricShapesApp.Models.Point(0.0, 0.0);
            if (shapeType == typeof(GeometricShapesApp.Models.Line))
                return new GeometricShapesApp.Models.Line(0.0, 0.0, 100.0, 100.0);
            if (shapeType == typeof(GeometricShapesApp.Models.Ellipse))
                return new GeometricShapesApp.Models.Ellipse(100.0, 100.0, 50.0, 30.0);
            if (shapeType == typeof(GeometricShapesApp.Models.Polygon))
            {
                var vertices = new List<(double, double)> { (100, 100), (150, 150), (200, 100) };
                return new GeometricShapesApp.Models.Polygon(0.0, 0.0, vertices);
            }

            var constructors = shapeType.GetConstructors();
            if (constructors.Length == 0)
                throw new Exception("Не удалось найти подходящий конструктор");

            var firstConstructor = constructors[0];
            var parameters = firstConstructor.GetParameters()
                .Select(p => GetDefaultValue(p.ParameterType))
                .ToArray();

            return firstConstructor.Invoke(parameters);
        }
        [RelayCommand]
        private void DrawShape()
        {
            if (_currentInstance is not Shape shape)
            {
                MethodResult = "Нет фигуры для отрисовки";
                return;
            }

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.DrawShape(shape);
                    MethodResult = $"Фигура {shape.Name} отрисована";
                }
            }
        }
 [RelayCommand]
private async Task SelectAssemblyFile()
{
    if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop || 
        desktop.MainWindow is null)
    {
        MethodResult = "Не удалось получить доступ к главному окну";
        return;
    }

    var files = await desktop.MainWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
    {
        Title = "Выберите сборку с фигурами",
        AllowMultiple = false,
        FileTypeFilter = new[]
        {
            new FilePickerFileType("DLL Files") { Patterns = new[] { "*.dll" } },
            new FilePickerFileType("All Files") { Patterns = new[] { "*" } }
        }
    });

    if (files.Count > 0 && files[0] is { } file)
    {
        try
        {
            AssemblyPath = file.Path.LocalPath;
            LoadAssemblyCommand.Execute(null);
        }
        catch (Exception ex)
        {
            MethodResult = $"Ошибка при выборе файла: {ex.Message}";
        }
    }
}
[RelayCommand]
private void ClearCanvas()
{
    if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
        if (desktop.MainWindow is MainWindow mainWindow)
        {
            var canvas = mainWindow.FindControl<Canvas>("DrawingCanvas");
            canvas?.Children.Clear();
            MethodResult = "Холст очищен";
        }
    }
}

[RelayCommand]
private void UpdateShapeProperties()
{
    if (_currentInstance == null) return;

    try
    {
        var properties = _currentInstance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (var prop in properties)
        {
            if (!prop.CanWrite) continue;
            
            var param = MethodParameters.FirstOrDefault(p => p.Name == prop.Name);
            if (param != null)
            {
                var value = Convert.ChangeType(param.Value, prop.PropertyType);
                prop.SetValue(_currentInstance, value);
            }
        }
        
        MethodResult = "Свойства фигуры успешно обновлены";
    }
    catch (Exception ex)
    {
        MethodResult = $"Ошибка обновления свойств: {ex.Message}";
    }
}

private object? _currentInstance;

        [RelayCommand]
        private void ShowShapeProperties()
        {
            if (_currentInstance == null) return;

            MethodParameters.Clear();
            var properties = _currentInstance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                if (!prop.CanWrite) continue;

                MethodParameters.Add(new ParameterViewModel
                {
                    Name = prop.Name,
                    Type = prop.PropertyType,
                    Value = prop.GetValue(_currentInstance),
                    IsReadOnly = false
                });
            }

            MethodResult = $"Свойства {_currentInstance.GetType().Name} загружены";
        }

        private static object? GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }

    public partial class ParameterViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _name = string.Empty;
        
        [ObservableProperty]
        private Type _type = typeof(object);
        
        [ObservableProperty]
        private object? _value;
        
        [ObservableProperty]
        private bool _isReadOnly;

        public string TypeName => Type.Name;

        public static object? GetDefaultValue(Type type)
        {
            if (type == typeof(string))
                return string.Empty;
            if (type.IsValueType)
                return Activator.CreateInstance(type);
            return null;
        }
    }
}