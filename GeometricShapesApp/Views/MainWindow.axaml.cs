using Avalonia.Controls;
using Avalonia.Media;
using GeometricShapesApp.ViewModels;
using Avalonia.Layout;
using System;
using Avalonia.Interactivity;
using System.Linq;
using System.Reactive.Linq;
using Avalonia;

namespace GeometricShapesApp.Views
{
    public partial class MainWindow : Window
    {
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            DrawShapes();
        }
        public MainWindow()
        {
            InitializeComponent();

            this.GetObservable(DataContextProperty).Subscribe(_ =>
            {
                if (DataContext is MainWindowViewModel vm)
                {
                    vm.Shapes.CollectionChanged += (s, e) =>
                    {
                        DrawShapes();
                    };
                }

                DrawShapes();
            });
        }

        public void DrawShapes()
        {

            if (DataContext is not MainWindowViewModel vm)
            {
                return;
            }

            DrawingCanvas.Children.Clear();

            foreach (var shape in vm.Shapes)
            {
                try
                {

                    if (shape is GeometricShapesApp.Models.Point point)
                    {

                        var dot = new Avalonia.Controls.Shapes.Ellipse
                        {
                            Width = 5,
                            Height = 5,
                            Fill = Brushes.Red,
                            Stroke = Brushes.Black,
                            StrokeThickness = 1,
                            DataContext = point
                        };

                        Canvas.SetLeft(dot, point.X - 2.5);
                        Canvas.SetTop(dot, point.Y - 2.5);
                        DrawingCanvas.Children.Add(dot);

                    }
                    else if (shape is GeometricShapesApp.Models.Line line)
                    {

                        var lin = new Avalonia.Controls.Shapes.Line
                        {
                            StartPoint = new Avalonia.Point(line.X, line.Y),
                            EndPoint = new Avalonia.Point(line.X2, line.Y2),
                            Stroke = Brushes.Black,
                            StrokeThickness = 2,
                            DataContext = line
                        };
                        DrawingCanvas.Children.Add(lin);

                    }
                    else if (shape is GeometricShapesApp.Models.Ellipse ellipse)
                    {
                        var ell = new Avalonia.Controls.Shapes.Ellipse
                        {
                            Width = ellipse.RadiusX * 2,
                            Height = ellipse.RadiusY * 2,
                            Stroke = Brushes.Black,
                            StrokeThickness = 2,
                            Fill = Brushes.LightBlue,
                            DataContext = ellipse
                        };

                        double left = ellipse.X - ellipse.RadiusX;
                        double top = ellipse.Y - ellipse.RadiusY;
                        Canvas.SetLeft(ell, left);
                        Canvas.SetTop(ell, top);
                        DrawingCanvas.Children.Add(ell);

                    }
                    else if (shape is GeometricShapesApp.Models.Polygon polygon)
                    {

                        var points = polygon.Vertices.Select(v => new Avalonia.Point(v.X, v.Y)).ToArray();

                        var poly = new Avalonia.Controls.Shapes.Polygon
                        {
                            Points = new Avalonia.Point[0],
                            Stroke = Brushes.Black,
                            StrokeThickness = 2,
                            Fill = Brushes.LightGreen,
                            DataContext = polygon
                        };


                        poly.Points = points;

                        DrawingCanvas.Children.Add(poly);
                    }
                    var boundingBox = shape.GetBoundingBox();
            var rect = new Avalonia.Controls.Shapes.Rectangle
            {
                Width = boundingBox.x2 - boundingBox.x1,
                Height = boundingBox.y2 - boundingBox.y1,
                Stroke = Brushes.Red,
                StrokeThickness = 1,
                StrokeDashArray = new Avalonia.Collections.AvaloniaList<double> { 4, 2 },
                Fill = Brushes.Transparent
            };
            Canvas.SetLeft(rect, boundingBox.x1);
            Canvas.SetTop(rect, boundingBox.y1);
            DrawingCanvas.Children.Add(rect);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to draw shape {shape.Name}: {ex}");
                }
            }

            foreach (var child in DrawingCanvas.Children)
            {
                Console.WriteLine($"[DrawShapes] Canvas child: {child.GetType().Name} " +
                    $"at ({Canvas.GetLeft(child)}, {Canvas.GetTop(child)})");
            }

            // Обновление информации о фигурах
            DisplayShapeInfo();
        }

        private void DisplayShapeInfo()
{
    if (DataContext is MainWindowViewModel vm)
    {
        InfoPanel.Children.Clear();

        foreach (var shape in vm.Shapes)
        {
            var boundingBox = shape.GetBoundingBox();
            var infoText = new TextBlock
            {
                Text = $"{shape.Name}: Площадь = {shape.GetArea():F2}, " +
                      $"Ограничивающий прямоугольник: ({boundingBox.x1:F0}, {boundingBox.y1:F0}) - ({boundingBox.x2:F0}, {boundingBox.y2:F0})",
                FontSize = 14,
                Margin = new Avalonia.Thickness(0, 5),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            InfoPanel.Children.Add(infoText);
        }
    }
}
    }

}