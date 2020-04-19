namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators
{
    public class CalculationData
    {
        public bool IsNewSwing { get; private set; }
        public double Price { get; private set; }
        public int BarIndex { get; private set; }
        public Point.SidePoint SideSwing { get; private set; }

        /// <summary>
        /// Constructor used to pass informations
        /// </summary>
        /// <param name="isNewSwing"></param>
        /// <param name="price"></param>
        /// <param name="barIndex"></param>
        /// <param name="sideSwing"></param>
        public CalculationData(bool isNewSwing, double price, int barIndex, Point.SidePoint sideSwing)
        {
            IsNewSwing = isNewSwing;
            Price = price;
            BarIndex = barIndex;
            SideSwing = sideSwing;
        }

        /// <summary>
        ///  This method constructor should only be used for situations where you do not have data to pass
        /// </summary>
        /// <param name="isNewSwing"></param>
        public CalculationData(bool isNewSwing)
        {
            if (isNewSwing)
                throw new System.Exception("isNewSwing must be false");

            IsNewSwing = isNewSwing;
            Price = 0;
            BarIndex = 0;
            SideSwing = Point.SidePoint.Unknow;
        }
    }
}
