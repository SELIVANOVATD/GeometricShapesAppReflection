
namespace GeometricShapesApp.Models
{
    public class Vertex
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vertex(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X}, {Y})";
    }
}