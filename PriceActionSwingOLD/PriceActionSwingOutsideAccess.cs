using NinjaTrader.Custom.Indicators.JiraiyaIndicators;
using NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwingOLD;

namespace NinjaTrader.NinjaScript.Indicators.JiraiyaIndicators
{
    public partial class PriceActionSwingOLD
    {
        public void OnPointCalculationUpdate(int pointsCount, Point pointOne, Point pointTwo)
        {
            // Every time a new point event happens, it will be drawn in this method
            SwingDrawing.DrawPoint(pointTwo);

            if (pointsCount > 1)
            {
                // Every time a new point event happens, it will be drawn in this method
                SwingDrawing.DrawZigZag(pointOne, pointTwo);
            }
        }
    }
}
