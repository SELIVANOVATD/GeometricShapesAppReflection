using System;

namespace GeometricShapesApp.Models
{
    public class Line : Shape
    {
        public double X2 { get; set; }
        public double Y2 { get; set; }

        public Line(double x1, double y1, double x2, double y2) : base(x1, y1)
        {
            X2 = x2;
            Y2 = y2;
        }

        public override (double x1, double y1, double x2, double y2) GetBoundingBox()
        {
            return (Math.Min(X, X2), Math.Min(Y, Y2), Math.Max(X, X2), Math.Max(Y, Y2));
        }

        public override double GetArea() => 0;
    }
}