namespace NinjaTrader.Custom.Indicators.JiraiyaIndicators.PriceActionSwing
{
    public class TickCalculation : Calculation
    {
        protected override CalculationData CalculateEachBarSwingPoint()
        {
            LogPrinter.Print(launcher, "TickCalculation.CalculateEachBarSwingPoint()");

            bool isRising = highs[0] > highs[1];
            bool isFalling = lows[0] < lows[1];
            LogPrinter.Print(launcher, "isRising : " + isRising + ", isFalling: " + isFalling);
            
            bool isOverHighStrength = highs[0] > (LastLow().Price + (launcher.Strength * launcher.TickSize));
            bool isOverLowStrength = lows[0] < (LastHigh().Price - (launcher.Strength * launcher.TickSize));
            LogPrinter.Print(launcher, "isOverHighStrength : " + isOverHighStrength + ", isOverLowStrength : " + isOverLowStrength);

            if (isRising && isOverHighStrength)
                return new CalculationData(true, highs[0], launcher.CurrentBar, Point.SideSwing.High);
            if (isFalling && isOverLowStrength)
                return new CalculationData(true, lows[0], launcher.CurrentBar, Point.SideSwing.Low);
            
            return new CalculationData(false);
        }
    }
}
