namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators
{
    public class Pivot
    {
        public Point FirstPoint { get; set; }
        public Point SecondPoint { get; set; }
        public Point ThirdyPoint { get; set; }
        public Point FortyPoint { get; set; }
        public SidePivot CurrentSidePivot { get; set; }

        public Pivot(Point firstPoint, Point secondPoint, Point thirdyPoint, Point fortyPoint, SidePivot sidePivot)
        {
            FirstPoint = firstPoint;
            SecondPoint = secondPoint;
            ThirdyPoint = thirdyPoint;
            FortyPoint = fortyPoint;
            CurrentSidePivot = sidePivot;
        }

        public enum SidePivot
        {
            High,
            Low,
            Unknow
        }
    }
}
