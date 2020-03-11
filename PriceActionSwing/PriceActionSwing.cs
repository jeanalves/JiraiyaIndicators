using NinjaTrader.NinjaScript;

namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public class PriceActionSwing
    {
        public NinjaScriptBase owner;

        public bool UseHighLow { get; set; }

        public PriceActionSwing(NinjaScriptBase owner)
        {
            this.owner = owner;
        }

        public void OnPointCalculationUpdate(int pointsCount, Point pointOne, Point pointTwo)
        {
            // Every time a new point event happens, it will be drawn in this method
            SwingDrawing.DrawPoint(owner, pointTwo);

            if (pointsCount > 1)
            {
                // Every time a new point event happens, it will be drawn in this method
                SwingDrawing.DrawZigZag(owner, pointOne, pointTwo);
            }
        }
    }
}
