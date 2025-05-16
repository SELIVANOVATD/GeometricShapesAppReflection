using System;
using System.Collections.Generic;
using System.Linq;

namespace GeometricShapesApp.Models
{
    public class Polygon : Shape
    {
        private readonly List<Vertex> _vertices = new List<Vertex>();

        public IReadOnlyList<Vertex> Vertices => _vertices.AsReadOnly();
        public int VertexCount => _vertices.Count;
        public Polygon(double x, double y, List<(double X, double Y)> vertices) : base(x, y)
        {
            _vertices = vertices?.Select(v => new Vertex(v.X, v.Y)).ToList()
                       ?? new List<Vertex>();
            Name = "Многоугольник";
        }
        // Безопасные методы для работы с вершинами
        public bool AddVertex(Vertex vertex)
        {
            if (vertex == null) return false;
            _vertices.Add(vertex);
            return true;
        }

        public bool RemoveVertexAt(int index)
        {
            if (index < 0 || index >= _vertices.Count) return false;
            _vertices.RemoveAt(index);
            return true;
        }

        public bool UpdateVertex(int index, Vertex vertex)
        {
            if (index < 0 || index >= _vertices.Count || vertex == null)
                return false;
            _vertices[index] = vertex;
            return true;
        }

        public void ReplaceAllVertices(IEnumerable<Vertex> newVertices)
        {
            _vertices.Clear();
            _vertices.AddRange(newVertices ?? throw new ArgumentNullException(nameof(newVertices)));
        }

        public override (double x1, double y1, double x2, double y2) GetBoundingBox()
        {
            double minX = double.MaxValue, minY = double.MaxValue;
            double maxX = double.MinValue, maxY = double.MinValue;

            foreach (var vertex in Vertices)
            {
                minX = Math.Min(minX, vertex.X);
                minY = Math.Min(minY, vertex.Y);
                maxX = Math.Max(maxX, vertex.X);
                maxY = Math.Max(maxY, vertex.Y);
            }

            return (minX, minY, maxX, maxY);
        }

        public override double GetArea()
        {
            double area = 0;
            int n = Vertices.Count;

            for (int i = 0; i < n; i++)
            {
                var current = Vertices[i];
                var next = Vertices[(i + 1) % n];

                area += current.X * next.Y - next.X * current.Y;
            }

            return Math.Abs(area) / 2;
        }
    }
}