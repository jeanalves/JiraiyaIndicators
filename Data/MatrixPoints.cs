using System.Collections.Generic;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators
{
    public class MatrixPoints
    {
        private readonly List<Point> pointsList = new List<Point>();
        public int index;
        public WhichTrendSideSignal trendSideSignal;
        public WhichGraphicPatternType graphicPatternType;

        // Initialization

        public MatrixPoints(WhichGraphicPatternType graphicPatternType)
        {
            this.graphicPatternType = graphicPatternType;
        }

        public MatrixPoints(List<Point> pointsList, int index, WhichTrendSideSignal trendSideSignal, WhichGraphicPatternType graphicPatternType)
        {
            this.pointsList = pointsList;
            this.index = index;
            this.trendSideSignal = trendSideSignal;
            this.graphicPatternType = graphicPatternType;
        }

        // Public (methods)

        public void AddPoint(Point point)
        {
            pointsList.Add(point);
        }

        // Properties

        public List<Point> PointsList
        {
            get
            {
                return pointsList;
            }

            private set { }
        }

        // Miscellaneous

        public enum WhichTrendSideSignal
        {
            Bullish,
            Bearish,
            Both,
            None
        }

        public enum WhichGraphicPatternType
        {
            Trend,
            Pivot
        }
    }
}
