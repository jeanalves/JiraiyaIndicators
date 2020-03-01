namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators
{
    public class Point
    {
        public double Price { get; set; }
        public int BarIndex { get; set; }
        public int PointIndex { get; private set; }
        public SideSwing CurrentSideSwing { get; private set; }

        public Point(double price, int barIndex, int pointIndex, SideSwing currentSideSwing)
        {
            Price = price;
            BarIndex = barIndex;
            PointIndex = pointIndex;
            CurrentSideSwing = currentSideSwing;
        }

        public enum SideSwing
        {
            High,
            Low,
            Unknow
        }
    }
}
