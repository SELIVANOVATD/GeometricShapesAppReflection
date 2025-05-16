namespace GeometricShapesApp.Models
{
    public abstract class Shape
    {
        public double X { get; set; }
        public double Y { get; set; }
        public string Name { get; set; }

        protected Shape(double x, double y)
        {
            X = x;
            Y = y;
            Name = string.Empty;
        }

        public abstract (double x1, double y1, double x2, double y2) GetBoundingBox();
        public abstract double GetArea();
    }
}