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
            PointList.Add(point);
        }

        public List<Point> PointList
        {
            get
            {
                return pointsList;
            }
        }
    }
}
