namespace GeometricShapesApp.Models
{
    public class Point : Shape
    {
        public Point(double x, double y) : base(x, y) { }

        public override (double x1, double y1, double x2, double y2) GetBoundingBox()
        {
            return (X, Y, X, Y);
        }

        public override double GetArea() => 0;
    }
}