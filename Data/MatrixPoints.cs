using System.Collections.Generic;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators
{
    public class MatrixPoints
    {
        private readonly List<Point> pointsList = new List<Point>();
        public WhichTrendSideSignal trendSideSignal;

        public MatrixPoints() { }

        public MatrixPoints(List<Point> pointsList, WhichTrendSideSignal trendSideSignal)
        {
            this.pointsList = pointsList;
            this.trendSideSignal = trendSideSignal;
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

        public enum WhichTrendSideSignal
        {
            Bullish,
            Bearish,
            Both,
            None
        }
    }
}
