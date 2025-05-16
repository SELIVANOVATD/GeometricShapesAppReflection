using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Markup.Xaml;
using GeometricShapesApp.Models;
using System;
using System.Linq;
using Avalonia.Input;

namespace ShapeReflectionApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void DrawShape(GeometricShapesApp.Models.Shape shape)
        {
            var canvas = this.FindControl<Canvas>("DrawingCanvas");
            canvas?.Children.Clear();

            try
            {
                if (shape is GeometricShapesApp.Models.Point point)
                {
                    var dot = new Avalonia.Controls.Shapes.Ellipse
                    {
                        Width = 6,
                        Height = 6,
                        Fill = Brushes.Red,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1
                    };
                    Canvas.SetLeft(dot, point.X - 3);
                    Canvas.SetTop(dot, point.Y - 3);
                    canvas?.Children.Add(dot);
                }
                else if (shape is GeometricShapesApp.Models.Line line)
                {
                    var lineShape = new Avalonia.Controls.Shapes.Line
                    {
                        StartPoint = new global::Avalonia.Point(line.X, line.Y),
                        EndPoint = new global::Avalonia.Point(line.X2, line.Y2),
                        Stroke = Brushes.Black,
                        StrokeThickness = 2
                    };
                    canvas?.Children.Add(lineShape);
                }
                else if (shape is GeometricShapesApp.Models.Ellipse ellipse)
                {
                    var ellipseShape = new Avalonia.Controls.Shapes.Ellipse
                    {
                        Width = ellipse.RadiusX * 2,
                        Height = ellipse.RadiusY * 2,
                        Stroke = Brushes.Black,
                        StrokeThickness = 2,
                        Fill = Brushes.LightBlue
                    };
                    Canvas.SetLeft(ellipseShape, ellipse.X - ellipse.RadiusX);
                    Canvas.SetTop(ellipseShape, ellipse.Y - ellipse.RadiusY);
                    canvas?.Children.Add(ellipseShape);
                }
                else if (shape is GeometricShapesApp.Models.Polygon polygon)
                {
                    var polygonShape = new Avalonia.Controls.Shapes.Polygon
                    {
                        Points = new global::Avalonia.Points(polygon.Vertices.Select(v => 
                            new global::Avalonia.Point(v.X, v.Y))),
                        Stroke = Brushes.Black,
                        StrokeThickness = 2,
                        Fill = Brushes.LightGreen
                    };
                    canvas?.Children.Add(polygonShape);
                }

                // Отрисовка bounding box
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
                canvas?.Children.Add(rect);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отрисовки: {ex.Message}");
            }
        }
    }
}