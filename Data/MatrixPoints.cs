using System.Collections.Generic;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators
{
    public class MatrixPoints
    {
        private readonly List<Point> pointsList = new List<Point>();

        public MatrixPoints() { }

        public MatrixPoints(List<Point> pointsList)
        {
            this.pointsList = pointsList;
        }

        public void AddPoint(Point point)
        {
            pointsList.Add(point);
        }

        public List<Point> PointsList
        {
            get
            {
                return pointsList;
            }
        }
    }
}
